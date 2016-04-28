using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class ClassButton2 : EvolutionPanelButton
{
	private float[] carrotCostArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] katanaCostArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] philosophyCostArray = new float[] {0.01f, 0.48f, 41f, 1.7f, 3.7f};

	private float[] carrotRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] katanaRewardArray = new float[] {0f, 0f, 0f, 0f, 0f};
	private float[] philosophyRewardArray = new float[] {0.22f, -0.39f, 3f, 1f, 1.2f};

	protected override void Awake ()
	{
		base.Awake ();
		namesArray = new string[] {"Carrot God", "Katana", "Philosophy"};
		methodArray = new UnityEngine.Events.UnityAction[] { delegate {CarrotGod();}, delegate {Katana();}, delegate {Philosophy();}};
		costVariablesList.Add (carrotCostArray);
		costVariablesList.Add (katanaCostArray);
		costVariablesList.Add (philosophyCostArray);
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, carrotRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.NotSetYet, katanaRewardArray}});
		rewardVariablesDickList.Add (new Dictionary<StatsType, float[]> {{StatsType.Attack, philosophyRewardArray}});
		messageArray = new string[] 
		{
			"", 
			"", 
			"-Increases the Attack Damage of your Psychological Units"
		};
	}
	
	public void CarrotGod() 
	{

	}
	
	public void Katana () 
	{

	}
	
	public void Philosophy() 
	{
		UpgradeAttackType (AttackType.Psychological, 0);
	}
}
