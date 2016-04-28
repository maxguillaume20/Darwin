using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class MonumentSelectionMenu : MonoBehaviour 
{
	public static GameObject thisGameObject { get; set; }
	private static UnoccupiedMonument selectedMonument { get; set; }
	private static MonumentSelectionPanel monSelectionPanel { get; set; }
	public static Dictionary <Species, Dictionary<Eras, Dictionary<string, SpecType>>> speciesMonumentTypeDick { get; set; }
	public static Dictionary<SpecType, string> monSpecNameDick { get; set; }
	private static Dictionary <Button, bool> buttonIsSelected { get; set; }
	public int lockedPopCost = 10;

	private void Awake() 
	{
		thisGameObject = gameObject;
		monSelectionPanel = GetComponentInChildren<MonumentSelectionPanel> ();
//		monSelectionPanel.resImages [2].sprite = HUD.speciesResourceSpriteDick [GameManager.HumanPlayer.species][ResourceType.Unique];
		Species[] humanSpeciesArray = new Species[] {Species.Bunnies, Species.Deer, Species.Sheep};
		Eras[] specErasArray = new Eras[] {Eras.StoneAge, Eras.Renaissance, Eras.Information};
		speciesMonumentTypeDick = new Dictionary<Species, Dictionary<Eras, Dictionary<string, SpecType>>> ();
		for (int i = 0; i < humanSpeciesArray.Length; i ++) 
		{
			speciesMonumentTypeDick.Add (humanSpeciesArray[i], new Dictionary<Eras, Dictionary<string, SpecType>>());
			for (int j = 0; j < specErasArray.Length; j ++) 
			{
				speciesMonumentTypeDick[humanSpeciesArray[i]].Add (specErasArray[j], new Dictionary<string, SpecType>());
			}
		}
		speciesMonumentTypeDick [Species.Bunnies] [Eras.StoneAge].Add ("Nature Monument", SpecType.Nature);
		speciesMonumentTypeDick [Species.Bunnies] [Eras.StoneAge].Add ("Sun Monument", SpecType.Sun);
		speciesMonumentTypeDick [Species.Deer] [Eras.StoneAge].Add ("Rock Monument", SpecType.Rock);
		speciesMonumentTypeDick [Species.Deer] [Eras.StoneAge].Add ("Stick Monument", SpecType.Stick);
		speciesMonumentTypeDick [Species.Sheep] [Eras.StoneAge].Add ("FireTower", SpecType.Fire);
		speciesMonumentTypeDick [Species.Sheep] [Eras.StoneAge].Add ("Pottery", SpecType.Wheel);
		monSpecNameDick = new Dictionary<SpecType, string> ();
		foreach (Species species in speciesMonumentTypeDick.Keys) 
		{
			foreach (Eras era in speciesMonumentTypeDick[species].Keys) 
			{
				foreach (string name in speciesMonumentTypeDick[species][era].Keys) 
				{
					monSpecNameDick.Add (speciesMonumentTypeDick[species][era][name], name);
				}
			}
		}

//		lockedMonumentCostArray = new float[] {1000f, 1000f, 500f};
		buttonIsSelected = new Dictionary<Button, bool> ();
		for (int i = 0; i < monSelectionPanel.selectionButtons.Length; i ++) 
		{
			buttonIsSelected.Add (monSelectionPanel.selectionButtons[i], false);
		}
		monSelectionPanel.InitializePanel ();

		gameObject.SetActive (false);
	}

	public static void OpenMenu(UnoccupiedMonument newMonument) 
	{
		thisGameObject.SetActive (true);
		selectedMonument = newMonument;
		List <string> monNamesList = new List<string> ();
		foreach (string monName in speciesMonumentTypeDick[newMonument.GetSpecies()][newMonument.era].Keys) 
		{
			monNamesList.Add(monName);
		}
		for (int i = 0; i < monSelectionPanel.selectionButtons.Length; i ++) 
		{
			monSelectionPanel.selectionButtons[i].GetComponentInChildren<Text>().text = monNamesList[i];
		}
		monSelectionPanel.OpenPanel (selectedMonument);
	}

	public void SelectMonument (Button selectionButton, SpecType monSpecType, int popCost) 
	{
		if (buttonIsSelected[selectionButton]) 
		{
			if ((int) selectedMonument.population >= popCost && (int) Pop_Dynamics_Model.modelStatsDick[selectedMonument.GetSpecies ()][StatsType.Population] > popCost) 
			{
				Pop_Dynamics_Model.modelStatsDick [GameManager.HumanPlayer.species][StatsType.Population] -= popCost;
				selectedMonument.ChangeLocalPopulation (-popCost);
				selectedMonument.SetMonumentType (monSpecType);
				gameObject.SetActive (false);
				GameManager.Hud.ClosePanel();
			}
			else if ((int) selectedMonument.population >= popCost)
			{
				HUD.StartChangeTextColorToRed (new Text[] {monSelectionPanel.popText, GameManager.Hud.populationText});
			}
			else 
			{
				HUD.StartChangeTextColorToRed (new Text[] {monSelectionPanel.popText, GameManager.Hud.Infotext.popText});
			}
			buttonIsSelected[selectionButton] = false;
		}
		else 
		{
			for (int i = 0; i < monSelectionPanel.selectionButtons.Length; i ++) 
			{
				buttonIsSelected[monSelectionPanel.selectionButtons[i]] = false;
			}
			buttonIsSelected[selectionButton] = true;
			StartCoroutine (HighLightSelectionButton (selectionButton));
		}
	}

	private IEnumerator HighLightSelectionButton (Button selectionButton) 
	{
		selectionButton.image.color = selectionButton.colors.highlightedColor;
		for (float time = 0f; time < GameManager.doubleTapButtonTime && buttonIsSelected[selectionButton]; time += Time.deltaTime) yield return null;
		buttonIsSelected [selectionButton] = false;
		selectionButton.image.color = selectionButton.colors.normalColor;
	}

	private void OnDisable() 
	{
		for (int i = 0; i < monSelectionPanel.selectionButtons.Length; i ++) 
		{
			buttonIsSelected[monSelectionPanel.selectionButtons[i]] = false;
			monSelectionPanel.selectionButtons[i].image.color = monSelectionPanel.selectionButtons[i].colors.normalColor;
		}
	}
}
