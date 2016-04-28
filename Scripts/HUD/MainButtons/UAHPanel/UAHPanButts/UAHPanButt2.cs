using UnityEngine;
using System.Collections;
using RTS;

public class UAHPanButt2 : UAHPanelButton 
{
	public Sprite warDrumsSprite;

	protected override void Awake() 
	{
		base.Awake ();
		uahNames = new string[] {"ApollosBow", "War Drums", "ManBearPig"};
		switch (GameManager.HumanPlayer.species) 
		{
		case Species.Bunnies:
			texticals[0].text = "Apollo's Bow";
			button.onClick.AddListener( delegate {StartCoroutine(DropApollosBow());});
			texticals[1].text = "F: 1";
			break;
		case Species.Deer:
			texticals[0].text = uahNames[1];
			texticals[1].text = "P: " + GameManager.GetGameObject ("WarDrumsAura").GetComponent<WarDrumsAura>().resCost;
			break;
		case Species.Sheep:
			texticals[0].text = uahNames[2];
			texticals[1].text = "";
			break;
		}
	}

	protected IEnumerator DropWarDrums() 
	{
		if (GameManager.HumanPlayer.GetResource(ResourceType.Unique) >= GameManager.GetGameObject ("WarDrumsAura").GetComponent<WarDrumsAura>().resCost) 
		{

		}
		else StartCoroutine(ChangeTextColorToRed());
		yield return null;
	}

	protected IEnumerator DropApollosBow() 
	{
		if (EnoughResources(uahNames[0]))
		{
			bool firstTap = false;
			while (Input.touchCount == 0 || !firstTap) 
			{
				if (Input.touchCount == 1 && GameManager.FingerInBounds(Input.GetTouch(0).position)) firstTap = true;
				yield return null;
			}
			Vector3 spawnPoint = Camera.main.ScreenToWorldPoint (new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch(0).position.y, GameManager.cameraHeight)); 
			GameManager.HumanPlayer.AddUnit(uahNames[0], spawnPoint);
		}
	}
}
