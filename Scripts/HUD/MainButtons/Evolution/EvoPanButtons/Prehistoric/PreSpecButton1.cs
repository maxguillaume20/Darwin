using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class PreSpecButton1 : SpecializationButton 
{
	private float[] natureCostArray = new float[] {1f, 0f, 0f, 0f, 0f};
	private float[] stickCostArray = new float[] {1f, 0f, 0f, 0f, 0f};
	private float[] wheelCostArray = new float[] {0.03f, 0.003f, 41f, 1.7f, 3.7f};

	private float[] natureRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] stickRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] wheelMainRewardArray = new float[] {-0.064f, 0.001f, 22f, 0.3f, 1f};
	private float[] wheelSecondaryRewardArray = new float[] {0.02f, 0.001f, 22f, 0.3f, 1f};

	protected override void Awake ()
	{
		base.Awake ();
		era = Eras.StoneAge;
		namesArray = new string[] {"Nature", "Stick", "The Wheel"};
		chronologicalOrderArray = new int[] {5, 1, 8};
		methodArray = new UnityEngine.Events.UnityAction[] {delegate {Nature();}, delegate {Stick();}, delegate {Wheel();}};
		costVariablesList.Add (natureCostArray);
		costVariablesList.Add (stickCostArray);
		costVariablesList.Add (wheelCostArray);
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, natureRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, stickRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.ResourceStats, wheelMainRewardArray}, {StatsType.MobileStats, wheelSecondaryRewardArray}});
		messageArray = new string[] 
		{
			"-Unlocks: Nature Monument",

			"-Unlocks: Stick Monument",

			"-Decreases the Gathering Times at your Gold Mines and Lumberyards" +
			"\n-Increase Movement Speed of Siege Weapons for All Species" +
			"\n-Unlocks: " + MonumentSelectionMenu.monSpecNameDick[SpecType.Wheel] +
			"\n-Disables: " + MonumentSelectionMenu.monSpecNameDick[SpecType.Fire]
		};
		specTypeArray = new SpecType[] {SpecType.Nature, SpecType.Stick, SpecType.Wheel};
	}

	public void Nature () 
	{

	}

	public void Stick () 
	{

	}

	public void Wheel() 
	{
		UpgradeWorldObjectType (WorldObjectType.Resource, StatsType.ResourceStats, 1);
		UpgradeAllSpeciesWOT (WorldObjectType.Siege, StatsType.MobileStats, 0);
	}
}
