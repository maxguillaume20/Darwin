using UnityEngine;
using System.Collections;
using RTS;

public class Academy : SpeciesUnitTrainer 
{
	protected override void MakeLocalUpgrades ()
	{
		// Increase unit attack
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] firstCostArray = new float[] {0f, 0f, 75f};
		localUpgradesList[0][0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Attack, 0, 0.12f, "-Increase unit attack\n by \r dps\n", firstCostArray, 1.5f);
		localUpgradesList [0] [0].isUnitUpgrade = true;
		// Increase unit health
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] secondCostArray = new float[] {150f, 100f, 0f};
		localUpgradesList [1] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Health, 1, 0.2f, "-Increase unit health\n by \r hp", secondCostArray, 1.7f);
		localUpgradesList [1] [0].isUnitUpgrade = true;
		// Decrease trainingTime and increase unit psychological defense
		localUpgradesList.Add (new LocalUpgrade[2]);
		float[] thirdCostArray = new float[] {0f, 150f, 25f};
		localUpgradesList [2] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 1, -0.075f, "-Decrease unit training time\n by \r s\n", thirdCostArray, 1.5f);
		localUpgradesList [2] [0].miscUpgradeSprite = LocalUpgradesMenu.statSpritesDick [StatsType.TimeStats];
		localUpgradesList [2] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Defense, 4, 0.15f, "-Increase unit psychological\ndefense by \r%");
		localUpgradesList [2] [1].isUnitUpgrade = true;
		localUpgradesList [2] [1].asAddition = true;
		localUpgradesList [2] [1].descriptionAsPercentage = true;
	}
}
