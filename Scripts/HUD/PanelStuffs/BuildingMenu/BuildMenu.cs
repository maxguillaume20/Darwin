using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using RTS;

public class BuildMenu : PanelButton 
{
	public static Dictionary<string, Dictionary<ResourceType, float>> buildingCostDick { get; set; }
	private BuildingMenuPanel buildingMenuPanel;
	public Sprite[] buildingSlotSprites;
	private static Sprite[] staticBuildingSlotSprites { get; set; }

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = PanelButtonType.BuildMenu;
		BuildMenu.buildingCostDick = new Dictionary<string, Dictionary<ResourceType, float>> ();
		buildingMenuPanel = GetComponentInChildren<BuildingMenuPanel> ();
		staticBuildingSlotSprites = buildingSlotSprites;
		buildingMenuPanel.SetSize (this);
	}

	protected override void OnEnable()
	{
		base.OnEnable ();
		foreach (StrategicPoint stratpt in GameManager.HumanPlayer.stratPoints.ownedAllList)
		{
			stratpt.EnableBuildingArea ();
		}
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();
		foreach (StrategicPoint stratpt in GameManager.HumanPlayer.stratPoints.ownedAllList)
		{
			if (stratpt.buildingArea) stratpt.buildingArea.gameObject.SetActive(false);
		}
	}

	public void StartPlacement(string buildingname, Text[] restexts)
	{
		if (EnoughResources(buildingname, restexts))
		{
			GameManager.HumanPlayer.tempBuilding.EnableTempBuilding(buildingname);
			GameManager.Hud.ClosePanel ();
			foreach (StrategicPoint stratpt in GameManager.HumanPlayer.stratPoints.ownedAllList)
			{
				stratpt.EnableBuildingArea ();
			}
			foreach (MainButton mb in GameManager.Hud.mainButtons) 
			{
				mb.gameObject.SetActive(false);
			}
			GameManager.checkButton.gameObject.SetActive (true);
			GameManager.checkButton.GetComponent<Button>().onClick.AddListener( delegate {SetBuilding(); });
			GameManager.cancelButton.gameObject.SetActive (true);
			GameManager.cancelButton.GetComponent<Button>().onClick.AddListener( delegate {CancelPlacement(); });
		}
	}

	public void SetBuilding()
	{
		if (GameManager.HumanPlayer.tempBuilding.isPlaceable && EnoughResources(GameManager.HumanPlayer.tempBuilding.name, GameManager.Hud.ResourceTexts.Values.ToArray()))
		{
			foreach (MainButton mb in GameManager.Hud.mainButtons) 
			{
				mb.gameObject.SetActive(true);
			}
			GameManager.checkButton.GetComponent<Button>().onClick.RemoveAllListeners();
			GameManager.cancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
			GameManager.checkButton.gameObject.SetActive (false);
			GameManager.cancelButton.gameObject.SetActive (false);;
			foreach (StrategicPoint stratpt in GameManager.HumanPlayer.stratPoints.ownedAllList)
			{
				if (stratpt.buildingArea) stratpt.buildingArea.gameObject.SetActive(false);
			}
			GameManager.HumanPlayer.tempBuilding.PlaceBuilding();
			IncreaseCost(GameManager.HumanPlayer.tempBuilding.name);
			GameManager.Hud.ClosePanel();
		}
	}

	public void CancelPlacement()
	{
		foreach (MainButton mb in GameManager.Hud.mainButtons) 
		{
			mb.gameObject.SetActive(true);
		}
		GameManager.checkButton.GetComponent<Button>().onClick.RemoveAllListeners();
		GameManager.cancelButton.GetComponent<Button>().onClick.RemoveAllListeners();
		GameManager.checkButton.gameObject.SetActive (false);
		GameManager.cancelButton.gameObject.SetActive (false);
		GameManager.HumanPlayer.tempBuilding.gameObject.SetActive (false);
		GameManager.HumanPlayer.userInput.SelectCapital ();
		GameManager.Hud.Infotext.gameObject.SetActive (false);
		gameObject.SetActive (true);
		CloseWOButtons ();
		GameManager.Hud.OpenPanelButton (PanelButtonType.BackButton);
		BackButton.SetGameObjectsToOpen (new GameObject[] {GameManager.Hud.Infotext.gameObject});
		BackButton.SetGameObjectsToClose (new GameObject[] {gameObject});
	}

	public static void NextEra(Eras nextEra) 
	{
		foreach (Button buildingButton in BuildingMenuPanel.buildingEraDick.Keys) 
		{
			if (BuildingMenuPanel.buildingEraDick[buildingButton] == nextEra) 
			{
				buildingButton.interactable = true;
			}
		}
	}

	private void IncreaseCost (string buildingName) 
	{
		Building building = GameManager.GetGameObject (buildingName).GetComponent<Building> ();
		List<ResourceType> resourceList = buildingCostDick [buildingName].Keys.ToList ();
		foreach (ResourceType resource in resourceList) 
		{
			buildingCostDick[buildingName][resource] = buildingCostDick[buildingName][resource] * building.multiBuildingExp; 
		}
		BuildingMenuPanel.ChangeCostText (buildingName);
	}

	public static Sprite SetBuildingSlotSprite (bool isOccupied) 
	{
		if (isOccupied) 
		{
			return staticBuildingSlotSprites[0];
		}
		else 
		{
			return staticBuildingSlotSprites[1];
		}
	}
}
