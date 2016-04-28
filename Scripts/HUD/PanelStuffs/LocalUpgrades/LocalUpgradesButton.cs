using UnityEngine;
using System.Collections;
using RTS;

public class LocalUpgradesButton : PanelButton 
{
	public LocalUpgradesMenu localUpgradesMenu;

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = PanelButtonType.LocalUpgradesButton;
	}

	public void OpenLocalUpgradesPanel () 
	{
		CloseWOButtons ();
		localUpgradesMenu.Enable (wo as Building);
		GameManager.Hud.OpenPanelButton (PanelButtonType.BackButton);
		BackButton.SetGameObjectsToClose (new GameObject[] {localUpgradesMenu.gameObject});
	}
}
