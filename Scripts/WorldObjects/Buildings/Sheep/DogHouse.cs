using UnityEngine;
using System.Collections;
using RTS;

public class DogHouse : Sentry 
{
	protected override void MakeLocalUpgrades ()
	{
		// Increase unit attack and health
		localUpgradesList.Add (new LocalUpgrade[2]);
		float[] firstCostArray = new float[] {100f, 75f, 0f};
		localUpgradesList [0] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Attack, 0, 0.2f, "-Increase unit attack\n by \r dps\n", firstCostArray, 1.5f);
		localUpgradesList [0] [0].isUnitUpgrade = true;
		localUpgradesList [0] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Health, 1, 0.15f, "-Increase unit health\n by \r hp");
		localUpgradesList [0] [1].isUnitUpgrade = true;
		// Increase max units
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] secondCostArray = new float[] {75f, 50f, 25f};
		localUpgradesList [1] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 0, 1f, "-Increase max " + unitName + "s\nby \r \n", secondCostArray, 1.5f);
		localUpgradesList [1] [0].asAddition = true;
		localUpgradesList [1] [0].messageArray = new string[] {"InstantiateNewUnit", "StartTraining"};
		// Decrease training time and increase building health
		localUpgradesList.Add (new LocalUpgrade[2]);
		float[] thirdCostArray = new float[] {50f, 100f, 0f};
		localUpgradesList [2] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 0, 1f, "-Decrease training time by \r s\n", thirdCostArray, 1.25f);
		localUpgradesList [2] [0].miscUpgradeSprite = LocalUpgradesMenu.statSpritesDick [StatsType.TimeStats];
		localUpgradesList [2] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Health, 1, 0.2f, "-Increase building health\nby \r hp\n");
	}
}
