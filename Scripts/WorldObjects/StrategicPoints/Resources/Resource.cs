using UnityEngine;
using System.Collections;
using RTS;

public class Resource : StrategicPoint  
{
	// [0] = gatheringAmount, [1] = gatheringTime
	public float[] resourceStatsArray;
	public ResourceType resource;

	protected IEnumerator GatherResources() 
	{
		while (occupied && currentMobCount > 0) 
		{
			yield return new WaitForSeconds (resourceStatsArray[1]);
			if (occupied && currentMobCount > 0) 
			{
				float resAmount = resourceStatsArray[0] * currentMobCount;
				player.ChangeResource (resource, resAmount);
				if (player == GameManager.HumanPlayer) 
				{
					HUD.resDisplayer.ActivateResourceDisplay (transform.position, GetSpecies(), resource, (int) resAmount);
				}
			}
		}
	}

	protected override void MakeLocalUpgrades ()
	{
		// Increase gathering amount
		localUpgradesList.Add (new LocalUpgrade[1]);
		localUpgradesList[0][0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.ResourceStats, 0, 0.5f, "-Increase gathering amount\n per " + unitName + " by \r %", new float[3], 1.5f);
		localUpgradesList [0] [0].descriptionAsPercentage = true;
		localUpgradesList [0] [0].miscUpgradeSprite = LocalUpgradesMenu.statSpritesDick [StatsType.TimeStats];
		// Increase max units
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] secondCostArray = new float[] {0f, 0f, 30f};
		localUpgradesList [1] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 0, 1f, "-Increase max " + unitName + "s by \r", secondCostArray, 1.5f);
		localUpgradesList [1] [0].asAddition = true;
		localUpgradesList [1] [0].messageArray = new string[] {"IncreaseIdealUnits"};
		if (buildingArea) 
		{
			// Increase building health, crush defense, and buildingArea
			localUpgradesList.Add (new LocalUpgrade[2]);
			float[] thirdCostArray = new float[] {0f, 200f, 0f};
			localUpgradesList [2] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Health, 1, 0.15f, "-Increase building health \nby \r hp\n", thirdCostArray, 1.3f);
			localUpgradesList [2] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Defense, 2, 0.12f, "-Increase building crush\ndefense by \r%\n");
			localUpgradesList [2] [1].asAddition = true;
			localUpgradesList [2] [1].descriptionAsPercentage = true;
//			localUpgradesList [2] [2] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.StratStats, 0, 0.10f, "-Increase the building area \nradius by \r%\n");
//			localUpgradesList [2] [2].descriptionAsPercentage = true;
//			localUpgradesList[2][2].miscUpgradeSprite = LocalUpgradesMenu.statSpritesDick[StatsType.RangedStats];
//			localUpgradesList [2] [2].messageArray = new string[] {"SetBuildingAreaSize"}; 
		}
	}

	protected override void CreateMob ()
	{
		base.CreateMob ();
		if (currentMobCount == 1) StartCoroutine(GatherResources());
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.ResourceStats, resourceStatsArray);
	}
}
