using UnityEngine;
using System.Collections.Generic;
using RTS;

public class NonOverlayCamera : MonoBehaviour 
{
	public static Camera thisCamera { get; set; }
	//Camera.main returns null when its gameObject is inactive
	private static GameObject mainCamGameObject { get; set; }
	public Canvas nonOverlayCanvas;
	private static Dictionary<PanelButtonType, NonOverlayPanel> panelDick { get; set; }
	private static PanelButtonType activePanelID;

	private void Awake () 
	{
		thisCamera = GetComponent<Camera> ();
		mainCamGameObject = Camera.main.gameObject;
		thisCamera.orthographicSize = Camera.main.orthographicSize;
	}

	private void Start () 
	{
		panelDick = new Dictionary<PanelButtonType, NonOverlayPanel> ();
		foreach (NonOverlayPanel panel in nonOverlayCanvas.GetComponentsInChildren<NonOverlayPanel>()) 
		{
			panelDick.Add (panel.panelID, panel);
			if (panel.panelID != PanelButtonType.PopulationsButton) 
			{
				panel.gameObject.SetActive (false);
			}
		}
		gameObject.SetActive (false);
	}

	public static void Enable (PanelButtonType panelID) 
	{
		GameManager.HumanPlayer.userInput.Deselect ();
		GameManager.HumanPlayer.userInput.enabled = false;
		GameManager.Hud.SetActive (false);
		mainCamGameObject.SetActive (false);
		thisCamera.gameObject.SetActive (true);
		activePanelID = panelID;
		if (activePanelID != PanelButtonType.PopulationsButton) 
		{
			panelDick[activePanelID].gameObject.SetActive (true);
		}
	}

	public static void Disable () 
	{
		GameManager.HumanPlayer.userInput.enabled = true;
		mainCamGameObject.SetActive (true);
		GameManager.Hud.SetActive (true);
		if (activePanelID != PanelButtonType.PopulationsButton) 
		{
			panelDick[activePanelID].gameObject.SetActive (false);
		}
		thisCamera.gameObject.SetActive (false);
	}
}
