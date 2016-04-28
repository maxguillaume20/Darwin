using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public class Building : WorldObject 
{
	// [0] = Gold, [1] = Wood, [2] = Unique;
	public float[] buildingCostArray;
	public float maxConstructionTime;
	public float multiBuildingExp;
	public bool constructing; 
	public bool repairing;
	protected NavMeshObstacle navMeshObs;
	protected PanelButtonType[] PanelButtonTypes;
	public Eras buildingEra;
	public List<LocalUpgrade[]> localUpgradesList = new List<LocalUpgrade[]> ();
	public bool isUpgrading;
	// [0] = timer, [1] = totalTime, [2] = upgradeIndex
	private float[] lUArray = new float[3];
	private static float lUBaseTime = 10f;
	private static float lURanktTimeIncrease = 1.25f;
	protected int remainingLocUpgrades = 2;
	public BuildingSlot buildingSlot;
//	protected delegate void UpgradeMethod();
//	protected Dictionary<Eras, UpgradeMethod> eraUpgradesDick = new Dictionary<Eras, UpgradeMethod>();

	protected override void Awake ()
	{
		base.Awake ();
		healthArray[0] = 1f;
		navMeshObs = GetComponent<NavMeshObstacle> ();
	}

	protected override void Start()
	{
		base.Start ();
		gameObject.tag = "Building";

		if (!GameManager.inBattleGround) 
		{
			MakeLocalUpgrades ();
			if (this as StrategicPoint == null) 
			{
				StartCoroutine (Construct ());
			}
		}
	}

	protected virtual void MakeLocalUpgrades () 
	{

	}

	public override void SelectTap (Player controller)
	{
		base.SelectTap (controller);
		if (IsOwnedBy(controller.species))
		{
			GameManager.Hud.OpenMainPanel();
			if (!constructing) 
			{
				for (int i = 0; i < buttons.Count; i++)
				{
					GameManager.Hud.OpenPanelButton(buttons[i]);
				}
//				if (this as SpeciesUnitTrainer  == null || this as StrategicPoint != null) GameManager.Hud.Infotext.statsText.gameObject.SetActive(false);
				if (healthArray[0] < healthArray[1]) GameManager.Hud.repairButton.button.interactable = true;
			}
		}
	}

	public override void Damage (System.Collections.Generic.List<AttackType> attackTypeList, float damage)
	{
		base.Damage (attackTypeList, damage);
		if (!constructing && !GameManager.Hud.repairButton.button.interactable && selected && player == GameManager.HumanPlayer && healthArray[0] < healthArray[1]) 
		{
			GameManager.Hud.repairButton.button.interactable = true;
		}
	}

	

	protected IEnumerator Construct() 
	{
		constructing = true;
		spriteParent.gameObject.SetActive (false);
		player.capital.constructionList.Add (this);
		float HPincrease = (healthArray[1] - healthArray[0]) / maxConstructionTime;
		if (HPincrease > 0f) 
		{
			ConstructionPS constructPS = ((GameObject)Instantiate (GameManager.GetGameObject ("ConstructionPS"), transform.position, Quaternion.identity)).GetComponent<ConstructionPS> ();
			constructPS.Initiate (this);
			float constructionTime = 0f;
			while (constructionTime < maxConstructionTime && constructing && isAlive) 
			{
				float constructTimeStep = player.capital.ConstructionOrRepairTimeStep (0);
				constructionTime += constructTimeStep;
				healthArray[0] += HPincrease * constructTimeStep;
				if (healthBar) healthBar.ChangeHP(healthArray[0]);
				yield return null;
			}
		}
		if (isAlive) 
		{
			FinishConstruction ();
		}
	}

	protected virtual void FinishConstruction() 
	{
		constructing = false;
		spriteParent.gameObject.SetActive (true);
		buttons.Add (PanelButtonType.RepairButton);
		buttons.Add (PanelButtonType.LocalUpgradesButton);
		player.capital.constructionList.Remove (this);
		statsText = "";
		if (healthArray[0] > healthArray[1]) 
		{
			healthArray[0] = healthArray[1];
		}
		if (!selected) healthBar.gameObject.SetActive(false);
		else if (GameManager.Hud.mainpanel.gameObject.activeSelf)
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				GameManager.Hud.OpenPanelButton(buttons[i]);
			}
//			GameManager.Hud.Infotext.statsText.gameObject.SetActive(false);
//			GameManager.Hud.Infotext.healthText.gameObject.SetActive(true);
		}
	}

	public void StartRepair() 
	{
		StartCoroutine (Repair ());
	}

	private IEnumerator Repair() 
	{
		if (GameManager.HumanPlayer.GetResource(ResourceType.Wood) > 0)
		{
			repairing = true;
			player.capital.constructionList.Add (this);
			ConstructionPS constructPS = ((GameObject)Instantiate (GameManager.GetGameObject ("ConstructionPS"), transform.position, Quaternion.identity)).GetComponent<ConstructionPS> ();
			constructPS.Initiate (this);
			while (healthArray[0] < healthArray[1] && healthArray[0] > 0 && repairing && isAlive) 
			{
				if (GameManager.HumanPlayer.GetResource(ResourceType.Wood) <= 0) 
				{
					GameManager.HumanPlayer.ChangeResource(ResourceType.Wood, 0 - GameManager.HumanPlayer.GetResource(ResourceType.Wood));
					repairing = false;
					break;
				}
//				else if (player.capital.GetCurrentUnits() < 1 && constructPS.activeSelf) constructPS.SetActive(false);
//				else if (player.capital.GetCurrentUnits() >= 1 && !constructPS.activeSelf) constructPS.SetActive(true);
				float repairAmount = player.capital.ConstructionOrRepairTimeStep (1);
				healthArray[0] += repairAmount;
				GameManager.HumanPlayer.ChangeResource(ResourceType.Wood, -repairAmount);
				if (healthBar) healthBar.ChangeHP(healthArray[0]);
				yield return null;
			}
			repairing = false;
			if (healthArray[0] > healthArray[1]) 
			{
				healthArray[0] = healthArray[1];
				healthBar.ChangeHP(healthArray[0]);
			}
			if (!selected) healthBar.gameObject.SetActive(false);
			player.capital.constructionList.Remove (this);
		}
		else yield return null;
	}

	public int GetRemainingLocUpgrades () 
	{
		return remainingLocUpgrades;
	}

	public void SetRemainingLocalUpgrades (int amount) 
	{
		remainingLocUpgrades = amount;
	}

	public void StartUpgradingBuilding (int locUpIndex) 
	{
		if (!isUpgrading) 
		{
			isUpgrading = true;
			lUArray[2] = locUpIndex;
			StartCoroutine (UpgradeBuilding());
		}
	}

	private IEnumerator UpgradeBuilding () 
	{
		lUArray[1] = lUBaseTime * localUpgradesList[(int)lUArray[2]][0].rank * lURanktTimeIncrease;
		for (lUArray[0] = 0f; lUArray[0] < lUArray[1] && isAlive && isUpgrading; lUArray[0] += Time.deltaTime) 
		{
			yield return null;
		}
		if (lUArray[0] >= lUArray[1]) 
		{
			remainingLocUpgrades --;
			LocalUpgradesMenu.FinishUpgrade (this, (int)lUArray[2]);
		}
		isUpgrading = false;
	}

	public float GetUpgradeProgress () 
	{
		return lUArray [0] / lUArray [1];
	}

	public int GetActiveUpgradeIndex () 
	{
		return (int)lUArray [2];
	}

//	public override string GetHealthText ()
//	{
//		if (constructing) 
//		{
//			return "Constructing";
//		}
//		return base.GetHealthText ();
//	}

	public override void Die ()
	{
		if (isAlive && this as StrategicPoint == null) 
		{
			if (player) player.buildings.RemoveBuilding (this);
			ResourceType[] resourceArray = BuildMenu.buildingCostDick [name].Keys.ToArray ();
			foreach (ResourceType resource in resourceArray) 
			{
				BuildMenu.buildingCostDick[name][resource] = BuildMenu.buildingCostDick[name][resource] / multiBuildingExp; 
			}
			BuildingMenuPanel.ChangeCostText (name);
		}
		base.Die ();
		if (buildingSlot) 
		{
			buildingSlot.SetOccupation (false);
		}
		if (!destroyingGameObject && this as StrategicPoint == null) 
		{
			StartDestroyGameObject ();
		}
	}

//	protected override void StartDestroyGameObject ()
//	{
//		navMeshObs.enabled = false;
//		base.StartDestroyGameObject ();
//	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.BuildingCost, buildingCostArray);
	}
}
