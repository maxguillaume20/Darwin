using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RTS;

public class ClassButton1 : EvolutionPanelButton 
{
	private float[] apollosCostArray = new float[] {1f, 0f, 0f, 0f, 0f};
	private float[] catapultsCostArray = new float[] {1f, 0f, 0f, 0f, 0f};
	private float[] geometryCostArray = new float[] {0.14f, 0.32f, 0f, 0f, 1f};

	private float[] apollosRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] catapultsRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] geometryMainRewardArray = new float[] {0.113f, 0.2f, 22f, 0.3f, 1f};
	private float[] geometrySecondaryRewardArray = new float[] {0.2f, 0.4f, 0f, 0f, 1f};


	protected override void Awake ()
	{
		base.Awake ();

		namesArray = new string[] {"Apollo's Arrow", "Catapults", "Geometry"};
		methodArray = new UnityEngine.Events.UnityAction[] { delegate {ApollosArrow();}, delegate {Catapults();}, delegate {Geometry();}};
		costVariablesList.Add (apollosCostArray);
		costVariablesList.Add (catapultsCostArray);
		costVariablesList.Add (geometryCostArray);
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, apollosRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, catapultsRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.RangedStats, geometryMainRewardArray}, {StatsType.Defense, geometrySecondaryRewardArray}});
		messageArray = new string[] 
		{
			"",

			"",

			"-Increases the Range of your Ranged Units and Towers" +
			"\n-Increases the Crush Defense of All your Buildings"
		};
	}

	public void ApollosArrow() 
	{

	}

	public void Catapults () 
	{

	}
	
	public void Geometry() 
	{
		UpgradeWorldObjectType (WorldObjectType.Building, StatsType.Defense, 2);
		UpgradeWorldObjectType (WorldObjectType.Ranged, StatsType.RangedStats, 0);
	}
}
