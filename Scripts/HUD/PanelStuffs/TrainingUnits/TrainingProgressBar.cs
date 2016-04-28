using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class TrainingProgressBar : MonoBehaviour 
{
	private static GameObject thisGameObject { get; set; }
	public static Image backgroundImage { get; set; }
	public static Image progressImage { get; set; }
	public static bool isActive { get; set; }
	public static Text countText;

	private void Awake () 
	{
		thisGameObject = gameObject;
		Image[] images = GetComponentsInChildren<Image> ();
		backgroundImage = images [0];
		progressImage = images [1];
		countText = GetComponentInChildren<Text> ();
		gameObject.SetActive (false);
	}

	public static void OpenBar (PanelButtonType buttonID) 
	{
		isActive = true;
		thisGameObject.SetActive (true);
		countText.gameObject.SetActive (true);
		countText.text = "";
		progressImage.color = GameManager.HumanPlayer.color;
		if (buttonID == PanelButtonType.UnitsMenu) 
		{
			backgroundImage.rectTransform.anchorMin = new Vector2 (0.35f, 0.74f);
			backgroundImage.rectTransform.anchorMax = new Vector2 (0.65f, 0.76f);
		}
		else if (buttonID == PanelButtonType.LocalUpgradesButton) 
		{
			backgroundImage.rectTransform.anchorMin = new Vector2 (0.15f, 0.6f);
			backgroundImage.rectTransform.anchorMax = new Vector2 (0.85f, 0.65f);
			countText.gameObject.SetActive (false);
		}
		else 
		{
			backgroundImage.rectTransform.anchorMin = new Vector2 (0.35f, 0.65f);
			backgroundImage.rectTransform.anchorMax = new Vector2 (0.65f, 0.67f);
		}
		progressImage.rectTransform.anchorMax = new Vector2 (0f, 1f);
		float countTextWidth = 0.1f * GameManager.panelWidth * Screen.width;
		float countTextHeight = 0.06f * Screen.height;
		countText.rectTransform.sizeDelta = new Vector2 (countTextWidth, countTextHeight);
		float xTextPos = Screen.width * GameManager.panelWidth * (backgroundImage.rectTransform.anchorMax.x - backgroundImage.rectTransform.anchorMin.x) + countTextWidth;
		float yTextPos = Screen.height * ((backgroundImage.rectTransform.anchorMax.y - backgroundImage.rectTransform.anchorMin.y) / 2f);
		countText.rectTransform.anchoredPosition = new Vector2 (xTextPos, yTextPos);
	}

	public static void CloseBar () 
	{
		isActive = false;
		countText.gameObject.SetActive (false);
		thisGameObject.SetActive (false);
	}

	public static void ChangeProgress (float percentage) 
	{
		progressImage.rectTransform.anchorMax = new Vector2 (percentage, 1f);
	}
}
