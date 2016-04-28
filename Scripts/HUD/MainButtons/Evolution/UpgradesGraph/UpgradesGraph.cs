using UnityEngine;
using UnityEngine.UI;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public class UpgradesGraph : MonoBehaviour 
{
	public GameObject[] graphComponents;
	private Text secondaryRewardText;
	private Image secondaryRewardLegend;
	public Text messageText;
	private CostPoint[] costPoints;
	private RewardPoint[] rewardPoints;
	public Button switchGraphsButton;
	private Dictionary<EvolutionPanelButton, bool> displayTotalChangeGraphDick = new Dictionary<EvolutionPanelButton, bool> ();
	public Vector2 maxAxisAnchors;
	public Vector2 minAxisAnchors;
	private float yScalingFactor = 0.15f;
	private float xScalingFactor = 0.02f;

	void Awake() 
	{
		costPoints = GetComponentsInChildren<CostPoint> ();
		rewardPoints = GetComponentsInChildren<RewardPoint> ();
		for (int i = 0; i < costPoints.Length; i++) 
		{
			costPoints[i].StartBitch();
			rewardPoints[i * 2].StartBitch();
			rewardPoints[i * 2 + 1].StartBitch();
		}
		// it doesn't matttteeeerrrrr
		float xMax = 0f;
		float xMin = 0f;
		float yMax = 0f;
		float yMin = 0f;
		foreach (GameObject go in graphComponents) 
		{
			switch (go.name) 
			{
			case "MainRewardLegend":
				go.GetComponent<Image>().color = GameManager.HumanPlayer.color;
				break;
			case "SecondaryRewardLegend":
				secondaryRewardLegend = go.GetComponent<Image>();
				break;
			case "SecondaryRewardLegendText":
				secondaryRewardText = go.GetComponent<Text>();
				break;
			case "GraphYAxis":
				RectTransform goRectTransform = go.GetComponent<RectTransform>();
				xMin = goRectTransform.anchorMax.x;
				yMax = goRectTransform.anchorMax.y;
				break;
			case "GraphXAxis":
				RectTransform gORectTransform = go.GetComponent<RectTransform>();
				xMax = gORectTransform.anchorMax.x;
				yMin = gORectTransform.anchorMax.y;
				break;
			}
		}
		secondaryRewardLegend.color = GetRewardPointColor(1);
		secondaryRewardLegend.gameObject.SetActive (false);
		secondaryRewardText.gameObject.SetActive (false);
		maxAxisAnchors = new Vector2 (xMax, yMax);
		minAxisAnchors = new Vector2 (xMin, yMin);
		foreach (EvolutionPanelButton epb in GameManager.evoPanel.evoPanButts) 
		{
			displayTotalChangeGraphDick.Add(epb, false);
		}
	}

	void OnEnable() 
	{
		foreach (GameObject go in graphComponents) 
		{
			if (go != secondaryRewardText.gameObject && go != secondaryRewardLegend.gameObject) 
			{
				go.SetActive(true);
			}
		}
		messageText.gameObject.SetActive (true);
		switchGraphsButton.gameObject.SetActive (true);
		SetGraph ();
	}
	
	void OnDisable() 
	{
		foreach (GameObject go in graphComponents) go.SetActive(false);
		foreach (CostPoint costPoint in costPoints) costPoint.image.enabled = false;
		foreach (RewardPoint rewardPoint in rewardPoints) rewardPoint.image.enabled = false;
		messageText.gameObject.SetActive (false);
		switchGraphsButton.gameObject.SetActive (false);
	}

	public void SwitchGraphs() 
	{
		foreach (CostPoint costPoint in costPoints) costPoint.image.enabled = false;
		foreach (RewardPoint rewardPoint in rewardPoints) rewardPoint.image.enabled = false;
		displayTotalChangeGraphDick [GameManager.evoPanel.selectedEPB] = !displayTotalChangeGraphDick [GameManager.evoPanel.selectedEPB];
		SetGraph ();
	}

	private void SetGraph() 
	{
		SetLegend ();
		if (!displayTotalChangeGraphDick[GameManager.evoPanel.selectedEPB]) 
		{
			PercentChangeGraph();
		}
		else TotalPercentChangeGraph();
	}

	private void PercentChangeGraph() 
	{
		messageText.text = "Percent Change Increase";
		switchGraphsButton.transform.localEulerAngles = new Vector3 (0f, 0f, 180f);
		float[] costArray = GameManager.evoPanel.selectedEPB.GetCostArray ();
		Dictionary<StatsType, float[]> rewardDick = GameManager.evoPanel.selectedEPB.GetRewardDick();
		StatsType[] statsTypesArray = rewardDick.Keys.ToArray();
		int secondaryUpgradeCount = 0;
		for (int i = 1; i <= GameManager.evoPanel.selectedEPB.rank && i <= costPoints.Length; i ++) 
		{
			Vector3 costPosition = new Vector3 (Screen.width * (minAxisAnchors[0] + xScalingFactor * i), Screen.height * (minAxisAnchors[1] + yScalingFactor * EvolutionPanelButton.RankIncrease(i, costArray[0], costArray[1], costArray[2], costArray[3], costArray[4])), 0f);
			costPoints[i - 1].transform.localPosition = costPosition;
			if (costPosition.y < Screen.height * maxAxisAnchors[1]) costPoints[i - 1].image.enabled = true;
			for (int j = 0; j < statsTypesArray.Length; j++) 
			{
				Vector3 rewardPosition = new Vector3(Screen.width * (minAxisAnchors[0] + xScalingFactor * i), Screen.height * (minAxisAnchors[1] + yScalingFactor * Mathf.Abs(EvolutionPanelButton.RankIncrease(i,rewardDick[statsTypesArray[j]][0], rewardDick[statsTypesArray[j]][1], rewardDick[statsTypesArray[j]][2], rewardDick[statsTypesArray[j]][3], rewardDick[statsTypesArray[j]][4]))), 0f);
				rewardPoints[i + j + secondaryUpgradeCount - 1].transform.localPosition = rewardPosition;
				rewardPoints[i + j + secondaryUpgradeCount - 1].image.color = GetRewardPointColor(j);
				if (rewardPosition.y < Screen.height * maxAxisAnchors[1]) rewardPoints[i + j + secondaryUpgradeCount - 1].image.enabled = true;
				secondaryUpgradeCount += j;
			}
		}
	}

	private void TotalPercentChangeGraph() 
	{
		messageText.text = "Total Percent Change";
		Vector3 costPosition = new Vector3 (Screen.width * minAxisAnchors[0], Screen.height * minAxisAnchors[1]);
		Vector3[] rewardPositions = new Vector3[] {costPosition, costPosition};
		switchGraphsButton.transform.localEulerAngles = Vector3.zero;
		float[] costArray = GameManager.evoPanel.selectedEPB.GetCostArray ();
		Dictionary<StatsType, float[]> rewardDick = GameManager.evoPanel.selectedEPB.GetRewardDick();
		StatsType[] statsTypesArray = rewardDick.Keys.ToArray();
		int secondaryUpgradeCount = 0;
		for (int i = 1; i <= GameManager.evoPanel.selectedEPB.rank && i <= costPoints.Length; i ++) 
		{
			costPosition = new Vector3 (costPosition.x + Screen.width * xScalingFactor, costPosition.y + Screen.height * yScalingFactor * EvolutionPanelButton.RankIncrease(i, costArray[0], costArray[1], costArray[2], costArray[3], costArray[4]), 0f);
			costPoints[i - 1].transform.localPosition = costPosition;
			if (costPosition.y < Screen.height * maxAxisAnchors[1]) costPoints[i - 1].image.enabled = true;
			for (int j = 0; j < statsTypesArray.Length; j++) 
			{
				rewardPositions[j] = new Vector3 (rewardPositions[j].x + Screen.width * xScalingFactor, rewardPositions[j].y + Screen.height * yScalingFactor * Mathf.Abs(EvolutionPanelButton.RankIncrease(i,rewardDick[statsTypesArray[j]][0], rewardDick[statsTypesArray[j]][1], rewardDick[statsTypesArray[j]][2], rewardDick[statsTypesArray[j]][3], rewardDick[statsTypesArray[j]][4])), 0f);
				rewardPoints[i + j + secondaryUpgradeCount - 1].transform.localPosition = rewardPositions[j];
				rewardPoints[i + j + secondaryUpgradeCount - 1].image.color = GetRewardPointColor(j);
				if (rewardPositions[j].y < Screen.height * maxAxisAnchors[1]) rewardPoints[i + j + secondaryUpgradeCount - 1].image.enabled = true;
				secondaryUpgradeCount += j;
			}
		}
	}

	// maybe flesh out, if not then delete
	private void SetLegend() 
	{
		if (GameManager.evoPanel.selectedEPB.GetRewardDick().Keys.Count > 1) 
		{
			secondaryRewardText.gameObject.SetActive(true);
			secondaryRewardLegend.gameObject.SetActive(true);
		}
	}

	private Color GetRewardPointColor(int index) 
	{
		if (index > 0) 
		{
			switch (GameManager.HumanPlayer.species) 
			{
			case Species.Deer:
				return new Color (0.7f, 0f, 0f);
			case Species.Bunnies:
				return new Color (0.25f, 0.25f, 0.25f);
			case Species.Sheep:
				return Color.cyan;
			}
		}
		return GameManager.HumanPlayer.color;
	}
}
