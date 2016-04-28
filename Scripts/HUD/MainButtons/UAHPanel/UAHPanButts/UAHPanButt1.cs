using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class UAHPanButt1 : UAHPanelButton 
{
//	private bool endDroppingGiantCarrots;
//
//	protected override void Awake() 
//	{
//		base.Awake ();
//		uahNames = new string[] {"GiantCarrot", "Gladiator", "Alchemist"};
//		switch (GameManager.HumanPlayer.species) 
//		{
//		case Species.Bunnies:
//			texticals[0].text = "Giant Carrot";
//			button.onClick.AddListener( delegate {DropGiantCarrot();});
//			texticals[1].text = "F: 100";
//			button.interactable = true;		
//			break;
//		case Species.Deer:
//			texticals[0].text = uahNames[1];
//			texticals[1].text = "";
//			era = Eras.Classical;
//			break;
//		case Species.Sheep:
//			texticals[0].text = uahNames[2];
//			texticals[1].text = "";
//			break;
//		}
//	}
//
//	public void DropGiantCarrot () 
//	{
//		if (EnoughResources(uahNames[0]))
//		{
//			GameManager.HumanPlayer.tempBuilding.EnableTempBuilding(uahNames[0]);
//			GameManager.uahPanel.gameObject.SetActive(false);
//			foreach (StrategicPoint stratpt in GameManager.HumanPlayer.stratPoints.ownedAllList)
//			{
//				if (stratpt.buildingArea) stratpt.buildingArea.gameObject.SetActive(true);
//			}
//			foreach (MainButton mb in GameManager.Hud.mainButtons) 
//			{
//				mb.gameObject.SetActive(false);
//			}
//			GameManager.checkButton.gameObject.SetActive (true);
//			GameManager.cancelButton.gameObject.SetActive (true);
//		}
//	}	
}
