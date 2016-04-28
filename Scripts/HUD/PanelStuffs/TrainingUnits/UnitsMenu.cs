using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class UnitsMenu : PanelButton 
{
	private MobTrainer mobTrainer;
	public Button plusButton;
	public Button minusButton;
	public Text unitsText;
	public Text[] resTexts;
	public Image[] resImages;
//	public Text popText;
//	public Image popImage;

	protected override void Awake() 
	{
		base.Awake ();
		buttonID = PanelButtonType.UnitsMenu;
	}

	protected override void OnEnable() 
	{
		base.OnEnable ();
		if (wo) 
		{
			plusButton.onClick.RemoveAllListeners ();
			minusButton.onClick.RemoveAllListeners ();
			GameManager.Hud.Infotext.SetPopInfoActive (false);
			mobTrainer = wo as MobTrainer;
			unitsText.gameObject.SetActive (true);
			unitsText.text = mobTrainer.GetTrainingText ();
			if (mobTrainer as SpeciesMobTrainer) 
			{
				StartCoroutine (LivingTrainer ());
			}
			else 
			{
				StartCoroutine (InanimateTrainer ());
			}

//			if (mobTrainer.idealMobCount < mobTrainer.mobTrainerStatsArray[0]) 
//			{
//				plusButton.gameObject.SetActive(true);
//			}
//			if (mobTrainer.idealMobCount > 0) 
//			{
//				minusButton.gameObject.SetActive(true);
//			}

//			popImage.gameObject.SetActive (true);
//			popImage.sprite = HUD.speciesPopSpriteDick[wo.GetSpecies()];
//			popText.gameObject.SetActive (true);
//			popText.text = mobTrainer.mobPopCount.ToString();
		}
	}

	private IEnumerator LivingTrainer () 
	{
		plusButton.onClick.AddListener (delegate {ChangeIdealUnits (1);});
		minusButton.onClick.AddListener (delegate {ChangeIdealUnits (-1);});
		SpeciesMobTrainer livingMT = mobTrainer as SpeciesMobTrainer;
		resTexts [1].gameObject.SetActive (true);
		resTexts [1].rectTransform.anchorMax = new Vector2 (0.55f, resTexts [1].rectTransform.anchorMax.y);
		resTexts [1].rectTransform.anchorMin = new Vector2 (0.45f, resTexts [1].rectTransform.anchorMin.y);
		resTexts [1].text = livingMT.mobPopCount.ToString ();
		resImages [1].gameObject.SetActive (true);
		resImages [1].rectTransform.anchorMax = new Vector2 (0.65f, resImages [1].rectTransform.anchorMax.y);
		resImages [1].rectTransform.anchorMin = new Vector2 (0.55f, resImages [1].rectTransform.anchorMin.y);
		resImages [1].sprite = HUD.speciesPopSpriteDick [livingMT.GetSpecies ()];
		while (gameObject.activeSelf) 
		{
			CheckTrainer ();
			if (livingMT.idealMobCount > livingMT.mobTrainerStatsArray[0] && plusButton.gameObject.activeSelf) 
			{
				livingMT.idealMobCount = (int)livingMT.mobTrainerStatsArray[0];
				plusButton.gameObject.SetActive(false);
			}
			else if (livingMT.idealMobCount == livingMT.mobTrainerStatsArray[0] && plusButton.gameObject.activeSelf) 
			{
				plusButton.gameObject.SetActive(false);
			}
			else if (livingMT.idealMobCount < livingMT.mobTrainerStatsArray[0] && !plusButton.gameObject.activeSelf)
			{
				plusButton.gameObject.SetActive(true);
			} 
			else if (livingMT.idealMobCount > 0 && !minusButton.gameObject.activeSelf) 
			{
				minusButton.gameObject.SetActive(true);
			}
			else if (livingMT.idealMobCount <= 0 && minusButton.gameObject.activeSelf) 
			{
				livingMT.idealMobCount = 0;
				minusButton.gameObject.SetActive(false);
			}
			yield return null;
		}
	}

	private IEnumerator InanimateTrainer () 
	{
		plusButton.onClick.AddListener (delegate {StartTraining ();});
		minusButton.onClick.AddListener (delegate {CancelTraining ();});
		NonRelatedUnitTrainer inanimateMT = mobTrainer as NonRelatedUnitTrainer;
		float[] currCostArray = GameManager.DuplicateArray (inanimateMT.costArray);
		int resCount = 0;
		for (int i = 0; i < inanimateMT.costArray.Length; i ++)
		{
			if (inanimateMT.costArray[i] > 0) 
			{
				resTexts[resCount].text = inanimateMT.costArray[i].ToString ("0");
				resImages[resCount].sprite = HUD.speciesResourceSpriteDick[inanimateMT.GetSpecies()][GameManager.resourceTypeArraytoDick[i]];
				resCount ++;
			}
		}
		// set res positions
		float resTextWidth = 0.2f;
		float resImageWidth = 0.1f;
		for (int i = 0; i < inanimateMT.costArray.Length; i ++) 
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
		while (gameObject.activeSelf) 
		{
			CheckTrainer ();
			for (int i = 0; i < currCostArray.Length; i ++) 
			{
				if (currCostArray[i] != inanimateMT.costArray[i]) 
				{
					resTexts[i].text = inanimateMT.costArray[i].ToString();
					currCostArray[i] = inanimateMT.costArray[i];
				}
			}
			if (inanimateMT.currentMobCount + inanimateMT.GetTrainingQueueCount() >= inanimateMT.mobTrainerStatsArray[0] && plusButton.gameObject.activeSelf) 
			{
				plusButton.gameObject.SetActive (false);
			}
			else if (inanimateMT.currentMobCount + inanimateMT.GetTrainingQueueCount() < inanimateMT.mobTrainerStatsArray[0] && !plusButton.gameObject.activeSelf) 
			{
				plusButton.gameObject.SetActive (true);
			}
			if (!mobTrainer.training && minusButton.gameObject.activeSelf) 
			{
				minusButton.gameObject.SetActive (false);
			}
			else if (mobTrainer.training && !minusButton.gameObject.activeSelf) 
			{
				minusButton.gameObject.SetActive (true);
			}
			yield return null;
		}
	}

	private void CheckTrainer () 
	{
		unitsText.text = mobTrainer.GetTrainingText ();
		if (mobTrainer.training) 
		{
			if (!TrainingProgressBar.isActive) 
			{
				TrainingProgressBar.OpenBar (buttonID);
			}
			TrainingProgressBar.ChangeProgress (mobTrainer.GetTrainingProgress ());
		}
		else if (TrainingProgressBar.isActive) TrainingProgressBar.CloseBar ();
	}

	protected override void OnDisable() 
	{
		GameManager.Hud.Infotext.SetPopInfoActive (true);
		TrainingProgressBar.CloseBar ();
		unitsText.gameObject.SetActive (false);
		foreach (Image thingy in resImages) thingy.gameObject.SetActive (false);
		foreach (Text thingy in resTexts) thingy.gameObject.SetActive (false);
		plusButton.gameObject.SetActive(false);
		minusButton.gameObject.SetActive(false);
	}

	public void ChangeIdealUnits(int amount) 
	{
		(mobTrainer as SpeciesMobTrainer).ChangeIdealUnits (amount);
	}

	public void StartTraining () 
	{
		NonRelatedUnitTrainer inanimateMT = (mobTrainer as NonRelatedUnitTrainer);
		if (HUD.EnoughResources (inanimateMT.costArray, resTexts))
		{
			inanimateMT.StartTraining ();
		}
	}
	
	public void CancelTraining () 
	{
		(mobTrainer as NonRelatedUnitTrainer).CancelTraining ();
	}
}
