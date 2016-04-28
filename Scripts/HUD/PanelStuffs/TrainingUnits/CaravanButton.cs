using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class CaravanButton : PanelButton
{
	private StrategicPoint stratpt;
	private bool settingPopCount;
	private int popCount;
	private int currMaxCarPopCount;
	public Button cancelButton;
	public Button checkButton;
	public Text mainText;
	public Text secondaryText;
	public GameObject[] addMinusButtons;

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = PanelButtonType.CaravanButton;
	}	
	 protected override void OnEnable ()
	{

		base.OnEnable ();
		if (wo) 
		{
			popCount = 0;
			currMaxCarPopCount = -1;
			settingPopCount = false;
			button.interactable = true;
			button.image.color = Color.white;
			stratpt = wo.GetComponent<StrategicPoint>();
			if (stratpt.trainingCaravan) 
			{
				secondaryText.gameObject.SetActive(true);
				cancelButton.gameObject.SetActive (true);
				cancelButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
				cancelButton.onClick.SetPersistentListenerState(1, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
			}
		}
	}

	protected override void OnDisable ()
	{
		foreach (GameObject go in addMinusButtons) go.SetActive(false);
		secondaryText.gameObject.SetActive (false);
		TrainingProgressBar.CloseBar ();
		cancelButton.gameObject.SetActive (false);
		checkButton.gameObject.SetActive (false);
	}

	private void Update ()
	{
		if (settingPopCount) 
		{
			if (currMaxCarPopCount != stratpt.GetMaxCarPopCount()) 
			{
				if (popCount >= stratpt.GetMaxCarPopCount()) 
				{
					popCount = stratpt.GetMaxCarPopCount();
					if (addMinusButtons[0].activeSelf) addMinusButtons[0].SetActive(false);
				}
				secondaryText.text = popCount + " / " + stratpt.GetMaxCarPopCount(); 
				currMaxCarPopCount = stratpt.GetMaxCarPopCount();
			}
			if (popCount > stratpt.GetMaxCarPopCount()) 
			{
				popCount = stratpt.GetMaxCarPopCount();
				secondaryText.text = popCount + " / " + stratpt.GetMaxCarPopCount(); 
				if (addMinusButtons[0].activeSelf) addMinusButtons[0].SetActive(false);
			}
			else if (popCount == stratpt.GetMaxCarPopCount() && addMinusButtons[0].activeSelf) 
			{
				addMinusButtons[0].SetActive(false);
			}
			else if (popCount < stratpt.GetMaxCarPopCount() && !addMinusButtons[0].activeSelf) 
			{
				addMinusButtons[0].gameObject.SetActive(true);
			}
			else if (popCount > 0 && !addMinusButtons[1].activeSelf) 
			{
				addMinusButtons[1].gameObject.SetActive(true);
			}
			else if (popCount <= 0 && addMinusButtons[1].activeSelf) 
			{
				popCount = 0;
				secondaryText.text = popCount + " / " + stratpt.GetMaxCarPopCount(); 
				addMinusButtons[1].SetActive(false);
			}
		}
		else if (stratpt.trainingCaravan)
		{
			if (!TrainingProgressBar.isActive) TrainingProgressBar.OpenBar (buttonID);
//			if (secondaryText.alignment != TextAnchor.MiddleRight) secondaryText.alignment = TextAnchor.MiddleRight;
			TrainingProgressBar.ChangeProgress (stratpt.GetCarTrainingProgress ());
			TrainingProgressBar.countText.text = stratpt.CaravanTrainingCount();
		}
		// Just stopped training
		else if (cancelButton.gameObject.activeSelf)
		{
			cancelButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
			cancelButton.onClick.SetPersistentListenerState(1, UnityEngine.Events.UnityEventCallState.Off);
			TrainingProgressBar.CloseBar ();
			cancelButton.gameObject.SetActive (false);
			secondaryText.gameObject.SetActive (false);
		}
	}

	public void SetPopCount() 
	{
		settingPopCount = true;
		button.interactable = false;
		button.image.color = Color.clear;
		foreach (GameObject go in addMinusButtons) go.SetActive(true);
		cancelButton.gameObject.SetActive (true);
		checkButton.gameObject.SetActive (true);
		secondaryText.gameObject.SetActive (true);
		secondaryText.alignment = TextAnchor.MiddleCenter;
		TrainingProgressBar.CloseBar ();
		secondaryText.text = popCount + " / " + stratpt.GetMaxCarPopCount();
		cancelButton.onClick.SetPersistentListenerState(1, UnityEngine.Events.UnityEventCallState.Off);
		cancelButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
	}

	public void AddCiviliantoCaravan() 
	{
		popCount ++;
		secondaryText.text = popCount + " / " + stratpt.GetMaxCarPopCount(); 
	}

	public void MinusCiviliantoCaravan() 
	{
		popCount --;
		secondaryText.text = popCount + " / " + stratpt.GetMaxCarPopCount(); 
	}

	public void StartTraining() 
	{
		if (popCount > 0)
		{
			foreach (GameObject go in addMinusButtons) go.SetActive(false);
			checkButton.gameObject.SetActive (false);
			settingPopCount = false;
			button.interactable = true;
			button.image.color = Color.white;
			secondaryText.text = "";
			cancelButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
			cancelButton.onClick.SetPersistentListenerState(1, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
//			TrainingProgressBar.OpenBar ();
			stratpt.ChangeLocalPopulation(-popCount);
			stratpt.StartTrainingNewCaravan(popCount);
		}
	}

	public void CancelSetPopCount() 
	{
		settingPopCount = false;
		button.interactable = true;
		button.image.color = Color.white;
		foreach (GameObject go in addMinusButtons) go.SetActive(false);
		checkButton.gameObject.SetActive (false);
		cancelButton.onClick.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
		cancelButton.onClick.SetPersistentListenerState(1, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
		secondaryText.gameObject.SetActive (false);
		if (!stratpt.trainingCaravan) 
		{
			cancelButton.gameObject.SetActive (false);
		}
	}

	public void CancelTraining()
	{
		stratpt.ChangeLocalPopulation (stratpt.CancelTrainingCaravan ());
	}
}
