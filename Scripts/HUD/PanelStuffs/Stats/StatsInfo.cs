using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class StatsInfo : PanelButton
{
	public static Dictionary<AttackType, Sprite> attackTypeSpriteDick { get; set; }
	private Dictionary<StatsType, StatsInfoText[]> statsTextsDick = new Dictionary<StatsType, StatsInfoText[]> ();
	public Image thisPanel;
	public Text nameText;
	public Image[] staticImages;
	private Dictionary<StatsType, int[]> statsIndecesDick = new Dictionary<StatsType, int[]> {{StatsType.Health, new int[] {1, 2}}, {StatsType.Defense, new int[] {0,5}}, {StatsType.Attack, new int[] {0}}, {StatsType.MobileStats, new int[] {0,1}}, {StatsType.RangedStats, new int[] {0,1}}};
	private List<StatsType> woStatsTypeList = new List<StatsType> ();
	public Image[] attackTypeImages;

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = PanelButtonType.StatsInfo;
		thisPanel.rectTransform.sizeDelta = new Vector2 (Screen.width * GameManager.panelWidth, Screen.height * 5f / 4f);
		attackTypeSpriteDick = new Dictionary<AttackType, Sprite> ();
		for (int i = 0; i < GameManager.attackTypeArray.Length; i ++) 
		{
			attackTypeSpriteDick.Add (GameManager.attackTypeArray[i], staticImages[2 + i].sprite);
		}
		foreach (StatsType statsType in statsIndecesDick.Keys) 
		{
			int arrayLength = 2;
			if (statsType == StatsType.Defense) 
			{
				arrayLength = 10;
			}
			else if (statsType == StatsType.Attack)
			{
				arrayLength = 4;
			}
			statsTextsDick.Add (statsType, new StatsInfoText[arrayLength]);
		}
		StatsInfoText[] statsTextsArray = GetComponentsInChildren <StatsInfoText> ();
		foreach (StatsInfoText statsText in statsTextsArray) 
		{
			statsText.Iniaite ();
			statsTextsDick[statsText.statsType][statsText.indexArray] = statsText;
		}
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		if (wo) 
		{
			GameManager.Hud.Infotext.gameObject.SetActive (false);
			nameText.gameObject.SetActive (true);
			nameText.text = wo.name;
			foreach (Image image in attackTypeImages) image.gameObject.SetActive(true);
			woStatsTypeList.Clear ();
			foreach (StatsType statsType in statsIndecesDick.Keys) 
			{
				if (wo.statsDick.ContainsKey(statsType)) 
				{
					woStatsTypeList.Add (statsType);
					if (statsType != StatsType.Attack) 
					{
						int textIndex = 0;
						for (int i = statsIndecesDick[statsType][0]; i < statsIndecesDick[statsType][1]; i ++) 
						{
							statsTextsDick[statsType][textIndex].SetStat (wo.statsDick[statsType][i]);
							if (statsType != StatsType.Defense) 
							{
								float increase = wo.statsDick[statsType][i] - GameManager.baseStatsDick[wo.name][statsType][i];
								statsTextsDick[statsType][textIndex + 1].SetIncrease (increase);
							}
							else 
							{
								statsTextsDick[statsType][textIndex + 1].SetIncrease ((wo.statsDick[statsType][i] - GameManager.baseStatsDick[wo.name][statsType][i]) * 100f);
							}
							textIndex += 2;
						}
					}
					else 
					{
						statsTextsDick[statsType][0].SetStat (wo.statsDick[statsType][0] * wo.statsDick[statsType][1]);
						float dpsIncrease = wo.statsDick[statsType][0] * wo.statsDick[statsType][1] - GameManager.baseStatsDick[wo.name][statsType][0] * GameManager.baseStatsDick[wo.name][statsType][1];
						statsTextsDick[statsType][1].SetIncrease (dpsIncrease);
						for (int i = 0; i < wo.attackTypeList.Count; i ++) 
						{
							attackTypeImages[i].sprite = attackTypeSpriteDick[wo.attackTypeList[i]];
						}
						if (wo.attackTypeList.Count == 1) 
						{
							attackTypeImages[1].gameObject.SetActive(false);
							attackTypeImages[0].rectTransform.anchorMin = new Vector2 (0.075f, attackTypeImages[0].rectTransform.anchorMin.y);
							attackTypeImages[0].rectTransform.anchorMax = new Vector2 (0.225f, attackTypeImages[0].rectTransform.anchorMax.y);
						}
						else 
						{
							attackTypeImages[0].rectTransform.anchorMin = new Vector2 (0f, attackTypeImages[0].rectTransform.anchorMin.y);
							attackTypeImages[0].rectTransform.anchorMax = new Vector2 (0.15f, attackTypeImages[0].rectTransform.anchorMax.y);
						}
					}
				}
				else 
				{
					statsTextsDick[statsType][0].text.text = "n/a";
					statsTextsDick[statsType][1].text.text = "";
					if (statsType == StatsType.Attack) 
					{
						foreach (Image image in attackTypeImages) image.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	private void Update() 
	{
		foreach (StatsType statsType in woStatsTypeList) 
		{
			if (statsType != StatsType.Attack) 
			{
				int textIndex = 0;
				for (int i = statsIndecesDick[statsType][0]; i < statsIndecesDick[statsType][1]; i ++) 
				{
					if (statsTextsDick[statsType][textIndex].amount != wo.statsDick[statsType][i]) 
					{
						statsTextsDick[statsType][textIndex].SetStat (wo.statsDick[statsType][i]);
						if (statsType != StatsType.Defense) 
						{
							float increase = wo.statsDick[statsType][i] - GameManager.baseStatsDick[wo.name][statsType][i];
							statsTextsDick[statsType][textIndex + 1].SetIncrease (increase);
						}
						else 
						{
							statsTextsDick[statsType][textIndex + 1].SetIncrease ((wo.statsDick[statsType][i] - GameManager.baseStatsDick[wo.name][statsType][i]) * 100f);
						}
					}
					textIndex += 2;
				}
			}
			else if (statsTextsDick[statsType][0].amount != wo.statsDick[statsType][0] * wo.statsDick[statsType][1]) 
			{
				statsTextsDick[statsType][0].SetStat (wo.statsDick[statsType][0] * wo.statsDick[statsType][1]);
				float increase = wo.statsDick[statsType][0] * wo.statsDick[statsType][1] - GameManager.baseStatsDick[wo.name][statsType][0] * GameManager.baseStatsDick[wo.name][statsType][1];
				statsTextsDick[statsType][1].SetIncrease (increase);
			}
		}
	}

	protected override void OnDisable() 
	{
		GameManager.Hud.Infotext.gameObject.SetActive (true);
	}
}
