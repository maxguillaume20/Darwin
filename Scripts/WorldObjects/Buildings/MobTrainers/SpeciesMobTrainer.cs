using UnityEngine;
using System.Collections;

public class SpeciesMobTrainer : MobTrainer 
{
	public int idealMobCount;
	public int mobPopCount;

	public int GetCurrentUnits() 
	{
		return currentMobCount;
	}

	public void ChangeIdealUnits(int amount) 
	{
		idealMobCount += amount;
		if (amount < 0 && idealMobCount < GetCurrentUnits()) 
		{
			DestroyMob();
		}
		StartTraining ();
	}

	// called by local upgrades with messages
	private void IncreaseIdealUnits () 
	{
		idealMobCount = (int)mobTrainerStatsArray[0];
		StartTraining ();
	}

	public override string GetTrainingText() 
	{
		return unitName + "s\n" + idealMobCount + " / " + mobTrainerStatsArray[0] + "\n" + currentMobCount;
	}

	protected virtual void DestroyMob () 
	{
		currentMobCount --;
	}
}
