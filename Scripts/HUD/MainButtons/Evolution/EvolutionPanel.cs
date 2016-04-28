using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using RTS;
 

public class EvolutionPanel : MonoBehaviour 
{
	public EvolutionButton evolutionButton;
	public bool openNewEraOnNextEnable;
	private NewEraAnnouncement newEraAnnouncement;
	public EvolutionPanelButton[] evoPanButts;
	private int nextUnlockableEPB;
	public EvolutionPanelButton selectedEPB;
	public Sprite[] evoPanButtSprites;
	private EvoProgressBar evoProgressBar;
	public Text nameText;
	public Text messageText;
	public Text purchasedText;
	// [0] = eraWidener, [1] = nextEraThreshold
	private static Dictionary<Eras, float[]> eraTimeStatsDick { get; set; }
	public static Dictionary<Player, float> playerEvoProgressDick { get; set; }
	public Dictionary<Species, string> GetIt = new Dictionary<Species, string>();
	public UpgradesGraph upgradesGraph;

	private void Awake() 
	{ 
		playerEvoProgressDick = new Dictionary<Player, float> ();
		eraTimeStatsDick = new Dictionary<Eras, float[]> {{Eras.StoneAge, new float[] {1f, 1f/6f}}, {Eras.Classical, new float[] {2f, 1f/3f}}, {Eras.Renaissance, new float[] {3f, 1f/2f}}, {Eras.Industrial,new float[] {5f, 2f/3f}}, {Eras.Information, new float[] {8f, 1f}}};
		GameManager.evoPanel = this;
		GetIt.Add(Species.Bunnies, "Bestowed ");
		GetIt.Add (Species.Deer, "Seized ");
		GetIt.Add (Species.Sheep, "Learned ");
		GetComponentsInChildren<Image> ()[1].color = new Color (GameManager.HumanPlayer.color.r, GameManager.HumanPlayer.color.g, GameManager.HumanPlayer.color.b, 0.1f);
	}

	private void Start() 
	{
		evoProgressBar = GetComponentInChildren<EvoProgressBar> ();
		evoProgressBar.image.color = GameManager.HumanPlayer.color;
		newEraAnnouncement = GetComponentInChildren<NewEraAnnouncement> ();
		newEraAnnouncement.gameObject.SetActive (false);
		foreach (EvolutionPanelButton epb in evoPanButts) epb.SetPosition();
		Array.Sort (evoPanButts, delegate (EvolutionPanelButton epb1, EvolutionPanelButton epb2) {
			return epb1.position.CompareTo (epb2.position);
		});
		gameObject.SetActive (false);
	}

	private void OnEnable() 
	{
		GameManager.HumanPlayer.userInput.enabled = false;
		if (openNewEraOnNextEnable) 
		{
			StartCoroutine (newEraAnnouncement.NewEraBitches (GameManager.HumanPlayer.Era));
			openNewEraOnNextEnable = false;
		}
	}

	private void OnDisable() 
	{
		GameManager.HumanPlayer.GetComponent<UserInput> ().enabled = true;
		upgradesGraph.gameObject.SetActive (false);
		selectedEPB = null;
		nameText.text = "";
		messageText.text = "";
		purchasedText.text = "";
		if (newEraAnnouncement.gameObject.activeSelf) 
		{
			newEraAnnouncement.gameObject.SetActive(false);
		}
		foreach (MainButton mb in GameManager.Hud.mainButtons) 
		{
			mb.gameObject.SetActive(true);
		}
	}

	public Sprite GetEvoPanButtSprite(string spriteName)
	{
		foreach (Sprite sprite in evoPanButtSprites) 
		{
			if (sprite.name == spriteName) 
			{
				return sprite;
			}
		}
		return null;
	}

	public void ChangeEvoProgress(Player player, float amount) 
	{
		// if the player enters a new era
		if (playerEvoProgressDick[player] + amount / eraTimeStatsDick[player.Era][0] >= eraTimeStatsDick[player.Era][1]) 
		{
			float excessRes = (playerEvoProgressDick[player] + amount / eraTimeStatsDick[player.Era][0]  - eraTimeStatsDick[player.Era][1]) * eraTimeStatsDick[player.Era][0];
			float newAmount = amount - excessRes;
			playerEvoProgressDick [player] += newAmount / eraTimeStatsDick[player.Era][0];
			Eras nextEra = GameManager.orderEraDick[GameManager.eraOrderDick[player.Era] + 1];
			playerEvoProgressDick [player] += excessRes / eraTimeStatsDick[nextEra][0];
		}
		else 
		{
			playerEvoProgressDick [player] += amount / eraTimeStatsDick[player.Era][0];
		}
		if (player == GameManager.HumanPlayer && playerEvoProgressDick[player] < player.GetMaxTotalUnique()) 
		{
			evoProgressBar.rectTransform.anchorMax = new Vector2(playerEvoProgressDick[player], evoProgressBar.rectTransform.anchorMax.y);
			for (int i = nextUnlockableEPB; i < evoPanButts.Length; i++) 
			{
				SpecializationButton sp = evoPanButts[i] as SpecializationButton;
				if (evoPanButts[i].position <= playerEvoProgressDick[player] && (sp == null || !sp.disabled && (sp.Era == Eras.StoneAge || sp.initiated))) 
				{
					evoPanButts[i].button.interactable = true;
				}
				else if (evoPanButts[i].position > playerEvoProgressDick[player]) 
				{
					nextUnlockableEPB = i;
					break;
				}
			}
		}
		// if New Era
		if (playerEvoProgressDick[player] >= eraTimeStatsDick[player.Era][1] && player.Era != Eras.Information) 
		{
			player.NewEra();
			if (player == GameManager.HumanPlayer) 
			{
				GameManager.Hud.EraText.text = player.Era.ToString();
				BuildMenu.NextEra (player.Era);
				LocalUpgradesMenu.NewEra ();
				if (gameObject.activeSelf) 
				{
					StartCoroutine (newEraAnnouncement.NewEraBitches (player.Era));
				}
				else 
				{
					openNewEraOnNextEnable = true;
					GameManager.Hud.StartFlashButton(evolutionButton.image, new BoolMethod (delegate {return openNewEraOnNextEnable;})); 
				}
			} 
		}
	}

	public void GoBack() 
	{
		gameObject.SetActive (false);
	}
}

