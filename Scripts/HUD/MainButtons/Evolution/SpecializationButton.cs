using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class SpecializationButton : EvolutionPanelButton
{
	public bool initiated;
	public SpecializationButton[] lineageButtons;
	public int opposites;
	protected int[] lineageIntArray;
	protected string[] disasterNamesArray;
	protected SpecType[] specTypeArray;
	protected Eras era;
	public SpecializationButton otherSpecButton;
	public bool disabled;

	protected override void Awake() 
	{
		base.Awake ();
		button.onClick.AddListener(delegate {FirstSelect();});
	}

	protected override void Start ()
	{
		base.Start ();
		lineageIntArray = new int[namesArray.Length];
		for (int i = 0; i < lineageIntArray.Length; i++) 
		{
			lineageIntArray[i] = 2 * i + opposites;
		}
	}

	public virtual void FirstSelect() 
	{
		if (GameManager.evoPanel.selectedEPB != this)
		{
			if (!GameManager.HumanPlayer.unlockedMonumentsDick[era].Contains (specTypeArray[mainArrayIndex])) 
			{
				GameManager.HumanPlayer.unlockedMonumentsDick[era].Add (specTypeArray[mainArrayIndex]);
			}
			otherSpecButton.button.interactable = false;
			otherSpecButton.disabled = true;
//			if (disasterNamesArray != null && disasterNamesArray[mainArrayIndex] != "") GameManager.disasters.StartDisaster(disasterNamesArray[mainArrayIndex]);
			lineageButtons [0].Initiate (lineageIntArray[mainArrayIndex]);
			lineageButtons [1].Initiate (lineageIntArray[mainArrayIndex]);
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(delegate {Upgrade();});
		}
	}
	
	protected virtual void Initiate(int i) 
	{
		initiated = true;
		if (GameManager.eraOrderDick[GameManager.HumanPlayer.Era] >= GameManager.eraOrderDick[Era]) 
		{
			button.interactable = true;
		}
		mainArrayIndex = i;
		images[1].sprite = GameManager.evoPanel.GetEvoPanButtSprite(namesArray[mainArrayIndex]);
	}

	protected void UpgradeAllSpeciesModelStats (StatsType statsType) 
	{
		foreach (Species upgradeSpecies in GameManager.speciesArray) 
		{
			if (upgradeSpecies != Species.Wolves) 
			{
				Pop_Dynamics_Model.modelStatsDick[upgradeSpecies][statsType] += Pop_Dynamics_Model.GetOriginalStat(upgradeSpecies, statsType) * RankIncrease (rank, rewardVariablesDickList[mainArrayIndex][statsType][0], rewardVariablesDickList [mainArrayIndex][statsType][1], rewardVariablesDickList[mainArrayIndex][statsType][2], rewardVariablesDickList[mainArrayIndex][statsType][3], rewardVariablesDickList[mainArrayIndex][statsType][4]); 
			}
		}
	}

	protected void UpgradeAllSpeciesWOT (WorldObjectType woType, StatsType statsType, int statsIndex) 
	{
		List<string> woNamesList = new List<string> ();
		foreach (Species thisSpecies in GameManager.speciesArray) 
		{
			woNamesList.AddRange (GameManager.GetSpeciesWOTList (thisSpecies, woType));
		}
		foreach (string woName in woNamesList) 
		{
			UpgradeWorldObject (woName, statsType, statsIndex);
		}
	}
}
