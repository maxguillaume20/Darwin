using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class FunctionButton1 : PanelButton
{
	private static Text mainText { get; set; }

	protected override void Awake ()
	{
		base.Awake ();
		mainText = GetComponentInChildren<Text> ();
		buttonID = PanelButtonType.FunctionButton1;
		mainText = GetComponentInChildren<Text> ();
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		if (wo) 
		{
			button.interactable = true;
			mainText.text = wo.GetFunctionButton1Text();
			button.onClick.RemoveAllListeners();
			if (wo as SpeciesUnitTrainer) 
			{
				SpeciesUnitTrainer mobTrainer = wo as SpeciesUnitTrainer;
				button.onClick.AddListener(mobTrainer.DeployGarrisonUnits);
				if (mobTrainer.GetCurrentUnits() == 0) 
				{
					button.interactable = false;
				}
				StartCoroutine(SpeciesUnitTrainerUpdate(mobTrainer));
			}
		}
	}

	private IEnumerator SpeciesUnitTrainerUpdate(SpeciesUnitTrainer mobTrainer) 
	{
		bool deploy = false;
		if (mobTrainer.GetGarrisonedUnitsCount() > 0) deploy = true;
		while (gameObject.activeSelf) 
		{
			if (!button.interactable && mobTrainer.GetCurrentUnits() > 0 && !mobTrainer.garrisoningUnits) 
			{
				button.interactable = true;
				SetText(mobTrainer.GetFunctionButton1Text());
			}
			else if (button.interactable && (mobTrainer.GetCurrentUnits() == 0 || mobTrainer.garrisoningUnits)) 
			{
				button.interactable = false;
				SetText(mobTrainer.GetFunctionButton1Text());
			}
			if (mobTrainer.GetGarrisonedUnitsCount() > 0 && !deploy || mobTrainer.GetGarrisonedUnitsCount() == 0 && deploy) 
			{
				SetText(mobTrainer.GetFunctionButton1Text());
				deploy = !deploy;
			}
			yield return null;
		}
	}

	public static void SetText (string text) 
	{
		mainText.text = text;
	}
}
