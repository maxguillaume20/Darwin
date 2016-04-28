using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class ZoomButton : MainButton 
{
	public Image icon;
	private bool zooming;
	private bool startedZooming;
	// zoomspeed has no noticeable effect..
	public float zoomSpeed = 0.1f;

	protected override void Start ()
	{
		base.Start ();
		icon = GetComponent<Image> ();
		button = GetComponent<Button> ();
	}

	public void StartZoom()
	{
		GameManager.HumanPlayer.userInput.enabled = false;
		icon.color = Color.red;
		button.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.Off);
		button.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
		StartCoroutine (ZoomCoroutine ());
	}

	public void EndZoom() 
	{
		GameManager.HumanPlayer.userInput.enabled = true;
		icon.color = Color.white;
		button.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.Off);
		button.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
	}

	private IEnumerator ZoomCoroutine() 
	{
		while (!GameManager.HumanPlayer.userInput.enabled && Input.touchCount != 2) 
		{
			yield return null;
		}
		while (Input.touchCount == 2) 
		{
			ZoomCamera(OrderLRtouches(Input.GetTouch(0), Input.GetTouch(1)));
			yield return null;
		}
		if(!GameManager.HumanPlayer.userInput.enabled) 
		{
			EndZoom();
		} 
	}
	
	private void ZoomCamera (Touch[] touches)
	{
		if (touches[0].deltaPosition.x > 0 && touches[1].deltaPosition.x < 0 && Camera.main.orthographicSize < GameManager.MaxZoom)
		{
			Camera.main.orthographicSize += zoomSpeed * Mathf.Min(Mathf.Abs(touches[0].deltaPosition.x), Mathf.Abs(touches[1].deltaPosition.x));
		}
		else if (touches[0].deltaPosition.x < 0 && touches[1].deltaPosition.x > 0 && Camera.main.orthographicSize > GameManager.MinZoom)
		{
			Camera.main.orthographicSize -= zoomSpeed * Mathf.Min(Mathf.Abs(touches[0].deltaPosition.x), Mathf.Abs(touches[1].deltaPosition.x));
		}
	}

	private Touch[] OrderLRtouches(Touch first, Touch second)
	{
		Touch[] touches = new Touch[2];
		if (first.position.x < second.position.x) 
		{
			touches[0] = first; 
			touches[1] = second;
		}
		else 
		{
			touches[1] = first; 
			touches[0] = second;
		}
		return touches;
	}
}
