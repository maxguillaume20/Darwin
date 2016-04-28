using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class EvolutionPanelButton : MonoBehaviour 
{
	public int rank = 1;
	public float position;
//	public bool isInteractable;
	protected int mainArrayIndex;
	public Eras Era; 
	public Button button;
	protected Species species;
	protected Sprite sprite;
	protected string[] namesArray;
	protected int[] chronologicalOrderArray;
	protected UnityEngine.Events.UnityAction[] methodArray;
	protected string[] messageArray;
	protected WorldObjectType[] wotArray;
	//float[0] = overall coefficient, [1] = overall exponent, [2] = sweet spot coefficient, [3] = sweet spot mid point, [4] = sweet spot distribution
	protected List<Dictionary<StatsType, float[]>> rewardVariablesDickList = new List<Dictionary<StatsType, float[]>> ();
	// ^
	protected List<float[]> costVariablesList = new List<float[]> ();
	public float cost;
	protected Image[] images;
	public Image resSprite;
	public Text resText;
	private Text rankText;

	protected virtual void Awake() 
	{
		images = GetComponentsInChildren<Image> ();
		rankText = GetComponentInChildren<Text> ();
		button.onClick.AddListener (delegate {Upgrade();});
		SpecializationButton sb = this as SpecializationButton; 
		species = GameManager.HumanPlayer.species;
		if (sb == null || Era == Eras.StoneAge) 
		{
			switch (GameManager.HumanPlayer.species) 
			{
			case Species.Bunnies:
				mainArrayIndex = 0;
				break;
			case Species.Deer:
				mainArrayIndex = 1;
				break;
			case Species.Sheep:
				mainArrayIndex = 2;
				break;
			}
		}
		resSprite.sprite = HUD.speciesResourceSpriteDick [species] [ResourceType.Unique];
		cost = 100f * GameManager.eraOrderDick [Era];
		button.interactable = false;
	}

	protected virtual void Start() 
	{
		SpecializationButton sb = this as SpecializationButton; 
		resText.text = (int)(cost) + "";
		if (sb == null || Era == Eras.StoneAge) 
		{
			if (Era != Eras.Industrial) 
			{
				images[1].sprite = GameManager.evoPanel.GetEvoPanButtSprite(namesArray[mainArrayIndex]);
			}
		}
	}

	protected void OnEnable() 
	{
		resText.gameObject.SetActive (true);
	}

	protected void OnDisable() 
	{
		resText.gameObject.SetActive (false);
	}

	public void SetPosition() 
	{
		// if statement is only temporary
		if (Era == Eras.StoneAge) 
		{
			RectTransform rectTransform = GetComponent<RectTransform> ();
			float xMinPos = (float)chronologicalOrderArray[mainArrayIndex] / 10f * 1f / 6f * GameManager.eraOrderDick[Era] - 0.03f;
			float xMaxPos = (float)chronologicalOrderArray[mainArrayIndex] / 10f * 1f / 6f * GameManager.eraOrderDick[Era] + 0.03f;
			if (xMinPos <= 0.03f) 
			{
				xMinPos = 0.03f;
				xMaxPos = 0.09f;
			}
			rectTransform.anchorMin = new Vector2(xMinPos, rectTransform.anchorMin.y);
			rectTransform.anchorMax = new Vector2(xMaxPos, rectTransform.anchorMax.y);
			rankText.rectTransform.sizeDelta = new Vector2 (Screen.width * 0.025f, Screen.height * 0.025f);
			rankText.rectTransform.localPosition = new Vector3 (rankText.rectTransform.localPosition.x, rankText.rectTransform.localPosition.y + ((rectTransform.anchorMax.y - rectTransform.anchorMin.y) * Screen.height + rankText.rectTransform.sizeDelta.y) / 2f, rankText.rectTransform.localPosition.z);
			RectTransform resTextTransform = resText.GetComponent<RectTransform> ();
			resTextTransform.anchorMin = new Vector2 (xMinPos + 0.01f, resTextTransform.anchorMin.y);
			resTextTransform.anchorMax = new Vector2 (xMaxPos - 0.01f, resTextTransform.anchorMax.y);
			RectTransform resSpriteTransform = resSprite.GetComponent<RectTransform> ();
			resSpriteTransform.anchorMin = new Vector2 (xMinPos - 0.02f, resSpriteTransform.anchorMin.y);
			resSpriteTransform.anchorMax = new Vector2 (xMinPos + 0.01f, resSpriteTransform.anchorMax.y);
			position = (float)(rectTransform.anchorMin.x + rectTransform.anchorMax.x) / 2f;
		} 
		else position = (float) (GetComponent<RectTransform>().anchorMin.x + GetComponent<RectTransform>().anchorMax.x) / 2f;
	}

	protected void Upgrade() 
	{
		if (GameManager.evoPanel.selectedEPB != this) 
		{
			Select(RankIncreaseText(messageArray[mainArrayIndex]));
		}
		else if (EnoughResources())
		{
			methodArray [mainArrayIndex] ();
			PurchaseUpgrade();
		}
	}

	protected void Select(string message) 
	{
		GameManager.evoPanel.selectedEPB = this;
		GameManager.evoPanel.nameText.text = namesArray [mainArrayIndex] + " Rank: " + rank;
		GameManager.evoPanel.messageText.text = message;
		GameManager.evoPanel.purchasedText.text = "";
		GameManager.evoPanel.upgradesGraph.gameObject.SetActive (false);
		GameManager.evoPanel.upgradesGraph.gameObject.SetActive (true);
	}

	protected string RankIncreaseText(string message)
	{
		string[] brokenMessage = message.Split ('\n');
		string newMessage = "";
		for(int i = 0; i < brokenMessage.Length; i++) 
		{
			if ((brokenMessage[i].Contains("-Unlocks") || brokenMessage[i].Contains("-Disables")) && rank > 1) 
			{
				brokenMessage[i] = "";
			}
//			if (brokenMessage[i].Contains("-Increase Cost by")) 
//			{
//				brokenMessage[i] += GetPercentageIncrease(rank, costVariablesList[mainArrayIndex]) + " of the Initial Cost";
//			}
//			else if (brokenMessage[i].Contains("-Unlocks") && rank > 1) 
//			{
//				brokenMessage[i] = "";
//			}
//			else if (brokenMessage[i].Contains("StatsType."))
//			{
//				string statsTypeString = brokenMessage[i].Substring(brokenMessage[i].LastIndexOf("StatsType."));
//				brokenMessage[i] = brokenMessage[i].Substring(0,brokenMessage[i].LastIndexOf("StatsType."));
//				StatsType statsType = (StatsType)System.Enum.Parse(typeof(StatsType), statsTypeString.Substring(10));
//				brokenMessage[i] += GetPercentageIncrease(rank, rewardVariablesDickList[mainArrayIndex][statsType]) + " of the Initial Value";
//			}
			if (i < brokenMessage.Length - 1) 
			{
				brokenMessage[i] += "\n";
			}
			newMessage += brokenMessage[i];
		} 
		return newMessage;
	}

	protected void PurchaseUpgrade() 
	{
		GameManager.HumanPlayer.ChangeResource (ResourceType.Unique, -cost);
		GameManager.evoPanel.selectedEPB = null;
		cost += GetOriginalCost () * RankIncrease (rank, costVariablesList[mainArrayIndex][0], costVariablesList [mainArrayIndex][1], costVariablesList[mainArrayIndex][2], costVariablesList[mainArrayIndex][3], costVariablesList[mainArrayIndex][4]);
		resText.text = (int) cost + "";
		GameManager.evoPanel.nameText.text = "";
		GameManager.evoPanel.messageText.text = "";
		GameManager.evoPanel.upgradesGraph.gameObject.SetActive (false);
		GameManager.evoPanel.purchasedText.text = GameManager.evoPanel.GetIt [GameManager.HumanPlayer.species] + namesArray[mainArrayIndex] + " Rank: " + rank;
		rankText.text = rank.ToString ();
		rank ++;
	}

	protected void ChangeModelStats (Species changingSpecies, StatsType statsType) 
	{
		float amount = Pop_Dynamics_Model.GetOriginalStat(changingSpecies, statsType) * RankIncrease (rank, rewardVariablesDickList[mainArrayIndex][statsType][0], rewardVariablesDickList [mainArrayIndex][statsType][1], rewardVariablesDickList[mainArrayIndex][statsType][2], rewardVariablesDickList[mainArrayIndex][statsType][3], rewardVariablesDickList[mainArrayIndex][statsType][4]); 
		Pop_Dynamics_Model.ChangeModelStats (changingSpecies, statsType, amount); 
	}

	protected void ChangeEatRates(Species predator, Species prey) 
	{
		float amount = Pop_Dynamics_Model.GetOriginalEatRate(predator, prey) * RankIncrease (rank, rewardVariablesDickList[mainArrayIndex][StatsType.EatRates][0], rewardVariablesDickList [mainArrayIndex][StatsType.EatRates][1], rewardVariablesDickList[mainArrayIndex][StatsType.EatRates][2], rewardVariablesDickList[mainArrayIndex][StatsType.EatRates][3], rewardVariablesDickList[mainArrayIndex][StatsType.EatRates][4]);
		Pop_Dynamics_Model.ChangeEatRates (predator, prey, amount);
	}

	protected void UpgradeAttackType(AttackType attackType, int damage0speed1) 
	{
		List<string> woNames = GameManager.GetSpeciesAttackTypeList (species, attackType);
		foreach (string woName in woNames) 
		{
			UpgradeWorldObject(woName, StatsType.Attack, damage0speed1);
		}
	}

	protected void UpgradeWorldObjectType(WorldObjectType wot, StatsType upgradeStat, int statarrayIndex) 
	{
		List<string> woNames = GameManager.GetSpeciesWOTList (species, wot);
		foreach (string woName in woNames) 
		{
			UpgradeWorldObject(woName, upgradeStat, statarrayIndex);
		}
	}

	protected void UpgradeWorldObject (string woName, StatsType upgradeStat, int statArrayIndex) 
	{
		float upgradeAmount = 0f;
		if (upgradeStat != StatsType.Defense) 
		{
			upgradeAmount = GameManager.baseStatsDick[woName][upgradeStat][statArrayIndex] * RankIncrease (rank, rewardVariablesDickList[mainArrayIndex][upgradeStat][0], rewardVariablesDickList [mainArrayIndex][upgradeStat][1], rewardVariablesDickList[mainArrayIndex][upgradeStat][2], rewardVariablesDickList[mainArrayIndex][upgradeStat][3], rewardVariablesDickList[mainArrayIndex][upgradeStat][4]);
		}
		else 
		{
			upgradeAmount = RankIncrease (rank, rewardVariablesDickList[mainArrayIndex][upgradeStat][0], rewardVariablesDickList [mainArrayIndex][upgradeStat][1], rewardVariablesDickList[mainArrayIndex][upgradeStat][2], rewardVariablesDickList[mainArrayIndex][upgradeStat][3], rewardVariablesDickList[mainArrayIndex][upgradeStat][4]);
		}
		GameManager.playersDick [species].upgradedStatsDick [woName] [upgradeStat] [statArrayIndex] += upgradeAmount;
		List<WorldObject> currWOList = GameManager.playersDick[species].currWorldObjectsDick[woName];
		for(int i = 0; i < currWOList.Count; i++) 
		{
			currWOList[i].statsDick[upgradeStat][statArrayIndex] += upgradeAmount;
			if (upgradeStat == StatsType.Health) 
			{
				currWOList[i].statsDick[upgradeStat][0] = currWOList[i].healthArray[0] / (currWOList[i].healthArray[1] - upgradeAmount) * currWOList[i].healthArray[1];
				if (currWOList[i].healthBar) currWOList[i].healthBar.ResetBar ();
			}
		}
	}

	protected string GetPercentageIncrease(int rank, float[] floatArray) 
	{
		return (RankIncrease (rank, floatArray [0], floatArray [1], floatArray [2], floatArray [3], floatArray[4]) * 100).ToString("#.0") + "%";
	}

	public static float RankIncrease(int rank, float overallCoefficient, float overallExp, float sweetSpotCoefficient, float sweetSpotMidPoint, float sweetSpotDistribution) 
	{
		return overallCoefficient * (Mathf.Pow (rank, overallExp) + sweetSpotCoefficient * (Mathf.Exp(-((rank - sweetSpotMidPoint) * (rank - sweetSpotMidPoint)) / (2 * sweetSpotDistribution * sweetSpotDistribution))/(sweetSpotDistribution * Mathf.Sqrt(2 * Mathf.PI)))); 
	}

	protected float GetOriginalCost() 
	{	
		float totalMultiplier = 1f;
		for (int i = 1; i < rank; i++) 
		{
			totalMultiplier += RankIncrease (i, costVariablesList[mainArrayIndex][0], costVariablesList[mainArrayIndex][1], costVariablesList[mainArrayIndex][2], costVariablesList[mainArrayIndex][3], costVariablesList[mainArrayIndex][4]);
//			totalMultiplier += costVariablesList[mainArrayIndex][0] * (Mathf.Pow(i, costVariablesList[mainArrayIndex][1]) + costVariablesList[mainArrayIndex][2] * (Mathf.Exp(-((i - costVariablesList[mainArrayIndex][3]) * (i - costVariablesList[mainArrayIndex][3]))/(2 * costVariablesList[mainArrayIndex][4] * costVariablesList[mainArrayIndex][4]))/(costVariablesList[mainArrayIndex][4] * Mathf.Sqrt(2 * Mathf.PI)))); 
		}
		return cost / totalMultiplier;
	}

	public float[] GetCostArray() 
	{
		return costVariablesList [mainArrayIndex];
	}

	public Dictionary<StatsType, float[]> GetRewardDick() 
	{
		return rewardVariablesDickList [mainArrayIndex];
	}

	protected bool EnoughResources() 
	{
		if (cost > GameManager.HumanPlayer.GetResource(ResourceType.Unique)) 
		{
			StartCoroutine(ChangeTextColorToRed());
			return false;
		}
		return true;
	}

	private IEnumerator ChangeTextColorToRed() 
	{
		resText.color = Color.red;
		yield return new WaitForSeconds (1f);
		resText.color = Color.black;
	}
}
