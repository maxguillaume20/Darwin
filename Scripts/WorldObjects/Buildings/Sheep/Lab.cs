using UnityEngine;
using System.Collections;
using RTS;

public class Lab : Building
{
	// [0] = successfulExpAmount, [1] = failedExpAmount, [2] = expCost, [3] = expCoolDown
	public float[] uniqueStatsArray;
	public bool ableToExperiment;
	private float cdTimer;

	public void StartExperimentCoolDown () 
	{
		if (ableToExperiment) 
		{
			StartCoroutine (ExperimentCoolDown ());
		}
	}

	private IEnumerator ExperimentCoolDown () 
	{
		ableToExperiment = false;
		for (cdTimer = 0f; cdTimer < uniqueStatsArray[3]; cdTimer += Time.deltaTime) 
		{
			yield return null;
		}
		ableToExperiment = true;
	}

	public float GetCoolDownProgress () 
	{
		return cdTimer / uniqueStatsArray [3];
	}

	protected override void FinishConstruction ()
	{
		buttons.Add (PanelButtonType.LabButton);
		ableToExperiment = true;
		base.FinishConstruction ();
	}

	protected override void MakeLocalUpgrades ()
	{
		// Increase the knowledge gain from experiments
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] firstCostArray = new float[] {200f, 150f, 0f};
		localUpgradesList [0] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.UniqueStats, 0, 0.2f, "-Increase the knowledge gain\nfrom your experiments by \r%\n", firstCostArray, 1.75f);
		localUpgradesList [0] [0].descriptionAsPercentage = true;
		localUpgradesList [0] [0].miscUpgradeSprite = HUD.speciesResourceSpriteDick [GetSpecies ()] [ResourceType.Unique];
		// Decrease expCost and cooldown
		localUpgradesList.Add (new LocalUpgrade[2]);
		float[] secondCostArray = new float[] {0f, 0f, 50f};
		localUpgradesList [1] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.UniqueStats, 2, -0.25f, "-Decrease experiment cost\nby \r %\n", secondCostArray, 1.4f);
		localUpgradesList [1] [0].descriptionAsPercentage = true;
		localUpgradesList [1] [0].miscUpgradeSprite = HUD.speciesResourceSpriteDick [GetSpecies ()] [ResourceType.Gold];
		localUpgradesList [1] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.UniqueStats, 3, -0.15f, "-Decrease experiment cool\ndown by \r s\n");
		localUpgradesList [1] [1].miscUpgradeSprite = LocalUpgradesMenu.statSpritesDick[StatsType.TimeStats];
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.UniqueStats, uniqueStatsArray);
	}
}
