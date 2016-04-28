using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class PopulationsButton : MainButton 
{	
//	public GameObject maincam;
//	public NonOverlayCamera popcam;
	public PopulationPanel popPanel;
//	public Button BottomRightButton;
//	public GameObject topPanel;
//	public Sprite[] BottomRightButtonSprites;
	public static bool popGraphOpen;

	public void OpenPopGraph() 
	{
		NonOverlayCamera.Enable (PanelButtonType.PopulationsButton);
		popGraphOpen = true;
//		GameManager.HumanPlayer.userInput.Deselect ();
//		GameManager.HumanPlayer.userInput.enabled = false;
//		GameManager.Hud.gameObject.SetActive (false);
//		maincam.gameObject.SetActive (false);
//		popcam.gameObject.SetActive (true);
		if (popPanel.showingStats) 
		{
			popPanel.StartCheckStats ();
		}
	}

	public void ClosePopGraph() 
	{
		popGraphOpen = false;
		NonOverlayCamera.Disable ();
//		GameManager.HumanPlayer.userInput.enabled = true;
//		maincam.gameObject.SetActive (true);
//		GameManager.Hud.gameObject.SetActive (true);
//		popcam.gameObject.SetActive (false);
	}
}
