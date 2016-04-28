using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public class StrategicPoint : SpeciesMobTrainer 
{ 
	// [0] = buildingSlotCount
	public float[] stratStatsArray;
	public float[] stratPointStatsArray;
	public bool occupied;
	public float population;
	public BuildingArea buildingArea;
	protected Flag flag;
	public bool trainingCaravan;
	private List<int> caravanPopList = new List<int> ();
	private static float carMaxTrainingTime = 10f;
	protected float currCarTrainingTime;
	protected float popSurvivalFactors = 25f;
	public List<SpeciesUnitTrainer> connectedSpeciesTrainers = new List<SpeciesUnitTrainer> ();

	protected override void Awake ()
	{
		base.Awake ();
		gameObject.tag = "StrategicPoint";
		flag = GetComponentInChildren<Flag> ();
		if (flag) 
		{
			flag.transform.eulerAngles = new Vector3 (HUD.xCameraRotation, 0f, 0f);
			flag.gameObject.SetActive (false);
		}
		buildingArea = GetComponentInChildren <BuildingArea> ();
		if (buildingArea) 
		{
			buildingArea.Initiate ();
		}
	}

	public override void SelectTap(Player controller)
	{
		base.SelectTap(controller);
		if (!occupied) 
		{
			healthBar.gameObject.SetActive(false);
			GameManager.Hud.OpenMainPanel ();
		}
		else 
		{
			foreach (SpeciesUnitTrainer sT in connectedSpeciesTrainers) 
			{
				sT.stratLine.gameObject.SetActive (true);
			}
		}
	}

	public override void Deselect ()
	{
		base.Deselect ();
		foreach (SpeciesUnitTrainer sT in connectedSpeciesTrainers) 
		{
			sT.stratLine.gameObject.SetActive (false);
		}
	}

	public override string GetHealthText ()
	{
		if (!occupied) 
		{
			return "Vacant";
		}
		return base.GetHealthText ();
	}

	public virtual void Occupy (Player newPlayer, float newPopulation)
	{
		isAlive = true;
		player = newPlayer;
		SetLayer ();
		player.stratPoints.AddStratPt (this);
//		player.AddToWOsDick (this as WorldObject);
		population = newPopulation;
		occupied = true;
		if (flag) 
		{
			flag.GetComponent<SpriteRenderer> ().color = newPlayer.color;
			flag.gameObject.SetActive (true);
		}
		if (selected && GameManager.Hud.mainpanel.gameObject.activeSelf) 
		{
			GameManager.Hud.Infotext.popSprite.gameObject.SetActive(true);
			GameManager.Hud.Infotext.popText.gameObject.SetActive(true);
		}
		StartCoroutine(Construct());
	}

	protected override void FinishConstruction ()
	{
		buttons.Add (PanelButtonType.CaravanButton);
		buttons.Add (PanelButtonType.UnitsButton); 
		base.FinishConstruction ();
		idealMobCount = (int) mobTrainerStatsArray [0];
		StartTraining ();
		if (buildingArea) 
		{
			buildingArea.SetSpecies (player.species);
			if (GameManager.Hud.PanelDict[PanelButtonType.BuildMenu].activeSelf) 
			{
				buildingArea.gameObject.SetActive(true);
			}
			List<SpeciesUnitTrainer> speciesTrainerList = buildingArea.GetBuildingsList<SpeciesUnitTrainer> ();
			foreach (SpeciesUnitTrainer sT in speciesTrainerList) 
			{
				if (sT.GetSpecies () == GetSpecies()) 
				{
					sT.SetMainStratPoint (this);
				}
			}
		} 
	}

	public void EnableBuildingArea () 
	{
		// the species of the buildingArea isn't set until FinishConstruction is called,
		// where as the strat species is set when Occupy is called
		if (buildingArea && buildingArea.GetSpecies() == GetSpecies()) 
		{
			buildingArea.gameObject.SetActive (true);
		}
	}

	// Some local upgrades send this message
	private void SetBuildingAreaSize() 
	{

	}

	public override void Damage (List<AttackType> damageTypeList, float damage) 
	{
		if (GetSpecies () == Species.NonPlayer) 
		{
			Debug.LogWarning ("Unoccupied StratPoint is being attacked: " + name); 
		}
		else 
		{
			foreach (AttackType damageType in damageTypeList) 
			{
				healthArray[0] -= damage * ((1f - defenseArray[GameManager.attackTypeDickToArray[damageType]]) / (float) damageTypeList.Count);
			}
			if ((int)healthArray[0] <= 0) 
			{
				healthArray[0] = 0f;
			}
			healthBar.ChangeHP (healthArray[0]);
			if ((int)healthArray[0] <= 0) 
			{
				Pop_Dynamics_Model.modelStatsDick [player.species][StatsType.Population] -= (damage / popSurvivalFactors);
				ChangeLocalPopulation (-damage / popSurvivalFactors);
			}
			if ((int)healthArray[0] <= 0 && (int)population <= 0) 
			{
				Die();
			}
		}
	}

	public void ChangeLocalPopulation (float amount)
	{
		int oldPop = (int)population;
		population += amount;
		if ((int)population <= 0) population = 0f;
		int popChange = (int)population - oldPop;
		if (popChange != 0) 
		{
			if (popChange < 0 && GetIdleUnits() <= 0) 
			{
				DestroyMobs ();
			}
			else if (GetIdleUnits() > 0) 
			{
				foreach (SpeciesUnitTrainer unitTrainer in connectedSpeciesTrainers) 
				{
					unitTrainer.StartTraining();
				}
				StartTraining ();
			}
			if (player.Equals(GameManager.HumanPlayer))
			{
				int resAmount = (int) Pop_Dynamics_Model.modelStatsDick [player.species][StatsType.Population];
				GameManager.Hud.populationText.text = resAmount.ToString();		
			}
		}
	}

	protected override bool AbleToTrain ()
	{
		return currentMobCount < idealMobCount && mobPopCount <= GetIdleUnits ();
	}

//	public int GetCurrentUnits ()
//	{
//		return currentMobCount;
//	}

	public int GetIdleUnits() 
	{
		return (int)population - currentMobCount * mobPopCount;
	}

	protected override void CreateMob() 
	{
		currentMobCount ++;
	}

	protected override void DestroyMob ()
	{
//		currentMobCount = idealMobCount;
		currentMobCount --;
	}

	private void DestroyMobs () 
	{
		while (currentMobCount * mobPopCount > (int) population) 
		{
			DestroyMob ();
		}
	}

	public void SpeciesUnitTrainerUnitMade (int mobPopCount) 
	{
		population -= mobPopCount;
		DestroyMobs ();
	}

	public int GetMaxCarPopCount() 
	{
		if ((int) population < player.capital.caravanStatsArray[0]) return (int)population;
		return (int)player.capital.caravanStatsArray[0];
	}

	public void StartTrainingNewCaravan(int popCount) 
	{
		caravanPopList.Add (popCount);
		if (!trainingCaravan) StartCoroutine(TrainCaravan());
	}

	public int CancelTrainingCaravan() 
	{
		int lastPopCount = caravanPopList[caravanPopList.Count - 1];
		caravanPopList.RemoveAt (caravanPopList.Count - 1);
		return lastPopCount;
	}

	private IEnumerator TrainCaravan() 
	{
		trainingCaravan = true;
		while (caravanPopList.Count > 0) 
		{
			for (currCarTrainingTime = 0.0f; currCarTrainingTime < carMaxTrainingTime && caravanPopList.Count > 0; currCarTrainingTime += Time.deltaTime) yield return null;
			if (currCarTrainingTime >= carMaxTrainingTime) 
			{
				Unit newCar = player.AddUnit ("Caravan", spawnPoint) as Unit;
				newCar.currPopCount = caravanPopList[0];
				newCar.healthArray[1] = player.capital.caravanStatsArray[1];
				newCar.healthArray[0] = player.capital.caravanStatsArray[1];
				newCar.mobileStatsArray[0] = player.capital.caravanStatsArray[2];
				caravanPopList.RemoveAt (0);
			}
		}
		trainingCaravan = false;
	}

	public float GetCarTrainingProgress()
	{
		return currCarTrainingTime / carMaxTrainingTime;
	}

	public string CaravanTrainingCount () 
	{
		if (caravanPopList.Count > 1) 
		{
			return "(" + (caravanPopList.Count - 1) + ")";
		}
		else return "";
	}

	public override void Die ()
	{
		if (isAlive) 
		{
//			isAlive = false;
			if (player) 
			{
				player.stratPoints.RemoveStratPt (this);
			}

//			player.RemoveFromWOsDick (this as WorldObject);
			occupied = false;
			foreach (SpeciesUnitTrainer unitTrainer in connectedSpeciesTrainers) 
			{
				unitTrainer.mainStratPoint = null;
				Destroy(unitTrainer.stratLine.gameObject);
			}
			connectedSpeciesTrainers.Clear ();
			foreach (StatsType statsType in statsDick.Keys) 
			{
				for (int i = 0; i < statsDick[statsType].Length; i++) 
				{
					statsDick[statsType][i] = GetLocalStat (statsType, i);
				}
			}
			buildingArea.SetSpecies (Species.NonPlayer);
//			healthBar.ResetBar ();
//			healthBar.gameObject.SetActive (false);
			if (flag) 
			{
				flag.GetComponent<SpriteRenderer> ().color = Color.white;
				flag.gameObject.SetActive (false);
			}
			buttons.Clear();
//			if (selected) 
//			{
//				Deselect();
//				GameManager.playersDick[player.species].userInput.SelectedObjects.Remove(this as WorldObject);
//			}
			StrategicPointTransform.StartRevertCoroutine (this);
		}
		base.Die ();
		if (player) 
		{
			player = null;
			SetLayer ();
		}
	}

	public virtual void Revert () 
	{
		gameObject.SetActive (true);
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.StratStats, stratPointStatsArray);
	}
}
