using UnityEngine;
using System.Collections;
using RTS;

public class StatsButton : PanelButton 
{
	public StatsInfo statsInfo;

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = PanelButtonType.StatsButton;
	}

	public void OpenStatsInfo() 
	{
		CloseWOButtons ();
		statsInfo.gameObject.SetActive (true);
		GameManager.Hud.OpenPanelButton (PanelButtonType.BackButton);
		BackButton.SetGameObjectsToClose (new GameObject[] {statsInfo.gameObject});
	}
}
