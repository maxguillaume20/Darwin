using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RTS;

public class PreSpecButton2 : SpecializationButton
{
	private float[] sunCostArray = new float[] {1f, 0f, 0f, 0f, 0f};
	private float[] rockCostArray = new float[] { 1f, -1.5f, 0.11f, 1.8f, 0.25f};
	private float[] fireCostArray = new float[] { 0.37f, -0.34f, 0.6f, 2.34f, 0.23f};

	private float[] sunRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] rockRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] mainFireRewardArray = new float[] {0.18f, -0.37f, 4f, 3.2f, 0.7f};
	private float[] secondaryFireRewardArray = new float[] {0.05f, 0.001f, 22f, 4.7f, 2.3f};

	protected override void Awake ()
	{
		base.Awake ();
		era = Eras.StoneAge;
		namesArray = new string[] {"Sun", "Rock", "Fire"};
		chronologicalOrderArray = new int[] {5, 1, 2};
		disasterNamesArray = new string[] {"", "", ""};
		methodArray = new UnityEngine.Events.UnityAction[] {delegate {Sun();}, delegate {Rock();}, delegate {Fire();}};
		costVariablesList.Add (sunCostArray);
		costVariablesList.Add (rockCostArray);
		costVariablesList.Add (fireCostArray);
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, sunRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.Defense, rockRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.Attack, mainFireRewardArray}, {StatsType.ProtectionRates, secondaryFireRewardArray}});;
		messageArray = new string[] 
		{
			"-Unlocks Monument: Sun Monument", 

			"-Unlocks: Quarry", 

			"-Increases the Attack Damage of your Incendiary Units" +
			"\n-Increases the Protection level for Sheep, Bunnies, and Deer" +
			"\n-Unlocks: " + MonumentSelectionMenu.monSpecNameDick[SpecType.Fire] +
			"\n-Disables: " + MonumentSelectionMenu.monSpecNameDick[SpecType.Wheel]
		};
		specTypeArray = new SpecType[] {SpecType.Sun, SpecType.Rock, SpecType.Fire};
	}
	public void Sun () 
	{

	}
	
	public void Rock () 
	{

	}
	
	public void Fire() 
	{
		UpgradeAttackType (AttackType.Incendiary, 0);
		UpgradeAllSpeciesModelStats (StatsType.ProtectionRates);
	}
}
