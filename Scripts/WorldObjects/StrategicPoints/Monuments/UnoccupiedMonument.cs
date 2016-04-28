using UnityEngine;
using System.Collections;
using RTS;

public class UnoccupiedMonument : StrategicPoint 
{
	public Eras era;

	public void Enable () 
	{
		gameObject.SetActive (true);
		foreach (BuildingSlot bS in buildingArea.buildingSlots)
		{
			bS.Initiate (this);
		}
	}

	public void SetMonumentType(SpecType monSpecType) 
	{
		if (!player.unlockedMonumentsDick[era].Contains(monSpecType)) 
		{
			player.unlockedMonumentsDick[era].Add (monSpecType);
		}
		Monument newMonument = ((GameObject)Instantiate (GameManager.GetGameObject (MonumentSelectionMenu.monSpecNameDick [monSpecType]), transform.position, Quaternion.identity)).GetComponent<Monument> ();
		newMonument.name = MonumentSelectionMenu.monSpecNameDick [monSpecType];
		newMonument.Initiate (this);
		isAlive = false;
		occupied = false;
		player = null;
		SetLayer ();
		population = 0f;
		flag.GetComponent<SpriteRenderer> ().color = Color.white;
		flag.gameObject.SetActive (false);
		gameObject.SetActive (false);
	}

	public override void Occupy (Player newPlayer, float newPopulation)
	{
		isAlive = true;
		player = newPlayer;
		population = newPopulation;
		SetLayer ();
		occupied = true;
		if (flag) 
		{
			flag.GetComponent<SpriteRenderer> ().color = newPlayer.color;
			flag.gameObject.SetActive (true);
		}
		if (selected && GameManager.Hud.mainpanel.gameObject.activeSelf) 
		{
			Deselect ();
			SelectTap (GameManager.HumanPlayer);
		}
	}
	
	public override void SelectTap (Player controller)
	{
		base.SelectTap (controller);
		if (occupied) 
		{
			healthBar.gameObject.SetActive(false);
			//			GameManager.Hud.Infotext.healthText.gameObject.SetActive(false);
			MonumentSelectionMenu.OpenMenu (this);
			for (int i = 0; i < buttons.Count; i ++) 
			{
				GameManager.Hud.ClosePanelButton(buttons[i]);
			}
		}
	}

	public override void Deselect ()
	{
		base.Deselect ();
		MonumentSelectionMenu.thisGameObject.SetActive (false);
	}

	public override string GetNameText ()
	{
		return era.ToString () + " Monument";
	}

	public override string GetHealthText ()
	{
		if (occupied) 
		{
			return "Select Monument";
		}
		return base.GetHealthText ();
	}
}
