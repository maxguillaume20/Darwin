using UnityEngine;
using System.Collections.Generic;
using RTS;

public class BackButton : PanelButton 
{
	public static List<GameObject> gameObjectsToClose { get; set; }
	public static List<GameObject> gameObjectsToOpen { get; set; }

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = PanelButtonType.BackButton;
		gameObjectsToClose = new List<GameObject> ();
		gameObjectsToOpen = new List<GameObject> ();
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();
		foreach(GameObject go in gameObjectsToOpen) 
		{
			go.SetActive(true);
		}
		gameObjectsToOpen.Clear ();
		foreach(GameObject go in gameObjectsToClose) 
		{
			go.SetActive(false);
		}
		gameObjectsToClose.Clear ();
	}

	public static void SetGameObjectsToOpen(GameObject[] gameObjects) 
	{
		for (int i = 0; i < gameObjects.Length; i ++) 
		{
			gameObjectsToOpen.Add(gameObjects[i]);
		}
	}

	public static void SetGameObjectsToClose(GameObject[] gameObjects) 
	{
		for (int i = 0; i < gameObjects.Length; i ++) 
		{
			gameObjectsToClose.Add(gameObjects[i]);
		}
	}

	public void GoBack()
	{
		List<PanelButtonType> woButtons = wo.GetButtons ();
		for (int i = 0; i < woButtons.Count; i ++) 
		{
			GameManager.Hud.OpenPanelButton(woButtons[i]);
		}
		gameObject.SetActive (false);
	}
}
