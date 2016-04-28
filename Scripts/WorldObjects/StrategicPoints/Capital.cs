using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Capital : StrategicPoint 
{
	// [0] = constructionRate, [1] = repairRate, [2] = multipleConstructionPenaltyExponent
	public float[] capitalStatsArray;
	// [0] = maxCaravanPopCount, [1] = maxCaravanHealth, [2] = caravanSpeed
	public float[] caravanStatsArray;
	public List<Building> constructionList = new List<Building> ();
	private bool wasOccupied;

	protected override void Awake ()
	{
		base.Awake ();
		healthArray [0] = healthArray [1];
	}

	protected override void Start()
	{
		base.Start ();
		Occupy (player, Pop_Dynamics_Model.modelStatsDick [player.species][StatsType.Population]);
	}

	public override void Occupy (Player newPlayer, float newPopulation)
	{
		base.Occupy (newPlayer, newPopulation);
		originalSpecies = newPlayer.species;
		buttons.Add (PanelButtonType.BuildingMenuButton);
	}

	public float ConstructionOrRepairTimeStep (int statsIndex) 
	{
		return GetCurrentUnits() * capitalStatsArray[statsIndex] * Time.deltaTime / Mathf.Pow(constructionList.Count, capitalStatsArray[2]);
	}

	public override void Die ()
	{
		if (isAlive) 
		{
			GameManager.playersDick [originalSpecies].LoseGame ();
		}
		base.Die ();
	}

	public override void Revert ()
	{
		Destroy (gameObject);
	}

	protected override void MakeLocalUpgrades ()
	{
		// Increase repair rate and decrease multi construction penalty
		localUpgradesList.Add (new LocalUpgrade[2]);
		float[] firstCostArray = new float[] {150f, 150f, 0f};
		localUpgradesList[0][0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.CapitalStats, 1, 0.25f, "-Increase repair rate\nby \r %\n", firstCostArray, 1.5f);
		localUpgradesList [0] [0].descriptionAsPercentage = true;
		localUpgradesList[0][1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.CapitalStats, 2, -0.1f, "-Decrease multi-construction\ndelay by \r%");
		localUpgradesList [0] [1].descriptionAsPercentage = true;
		// Increase caravan capacity, health, and speed
		localUpgradesList.Add (new LocalUpgrade[3]);
		float[] secondCostArray = new float[] {75f, 75f, 0f};
		localUpgradesList [1] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.CaravanStats, 0, 2f, "-Increase caravan capacity\nby \r\n", secondCostArray, 1.5f);
		localUpgradesList [1] [0].asAddition = true;
		localUpgradesList [1] [1] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.CaravanStats, 1, 0.1f, "-Increase caravan health\nby \r hp\n");
		localUpgradesList [1] [2] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.CaravanStats, 2, 0.25f, "-Increase caravan speed\nby \r m/s");
		localUpgradesList [1] [2].messageArray = new string[] {"IncreaseCurrentCaravansStats"};
		// Increase max builders
		localUpgradesList.Add (new LocalUpgrade[1]);
		float[] thirdCostArray = new float[] {0f, 0f, 75f};
		localUpgradesList [2] [0] = LocalUpgradesMenu.MakeLocalUpgrade (StatsType.MobTrainerStats, 0, 2f, "-Increase max " + unitName + "s by \r", thirdCostArray, 1.5f);
		localUpgradesList [2] [0].asAddition = true;
		localUpgradesList [2] [0].messageArray = new string[] {"IncreaseIdealUnits"};
	}

	private void IncreaseCurrentCaravansStats () 
	{
		foreach (Caravan car in player.currWorldObjectsDick["Caravan"]) 
		{
			float currentHealthRatio = car.healthArray[0] / car.healthArray[1];
			car.healthArray[1] += localUpgradesList[1][1].change * car.GetLocalStat (StatsType.Health, 1);
			car.healthArray[0] = currentHealthRatio * car.healthArray[1];
			if (car.healthBar) 
			{
				car.healthBar.ResetBar ();
			}
			car.mobileStatsArray[0] += localUpgradesList[1][2].change * car.GetLocalStat (StatsType.MobileStats, 0);
			car.SetMovingNavAgentSpeed ();
		}
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.CapitalStats, capitalStatsArray);
		statsDick.Add (StatsType.CaravanStats, caravanStatsArray);
	}
}