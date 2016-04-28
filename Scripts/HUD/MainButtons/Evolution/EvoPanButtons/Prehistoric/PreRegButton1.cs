using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class PreRegButton1 : EvolutionPanelButton
 {
	private float[] burrowCostArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] wardrumsCostArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] domesticationCostArray = new float[] {0.05f, 0.003f, 41f, 1.7f, 3.7f};

	private float[] burrowRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] wardrumsRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] domesticationRewardMainArray = new float[]{-0.05f, 0.001f, 22f, 4.7f, 2.3f};
	private float[] domesticationRewardSecondaryArray = new float[]{0.405f, -1.3f, 1f, 0f, 0.5f};

	protected override void Awake ()
	{
		base.Awake ();
		namesArray = new string[] {"Burrow", "War Drums", "Domestication"};
		chronologicalOrderArray = new int[] {1, 5, 5};
		methodArray = new UnityEngine.Events.UnityAction[] {delegate {Burrow();},delegate {WarDrums();}, delegate {Domestication();}};
		costVariablesList.Add (burrowCostArray);
		costVariablesList.Add (wardrumsCostArray);
		costVariablesList.Add (domesticationCostArray);
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, burrowRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.Attack, wardrumsRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.EatRates, domesticationRewardMainArray}, {StatsType.Health, domesticationRewardSecondaryArray}});
		messageArray = new string[] 
		{
			"",

			"",

			"-Decreases the the Wolf Predation Rate on Sheep" +
			"\n-Increases the Health of the Dogs from your DogHouses"
		};
	}

	private void Burrow ()
	{

	}

	private void WarDrums () 
	{

	}
	
	private void Domestication() 
	{
		ChangeEatRates (Species.Wolves, Species.Sheep);
		UpgradeWorldObject ("Dog", StatsType.Health, 1);
	}
}
