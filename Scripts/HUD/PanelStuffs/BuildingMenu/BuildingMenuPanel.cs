using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public class BuildingMenuPanel : MonoBehaviour 
{
	private BuildMenu buildMenu;
	public static Dictionary<Button, Text[]> buildingsTextDick { get; set; }
	public static Dictionary<Button, Eras> buildingEraDick { get; set; }
	public Building[] buildingArray;
	public string[] buildingsNameArray;
	public Button originalBuildingButton;
	public Image originalResImage;
	public Text originalResText;
	private float menuHeight;
	private float buildingButtonHeight = 0.05f;
	private float buildingButtonWidth = 0.7f;
	private float menuEditorHeight;
	private float buildingInfoHeight;
	private float insetY = 0.02f;
	private float insetX = 0.15f;
	private float resOffsetY = 0.01f;
	//	private float resOffsetX = 0.05f;
	private float resHeight = 0.02f;
	//	private float resWidth = 0.30f;

	public void SetSize (BuildMenu thisBuildMenu) 
	{
		buildMenu = thisBuildMenu;
		buildingsTextDick = new Dictionary<Button, Text[]> ();
		// includes strat points
		List<string> allBuildingNamesList = GameManager.GetSpeciesWOTList (GameManager.HumanPlayer.species, WorldObjectType.Building);
		List<string> buildingNamesList = new List<string>();
		foreach (string buildingName in allBuildingNamesList) 
		{
			StrategicPoint stratPoint = GameManager.GetGameObject(buildingName).GetComponent<StrategicPoint>();
			if (stratPoint == null || (stratPoint as Monument == null && stratPoint.originalSpecies != Species.NonPlayer)) 
			{
				buildingNamesList.Add (buildingName);
			}
		}
		buildingArray = new Building [buildingNamesList.Count];
		// sort the buildings by era
		for (int i = 0; i < buildingNamesList.Count; i ++) 
		{
			buildingArray[i] = GameManager.GetGameObject(buildingNamesList[i]).GetComponent<Building>();
		}
		Array.Sort (buildingArray, delegate (Building building1, Building building2) {
			return GameManager.eraOrderDick[building1.buildingEra].CompareTo(GameManager.eraOrderDick[building2.buildingEra]);
		});
		buildingsNameArray = new string[buildingArray.Length];
		for (int i = 0; i < buildingArray.Length; i ++) buildingsNameArray[i] = buildingArray[i].name;
		menuEditorHeight = Screen.height * 2;
		buildingInfoHeight = insetY + (originalBuildingButton.GetComponent<RectTransform> ().anchorMax.y - originalResImage.rectTransform.anchorMin.y);
		menuHeight = buildingsNameArray.Length * buildingInfoHeight * menuEditorHeight;
		RectTransform rectTransform = GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (Screen.width * (MainPanel.rectTransform.anchorMax.x - MainPanel.rectTransform.anchorMin.x), menuHeight); 
		// Setting the first buildingButton and shit
		float[] firstBuildingCostArray = GameManager.GetGameObject (buildingsNameArray [0]).GetComponent<Building> ().buildingCostArray;
		BuildMenu.buildingCostDick.Add (buildingsNameArray[0], new Dictionary<ResourceType, float> ());
		for (int i = 0; i < firstBuildingCostArray.Length; i ++) 
		{
			if (firstBuildingCostArray[i] > 0) 
			{
				BuildMenu.buildingCostDick[buildingsNameArray[0]].Add(GameManager.resourceTypeArraytoDick[i], firstBuildingCostArray[i]);
			}
		}
		buildingEraDick = new Dictionary<Button, Eras> ();
		originalBuildingButton.GetComponentInChildren<Text> ().text = buildingsNameArray[0];
		originalBuildingButton.name = buildingsNameArray [0];
		RectTransform obRectTransform = originalBuildingButton.GetComponent<RectTransform> ();
		obRectTransform.anchorMax = new Vector2 (insetX + buildingButtonWidth, 1f - insetY * menuEditorHeight / menuHeight);
		obRectTransform.anchorMin = new Vector2 (insetX, 1f - (insetY + buildingButtonHeight) * menuEditorHeight / menuHeight);
		float lastAnchorMaxY = obRectTransform.anchorMax.y;
		Text[] obTexts = SetCostDisplay (buildingsNameArray[0], originalResImage, originalResText, lastAnchorMaxY);
		originalBuildingButton.onClick.AddListener(delegate {buildMenu.StartPlacement(buildingsNameArray[0], obTexts);});
		buildingsTextDick.Add (originalBuildingButton, obTexts);
		buildingEraDick.Add (originalBuildingButton, buildingArray [0].buildingEra);
		// Andd the rest
		for (int i = 1; i < buildingsNameArray.Length; i ++) 
		{
			lastAnchorMaxY = AddBuildingToMenu(i, GameManager.GetGameObject (buildingsNameArray [i]).GetComponent<Building> ().buildingCostArray, lastAnchorMaxY);
		}
	}
	
	private float AddBuildingToMenu (int index, float[] buildingCostArray, float lastAnchorMaxY) 
	{
		BuildMenu.buildingCostDick.Add (buildingsNameArray[index], new Dictionary<ResourceType, float> ());
		Button buildingButton = Instantiate<Button> (originalBuildingButton);
		buildingButton.GetComponentInChildren<Text> ().text = buildingsNameArray[index];
		buildingButton.name = buildingsNameArray [index];
		float thisAnchorMaxY = lastAnchorMaxY - buildingInfoHeight * menuEditorHeight / menuHeight;
//		float thisAnchorMaxY = lastAnchorMaxY - 0.1f * Screen.height * 2 / menuHeight;
		RectTransform buttonRectTransform = buildingButton.GetComponent<RectTransform> ();
		SetDisplayRectTransforms (buttonRectTransform);
		buttonRectTransform.anchorMin = new Vector2 (insetX, thisAnchorMaxY - buildingButtonHeight  * menuEditorHeight / menuHeight);
		buttonRectTransform.anchorMax = new Vector2 (insetX + buildingButtonWidth, thisAnchorMaxY);
		for (int i = 0; i < buildingCostArray.Length; i ++) 
		{
			if (buildingCostArray[i] > 0) 
			{
				BuildMenu.buildingCostDick[buildingsNameArray[index]].Add(GameManager.resourceTypeArraytoDick[i], buildingCostArray[i]);
			}
		}
		Image firstResImage = Instantiate<Image> (originalResImage);
		Text firstResText = Instantiate<Text> (originalResText);
		Text[] resTexts = SetCostDisplay (buildingsNameArray[index], firstResImage, firstResText, thisAnchorMaxY);
		buildingButton.onClick.AddListener(delegate {buildMenu.StartPlacement(buildingsNameArray[index], resTexts);});
		buildingsTextDick.Add (buildingButton, resTexts);
		buildingEraDick.Add (buildingButton, buildingArray [index].buildingEra);
		if (buildingEraDick[buildingButton] != Eras.StoneAge) buildingButton.interactable = false;
		return thisAnchorMaxY;
	}
	
	private Text[] SetCostDisplay(string buildingName, Image firstResImage, Text firstResText, float thisTopAnchor) 
	{
		float resBottom = thisTopAnchor - (buildingButtonHeight + resOffsetY + resHeight) * menuEditorHeight / menuHeight;
		float resTop = thisTopAnchor - (buildingButtonHeight + resOffsetY) * menuEditorHeight / menuHeight;
		float resBaseXOffSet = -0.1666666f * (BuildMenu.buildingCostDick [buildingName].Count - 1);
		List<Image> resImageList = new List<Image> {firstResImage};
		List<Text> resTextList = new List<Text> {firstResText};
		ResourceType[] resourcesCostArray = BuildMenu.buildingCostDick [buildingName].Keys.ToArray ();
		if (resourcesCostArray.Length > 1) 
		{
			for (int i = 1; i < resourcesCostArray.Length; i ++) 
			{
				Image secondResImage = Instantiate<Image> (originalResImage);
				resImageList.Add(secondResImage);
				Text secondResText = Instantiate<Text> (originalResText);
				resTextList.Add(secondResText);
			}
		}
		for (int i = 0; i < BuildMenu.buildingCostDick[buildingName].Count; i ++) 
		{
			resImageList[i].sprite = HUD.speciesResourceSpriteDick[GameManager.HumanPlayer.species][resourcesCostArray[i]];
			resTextList[i].text = ((int)BuildMenu.buildingCostDick[buildingName][resourcesCostArray[i]]).ToString();
			RectTransform resImageRectTransform = resImageList[i].GetComponent<RectTransform>();
			SetDisplayRectTransforms (resImageRectTransform);
			resImageRectTransform.anchorMin = new Vector2 (0.35f + resBaseXOffSet + i * 0.33333333f, resBottom);
			resImageRectTransform.anchorMax = new Vector2 (0.45f + resBaseXOffSet + i * 0.3333333f, resTop);
			RectTransform resTextRectTransform = resTextList[i].GetComponent<RectTransform>();
			SetDisplayRectTransforms (resTextRectTransform);
			resTextRectTransform.anchorMin = new Vector2 (0.45f + resBaseXOffSet + i * 0.333333333f, resBottom);
			resTextRectTransform.anchorMax = new Vector2 (0.65f + resBaseXOffSet + i * 0.333333333f, resTop);
		}
		return resTextList.ToArray ();
	}
	
	private void SetDisplayRectTransforms (RectTransform newRectTransform) 
	{
		newRectTransform.SetParent(transform);
		newRectTransform.offsetMax = Vector2.zero;
		newRectTransform.offsetMin = Vector2.zero;
		newRectTransform.localScale = Vector3.one;
	}

	public static void ChangeCostText (string buildingName) 
	{
		Button buildingButton = GetBuildingButton (buildingName);
		ResourceType[] resourcesCostArray = BuildMenu.buildingCostDick [buildingName].Keys.ToArray ();
		for (int i = 0; i < BuildMenu.buildingCostDick[buildingName].Count; i ++) 
		{
			buildingsTextDick[buildingButton][i].text = ((int)(BuildMenu.buildingCostDick[buildingName][resourcesCostArray[i]])).ToString();
		}
	}

	private static Button GetBuildingButton (string buildingName) 
	{
		foreach (Button button in buildingsTextDick.Keys) 
		{
			if (buildingName == button.name) 
			{
				return button;
			}
		}
		return null;
	}
}
