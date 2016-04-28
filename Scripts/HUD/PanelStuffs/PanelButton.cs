using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class PanelButton : MonoBehaviour 
{
	public Button button;
	protected WorldObject wo;
	protected PanelButtonType buttonID;
	public bool selected;
	
	protected virtual void Awake ()
	{
		button = GetComponent<Button> ();
	}

	protected virtual void Start()
	{

	}

	protected virtual void OnEnable()
	{
		if (GameManager.HumanPlayer.userInput.GetSelectedObjects().Count > 0) 
		{
			wo = GameManager.HumanPlayer.userInput.GetSelectedObjects()[0];
		}
	}

	protected virtual void OnDisable()
	{
	
	}

	protected void CloseWOButtons() 
	{
		List<PanelButtonType> woButtons = wo.GetButtons ();
		for (int i = 0; i < woButtons.Count; i ++) 
		{
			GameManager.Hud.ClosePanelButton(woButtons[i]);
		}
	}

	protected IEnumerator SelectedCountDown() 
	{
		yield return new WaitForSeconds (GameManager.doubleTapButtonTime);
		selected = false;
		button.image.color = button.colors.normalColor;
	}

	public PanelButtonType GetButtonID()
	{
		return buttonID;
	}

	protected IEnumerator ChangeTextColorToRed(Text[] texts) 
	{
		foreach(Text text in texts) text.color = Color.red;
		for (float timer = 0.0f; timer < 1f; timer += Time.deltaTime) yield return null;
		foreach(Text text in texts) text.color = Color.black;
	}

	protected virtual bool EnoughResources(string name, Text[] texts)
	{
		Building building = GameManager.GetGameObject(name).GetComponent<Building> ();
		if (building) 
		{
			foreach (ResourceType resource in BuildMenu.buildingCostDick[name].Keys) 
			{
				if  (GameManager.HumanPlayer.GetResource(resource) < BuildMenu.buildingCostDick[name][resource]) 
				{
					StartCoroutine(ChangeTextColorToRed(texts));
					return false;
				}
			}
		}
		return true;
	}	
}
