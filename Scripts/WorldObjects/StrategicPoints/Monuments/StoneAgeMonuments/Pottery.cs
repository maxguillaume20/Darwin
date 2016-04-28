using UnityEngine;
using System.Collections.Generic;
using RTS;

public class Pottery : ResourceMonument 
{
	protected override void MakeLocalUpgrades ()
	{
		// Increase gathering amount
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] firstCostArray = new float[] { 0f, 250f, 50};
		localUpgradesList[0][0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.ResourceStats, 0, 0.15f, "-Increase gathering amount\n per " + unitName + " by \r%", firstCostArray, 1.5f);
		localUpgradesList [0] [0].descriptionAsPercentage = true;
		localUpgradesList [0] [0].miscUpgradeSprite = HUD.speciesResourceSpriteDick [GetSpecies ()] [ResourceType.Gold];
		// Increase max units and decrease training time
		localUpgradesList.Add (new LocalUpgrade[2]);
		float[] secondCostArray = new float[] {0f, 0f, 100f};
		localUpgradesList [1] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 0, 1f, "-Increase max " + unitName + "s by \r\n", secondCostArray, 1.5f);
		localUpgradesList [1] [0].asAddition = true;
		localUpgradesList [1] [0].messageArray = new string[] {"IncreaseIdealUnits"};
		localUpgradesList [1] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 1, -0.15f, "-Decrease training time\n by \r%");
		localUpgradesList [1] [1].descriptionAsPercentage = true;
		// Increase building health, crush defense, and buildingArea
		localUpgradesList.Add (new LocalUpgrade[2]);
		float[] thirdCostArray = new float[] {0f, 300f, 0f};
		localUpgradesList [2] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Health, 1, 0.15f, "-Increase building health \nby \r hp\n", thirdCostArray, 1.3f);
		localUpgradesList [2] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.Defense, 2, 0.12f, "-Increase building crush\ndefense by \r%\n");
		localUpgradesList [2] [1].asAddition = true;
		localUpgradesList [2] [1].descriptionAsPercentage = true;
//		localUpgradesList [2] [2] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.StratStats, 0, 0.10f, "-Increase the building area \nradius by \r%");
//		localUpgradesList [2] [2].descriptionAsPercentage = true;
//		localUpgradesList[2][2].miscUpgradeSprite = LocalUpgradesMenu.statSpritesDick[StatsType.RangedStats];
//		localUpgradesList [2] [2].messageArray = new string[] {"SetBuildingAreaSize"}; 
	}
}
