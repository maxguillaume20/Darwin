using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class UnitsButton : PanelButton
{
	public UnitsMenu unitsMenu;
	private Text mainText;

	protected override void Awake ()
	{
		base.Awake ();
		mainText = GetComponentInChildren<Text> ();
		buttonID = PanelButtonType.UnitsButton;
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		if (wo) 
		{
			MobTrainer mobTrainer = wo as MobTrainer;
			mainText.text = mobTrainer.unitName + "s";
		}
	}

	public void OpenUnitsStuff() 
	{
		CloseWOButtons ();
		unitsMenu.gameObject.SetActive (true);
		GameManager.Hud.OpenPanelButton (PanelButtonType.BackButton);
		BackButton.SetGameObjectsToClose(new GameObject[] {unitsMenu.gameObject});
	}
}
