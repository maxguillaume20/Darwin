using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RTS;

public class LocalUpgradesMenu : MonoBehaviour 
{
	public static bool isActive;
	public static Building selectedBuilding { get; set; }
	public LocalUpgradesMenuButton selectedLUB;
	public GameObject cancelButton;
	private static LocalUpgradesMenuButton[] menuButtons;
	public static Text remainingText;
	public Text descriptionText;
	public Sprite[] statSpritesArray;
	public static Dictionary<StatsType, Sprite> statSpritesDick { get; set; }

	private void Awake () 
	{
		menuButtons = GetComponentsInChildren<LocalUpgradesMenuButton> ();
		for (int i = 0; i < menuButtons.Length; i ++) 
		{
			menuButtons[i].Initiate (i);
		}
		remainingText = GetComponentInChildren<Text> ();
		statSpritesDick = new Dictionary<StatsType, Sprite> ();
		StatsType[] statsArray = new StatsType[] {
						StatsType.Health,
						StatsType.Attack,
						StatsType.Defense,
						StatsType.MobileStats,
						StatsType.RangedStats,
						StatsType.CaravanStats,
						StatsType.CapitalStats,
						StatsType.TimeStats
				};
		for (int i = 0; i < statsArray.Length; i ++) 
		{
			statSpritesDick.Add (statsArray[i], statSpritesArray[i]);
		}
		gameObject.SetActive (false);
	}

	public void Enable (Building newBuilding) 
	{
		isActive = true;
		selectedBuilding = newBuilding;
		selectedLUB = null;
		SetRemainingCount ();
		descriptionText.text = "";
		if (!selectedBuilding.isUpgrading) 
		{
			cancelButton.SetActive (false);
			foreach (LocalUpgradesMenuButton lUButton in menuButtons) 
			{
				lUButton.gameObject.SetActive (true);
			}
		}
		else 
		{
			remainingText.text = "";
			cancelButton.SetActive (true);
			foreach (LocalUpgradesMenuButton lUButton in menuButtons) 
			{
				lUButton.gameObject.SetActive (false);
			}
		}
		gameObject.SetActive (true);
	}

	private void OnEnable () 
	{
		GameManager.Hud.Infotext.SetPopInfoActive (false);
		GameManager.Hud.Infotext.healthText.gameObject.SetActive (false);
	}

	private void Update () 
	{
		if (selectedBuilding.isUpgrading) 
		{
			if (!cancelButton.activeSelf) 
			{
				cancelButton.SetActive (true);
			}
			if (!TrainingProgressBar.isActive) 
			{
				TrainingProgressBar.OpenBar (PanelButtonType.LocalUpgradesButton);
			}
			TrainingProgressBar.ChangeProgress (selectedBuilding.GetUpgradeProgress ());
		}
		else 
		{
			if (cancelButton.activeSelf) 
			{
				cancelButton.SetActive (false);
			}
			if (TrainingProgressBar.isActive) 
			{
				TrainingProgressBar.CloseBar ();
			}
		}
	}

	public void InitiateUpgrade (int locUpIndex) 
	{
		remainingText.text = "";
		descriptionText.text = "";
		selectedLUB = null;
		foreach (LocalUpgradesMenuButton lUButton in menuButtons) 
		{
			lUButton.gameObject.SetActive (false);
		}
		LocalUpgradesMenu.selectedBuilding.StartUpgradingBuilding (locUpIndex);
	}

	public static void FinishUpgrade (Building finishedBuilding, int locUpIndex) 
	{
		LocalUpgrade firstLU = finishedBuilding.localUpgradesList[locUpIndex][0];
		for (int i = 0; i < firstLU.costArray.Length; i ++) 
		{
			finishedBuilding.localUpgradesList[locUpIndex][0].costArray[i] *= firstLU.costIncrease;
		}
		finishedBuilding.localUpgradesList[locUpIndex][0].rank ++;
		if (isActive && finishedBuilding == selectedBuilding) 
		{
			SetRemainingCount ();
			TrainingProgressBar.CloseBar ();
			foreach (LocalUpgradesMenuButton lUButton in menuButtons) 
			{
				lUButton.gameObject.SetActive (true);
			}
		}
		for (int i = 0; i < finishedBuilding.localUpgradesList[locUpIndex].Length; i ++) 
		{
			LocalUpgrade lU = finishedBuilding.localUpgradesList[locUpIndex][i];
			float upgradeAmount = GetUpgradeAmount (finishedBuilding, lU);
			if (!lU.isUnitUpgrade) 
			{
				UpgradeStat (finishedBuilding as WorldObject, lU, upgradeAmount);
			}
			else 
			{
				MobTrainer mT = finishedBuilding as MobTrainer;
				for (int j = 0; j < mT.mobTrainerStatsArray[0]; j ++) 
				{
					UpgradeStat (mT.GetMob(j) as WorldObject, lU, upgradeAmount);
				}
				mT.locUpUnitStatsDick[lU.statsType][lU.statsIndex] += upgradeAmount;
			}
			if (lU.messageArray != null) 
			{
				foreach (string message in lU.messageArray) 
				{
					finishedBuilding.SendMessage (message, SendMessageOptions.RequireReceiver);
				}
			}
		}
	}
	
