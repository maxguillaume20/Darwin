using UnityEngine;
using System.Collections;
using RTS;

public class RepairButton : PanelButton 
{
	private Building currBuilding;
	private float rotateSpeed = 65;
	private float maxRotation = 30;

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = PanelButtonType.RepairButton;
		button.interactable = false;
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		if (wo) 
		{
			currBuilding = wo as Building;
			if (currBuilding.healthArray[0] >= currBuilding.healthArray[1]) button.interactable = false;
			else button.interactable = true;
			if (currBuilding.repairing) 
			{
				StartCoroutine(RotateHammer());
				button.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.Off);
				button.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
			}
			else 
			{
				button.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.Off);
				button.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
			}
		}
	}

	private void Update ()
	{
		if (!button.interactable && currBuilding.healthArray[0] < currBuilding.healthArray[1] && currBuilding.healthArray[0] > 0f) 
		{
			button.interactable = true;
		}
		else if (button.interactable && (currBuilding.healthArray[0] >= currBuilding.healthArray[1] || currBuilding.healthArray[0] <= 0f)) 
		{
			button.interactable = false;
		}
	}

	public void StartRepair() 
	{
		currBuilding.StartRepair ();
		StartCoroutine (RotateHammer ());
		button.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.Off);
		button.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
	}

	public void CancelRepair() 
	{
		currBuilding.repairing = false;
		button.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.Off);
		button.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
	}

	private IEnumerator RotateHammer() 
	{
		float rotation = 0f;
		while (currBuilding.repairing) 
		{
			rotation += rotateSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler( new Vector3 ( 0f, 0f, rotation % maxRotation));
			yield return null;
		}
		transform.rotation = Quaternion.Euler(new Vector3 ( 0f, 0f, 0f));
		if (currBuilding.healthArray[0] >= currBuilding.healthArray[1]) button.interactable = false;
		button.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.Off);
		button.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
	}
}
