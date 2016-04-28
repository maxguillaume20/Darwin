using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using RTS;

public class BuildingMenuButton : PanelButton
{
	
	public BuildMenu buildmenu;

	protected override void Awake()
	{
		base.Awake ();
		buttonID = PanelButtonType.BuildingMenuButton;
	}

	public void OpenBuildMenu()
	{
		GameManager.Hud.Infotext.gameObject.SetActive (false);
		buildmenu.gameObject.SetActive (true);
		CloseWOButtons ();
		GameManager.Hud.OpenPanelButton (PanelButtonType.BackButton);
		BackButton.SetGameObjectsToOpen (new GameObject[] {GameManager.Hud.Infotext.gameObject});
		BackButton.SetGameObjectsToClose (new GameObject[] {buildmenu.gameObject, /*GameManager.Hud.Infotext.statsText.gameObject*/});
	}	
}
