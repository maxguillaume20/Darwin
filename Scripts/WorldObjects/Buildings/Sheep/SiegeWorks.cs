using UnityEngine;
using System.Collections;
using RTS;

public class SiegeWorks : NonRelatedUnitTrainer
{
	protected override void MakeLocalUpgrades ()
	{
		// Increase unit attack
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] firstCostArray = new float[] {100f, 100f, 0f};
		localUpgradesList[0][0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Attack, 0, 0.1f, "-Increase " + unitName + " attack\n by \r dps\n", firstCostArray, 1.5f);
		localUpgradesList [0] [0].isUnitUpgrade = true;
		// Increase unit range
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] secondCostArray = new float[] {50f, 75f, 0f};
		localUpgradesList [1] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.RangedStats, 0, 0.15f, "-Increase " + unitName + " range\n by \r m", secondCostArray, 1.5f);
		localUpgradesList [1] [0].isUnitUpgrade = true;
		// Increase max units
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] thirdCostArray = new float[] {150f, 150f, 0f};
		localUpgradesList [2] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 0, 1f, "-Increase max " + unitName + "s by \r", thirdCostArray, 1.5f);
		localUpgradesList [2] [0].asAddition = true;
		localUpgradesList [2] [0].messageArray = new string[] {"InstantiateNewUnit"};
	}
}