	public static float GetUpgradeAmount (Building poopBuilding, LocalUpgrade lU) 
	{
		float upgradeAmount = 0f;
		if (lU.asAddition) 
		{
			upgradeAmount = lU.change;
		}
		else if (!lU.isUnitUpgrade) 
		{
			upgradeAmount = lU.change * poopBuilding.GetLocalStat (lU.statsType, lU.statsIndex);
		}
		else 
		{
			upgradeAmount = lU.change * (poopBuilding as MobTrainer).GetMob(0).GetLocalStat (lU.statsType, lU.statsIndex);
		}
		return upgradeAmount;
	}
	
	private static void UpgradeStat (WorldObject worldObject, LocalUpgrade lU, float upgradeAmount) 
	{
		worldObject.statsDick[lU.statsType][lU.statsIndex] += upgradeAmount;
		if (lU.statsType == StatsType.Health) 
		{
			worldObject.statsDick[lU.statsType][0] = worldObject.healthArray[0] / (worldObject.healthArray[1] - upgradeAmount) * worldObject.healthArray[1];
			if (worldObject.healthBar) worldObject.healthBar.ResetBar ();
		}
	}

	public static void SetRemainingCount () 
	{
		remainingText.text = "Remaining Local Upgrades: " + selectedBuilding.GetRemainingLocUpgrades ();
	}

	public void SetDescriptionText () 
	{
		string description = "";
		for (int i = 0; i < selectedBuilding.localUpgradesList[selectedLUB.locUpIndex].Length; i ++) 
		{
			LocalUpgrade lU = selectedBuilding.localUpgradesList[selectedLUB.locUpIndex][i];
			float upgradeAmount = GetUpgradeAmount (selectedBuilding, lU);
			string[] brokenDes = lU.description.Split('\r');
			if (!lU.descriptionAsPercentage) 
			{
				description += brokenDes[0] + Mathf.Abs(upgradeAmount).ToString("0.##") + brokenDes[1];
			}
			else 
			{
				description += brokenDes[0] + Mathf.Abs(lU.change * 100f).ToString("0.##") + brokenDes[1];
			}
		}
		descriptionText.text = description;
	}

	public void CancelUpgrade () 
	{
		selectedBuilding.isUpgrading = false;
		LocalUpgrade activeUpgrade = selectedBuilding.localUpgradesList [selectedBuilding.GetActiveUpgradeIndex ()][0];
		selectedBuilding.GetPlayer ().CancelPurchase (activeUpgrade.costArray);
		TrainingProgressBar.CloseBar ();
		cancelButton.SetActive (false);
		SetRemainingCount ();
		foreach (LocalUpgradesMenuButton mB in menuButtons) 
		{
			mB.gameObject.SetActive (true);
		}
	}

	public static void NewEra () 
	{
		if (isActive) 
		{
			SetRemainingCount ();
			foreach (LocalUpgradesMenuButton mB in menuButtons) mB.gameObject.SetActive (false);
			foreach (LocalUpgradesMenuButton mB in menuButtons) mB.gameObject.SetActive (true);
		}
	}

	private void OnDisable () 
	{
		if (isActive) 
		{
			isActive = false;
			TrainingProgressBar.CloseBar ();
			GameManager.Hud.Infotext.SetPopInfoActive (true);
			GameManager.Hud.Infotext.healthText.gameObject.SetActive (true);
		}
	}

	public static LocalUpgrade MakeLocalUpgrade (StatsType newStatsType, int newStatsIndex, float newChange, string newDescription) 
	{
		LocalUpgrade newLocalUpgrade = new LocalUpgrade ();
		newLocalUpgrade.rank = 1;
		newLocalUpgrade.statsType = newStatsType;
		newLocalUpgrade.statsIndex = newStatsIndex;
		newLocalUpgrade.change = newChange;
		newLocalUpgrade.description = newDescription;
		return newLocalUpgrade;
	}

	public static LocalUpgrade MakeLocalUpgrade (StatsType newStatsType, int newStatsIndex, float newChange, string newDescription, float[] newCostArray, float newCostIncrease) 
	{
		LocalUpgrade newLocalUpGrade = MakeLocalUpgrade (newStatsType, newStatsIndex, newChange, newDescription);
		newLocalUpGrade.costArray = newCostArray;
		newLocalUpGrade.costIncrease = newCostIncrease;
		return newLocalUpGrade;
	}
}

public struct LocalUpgrade 
{
	public int rank;
	public StatsType statsType;
	public int statsIndex;
	public float change;
	public string description;
	public float[] costArray;
	public float costIncrease;
	public bool isUnitUpgrade;
	public bool asAddition;
	public bool descriptionAsPercentage;
	public string[] messageArray;
	public Sprite miscUpgradeSprite;
}


