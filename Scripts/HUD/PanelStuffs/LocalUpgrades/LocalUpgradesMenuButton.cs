using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class LocalUpgradesMenuButton : MonoBehaviour 
{
	private LocalUpgradesMenu locUpMenu;
	public int locUpIndex;
	private Button button;
	public Text rankText;
	public Text[] resTexts;
	public Image[] resImages;
	private Image[] statImages;
	private static float resTextWidth = 0.2f;
	private static float resImageWidth = 0.1f;

	public void Initiate (int thisLocUpIndex) 
	{
		locUpMenu = GetComponentInParent<LocalUpgradesMenu> ();
		locUpIndex = thisLocUpIndex;
		button = GetComponent<Button> ();
		Image[] allImages = GetComponentsInChildren<Image> ();
		statImages = new Image[allImages.Length - 1];
		for (int i = 1; i < allImages.Length; i ++) 
		{
			statImages[i - 1] = allImages[i];
			statImages[i - 1].gameObject.SetActive (false);
		}
	}

	private void OnEnable () 
	{	
		if (LocalUpgradesMenu.selectedBuilding) 
		{
			if (locUpIndex < LocalUpgradesMenu.selectedBuilding.localUpgradesList.Count) 
			{
				rankText.gameObject.SetActive (true);
				if (LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][0].rank > 1) 
				{
					rankText.text = (LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][0].rank - 1).ToString();
				}
				else 
				{
					rankText.text = "";
				}
				// set stat sprites
				int lUCount = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex].Length;
				bool allSameStat = false;
				Sprite firstMiscSprite = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][0].miscUpgradeSprite;
				StatsType firstStatsType = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][0].statsType;
				for (int i = 0; i < lUCount; i ++) 
				{
					Sprite miscSprite = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][i].miscUpgradeSprite;
					StatsType upStatsType = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][i].statsType;
					if (miscSprite != firstMiscSprite || upStatsType != firstStatsType) 
					{
						break;
					}
					else if (i == lUCount - 1) 
					{
						allSameStat = true;
					}
				}
				if (allSameStat) 
				{
					lUCount = 1;
				}
				for (int i = 0; i < lUCount; i ++) 
				{
					statImages[i].gameObject.SetActive (true);
					Sprite miscSprite = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][i].miscUpgradeSprite;
					if (miscSprite) 
					{
						statImages[i].sprite = miscSprite;
					}
					else 
					{
						StatsType statsType = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][i].statsType;
						if (statsType == StatsType.MobTrainerStats) 
						{
							statImages[i].sprite = HUD.speciesPopSpriteDick[LocalUpgradesMenu.selectedBuilding.GetSpecies()];
						}
						else if (statsType == StatsType.ResourceStats) 
						{
							ResourceType resType = (LocalUpgradesMenu.selectedBuilding as Resource).resource;
							statImages[i].sprite = HUD.speciesResourceSpriteDick[LocalUpgradesMenu.selectedBuilding.GetSpecies()][resType];
						}
						else 
						{
							statImages[i].sprite = LocalUpgradesMenu.statSpritesDick[statsType];
						}
					}
					statImages[i].rectTransform.anchorMin = new Vector2 (0.35f - 0.16f * (lUCount - 1) + i * 0.32f, 0.1f);
					statImages[i].rectTransform.anchorMax = new Vector2 (0.65f - 0.16f * (lUCount - 1) + i * 0.32f, 0.9f);
				}
				// set res sprites and costs
				LocalUpgrade lU = LocalUpgradesMenu.selectedBuilding.localUpgradesList [locUpIndex][0];
				int resCount = 0;
				for (int i = 0; i < lU.costArray.Length; i ++)
				{
					if (lU.costArray[i] > 0) 
					{
						resTexts[resCount].text = lU.costArray[i].ToString ("0");
						resImages[resCount].sprite = HUD.speciesResourceSpriteDick[LocalUpgradesMenu.selectedBuilding.GetSpecies()][GameManager.resourceTypeArraytoDick[i]];
						resCount ++;
					}
				}
				// set res positions
				for (int i = 0; i < lU.costArray.Length; i ++) 
				{
					if (i < resCount) 
					{
						float totalWidth = resImageWidth + resTextWidth;
						float resImageMinX = 0.5f - (totalWidth / 2f)  - (resCount - 1) * (totalWidth / 2f) + totalWidth * i;
						float resTextMinX = resImageMinX + resImageWidth;
						resImages[i].gameObject.SetActive (true);
						resImages[i].rectTransform.anchorMin = new Vector2 (resImageMinX, resImages[i].rectTransform.anchorMin.y);
						resImages[i].rectTransform.anchorMax = new Vector2 (resTextMinX, resImages[i].rectTransform.anchorMax.y);
						resTexts[i].gameObject.SetActive (true);
						resTexts[i].rectTransform.anchorMin = new Vector2 (resTextMinX, resTexts[i].rectTransform.anchorMin.y);
						resTexts[i].rectTransform.anchorMax = new Vector2 (resTextMinX + resTextWidth, resTexts[i].rectTransform.anchorMax.y);
					}
					else 
					{
						resImages[i].gameObject.SetActive (false);
						resTexts[i].gameObject.SetActive (false);
					}

				}
				button.interactable = !(LocalUpgradesMenu.selectedBuilding.GetRemainingLocUpgrades () <= 0);
			}
			else 
			{
				gameObject.SetActive (false);
			}
		}
	}

	private void OnDisable () 
	{
		rankText.gameObject.SetActive (false);
		foreach (Image i in statImages) i.gameObject.SetActive (false);
		foreach (Text t in resTexts) t.gameObject.SetActive (false);
		foreach (Image i in resImages) i.gameObject.SetActive (false);
	}

	public void SelectButton () 
	{
		if (locUpMenu.selectedLUB != this) 
		{
			if (locUpMenu.selectedLUB != null) 
			{
				locUpMenu.selectedLUB.button.image.color = locUpMenu.selectedLUB.button.colors.normalColor;
			}
			locUpMenu.selectedLUB = this;
			button.image.color = button.colors.highlightedColor;
			locUpMenu.SetDescriptionText ();
		}
		else if (HUD.EnoughResources (LocalUpgradesMenu.selectedBuilding.localUpgradesList[locUpIndex][0].costArray, resTexts)) 
		{
			locUpMenu.InitiateUpgrade (locUpIndex);
		}
	}
}
