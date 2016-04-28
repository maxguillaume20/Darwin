using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class MonumentSelectionPanel : MonoBehaviour 
{
	public MonumentSelectionMenu monSelectionMenu;
	private RectTransform rectTransform;
	public Button[] selectionButtons;
	public Image popImage;
	public Text popText;
	// [0] = UnlockedText, [1] = EmptyUnlockedText, [2] = LockedText
	public Text[] otherTexts;
	private float panelHeight = 0.6f;
	private float lockedPopCost;

	public void InitializePanel() 
	{
		rectTransform = GetComponent<RectTransform> ();
		rectTransform.sizeDelta = new Vector2 (Screen.width * (MainPanel.rectTransform.anchorMax.x - MainPanel.rectTransform.anchorMin.x), Screen.height * panelHeight);
		otherTexts [0].rectTransform.anchorMin = new Vector2 (0f, 0.9f);
		otherTexts [0].rectTransform.anchorMax = new Vector2 (1f, 1f);
		popText.text = "-" + ((int)monSelectionMenu.lockedPopCost).ToString ();
	}

	public void OpenPanel (UnoccupiedMonument newMonument) 
	{
		if (newMonument.GetPlayer().unlockedMonumentsDick[newMonument.era].Count < 2) 
		{
			popImage.gameObject.SetActive (true);
			popText.gameObject.SetActive (true);
			popImage.sprite = HUD.speciesPopSpriteDick[newMonument.GetSpecies ()];
			otherTexts [2].rectTransform.anchorMin = new Vector2 (0f, 0.55f);
			otherTexts [2].rectTransform.anchorMax = new Vector2 (1f, 0.65f);
			if (newMonument.GetPlayer().unlockedMonumentsDick[newMonument.era].Count == 0) 
			{
				otherTexts [1].rectTransform.anchorMin = new Vector2 (0f, 0.65f);
				otherTexts [1].rectTransform.anchorMax = new Vector2 (1f, 0.9f);
				for (int i = 0; i < selectionButtons.Length; i ++) 
				{
					selectionButtons[i].GetComponent<RectTransform>().anchorMin = new Vector2 (0.15f, 0.25f - i * 0.2f);
					selectionButtons[i].GetComponent<RectTransform>().anchorMax = new Vector2 (0.85f, 0.4f - i * 0.2f);
					AddSelectMonumentListener (newMonument, selectionButtons[i], monSelectionMenu.lockedPopCost);
				}
			}
			else 
			{
				otherTexts[1].gameObject.SetActive(false);
				for (int i = 0; i < selectionButtons.Length; i ++) 
				{
					SpecType monumentSpec = MonumentSelectionMenu.speciesMonumentTypeDick[newMonument.GetSpecies()][newMonument.era][selectionButtons[i].GetComponentInChildren<Text>().text];
					if (newMonument.GetPlayer().unlockedMonumentsDick[newMonument.era].Contains(monumentSpec))
					{
						selectionButtons[i].GetComponent<RectTransform>().anchorMin = new Vector2 (0.15f, 0.7f);
						selectionButtons[i].GetComponent<RectTransform>().anchorMax = new Vector2 (0.85f, 0.85f);
						AddSelectMonumentListener (newMonument, selectionButtons[i], 0);
					}
					else 
					{
						selectionButtons[i].GetComponent<RectTransform>().anchorMin = new Vector2 (0.15f, 0.25f);
						selectionButtons[i].GetComponent<RectTransform>().anchorMax = new Vector2 (0.85f, 0.4f);
						AddSelectMonumentListener (newMonument, selectionButtons[i], monSelectionMenu.lockedPopCost);
					}
				}
			}
		}
		else 
		{
			popImage.gameObject.SetActive (false);
			popText.gameObject.SetActive (false);
			for (int i = 0; i < selectionButtons.Length; i ++) 
			{
				selectionButtons[i].GetComponent<RectTransform>().anchorMin = new Vector2 (0.15f, 0.7f - i * 0.2f);
				selectionButtons[i].GetComponent<RectTransform>().anchorMax = new Vector2 (0.85f, 0.85f - i * 0.2f);
				AddSelectMonumentListener (newMonument, selectionButtons[i], 0);
			}
			otherTexts[2].rectTransform.anchorMax = new Vector2 (1f, 0.4f);
			otherTexts[2].rectTransform.anchorMin = new Vector2 (0f, 0.3f);
			otherTexts[1].text = "None";
			otherTexts[1].rectTransform.anchorMax = new Vector2 (1f, 0.3f);
			otherTexts[1].rectTransform.anchorMin = new Vector2 (0f, 0.1f);
		}
	}

	private void AddSelectMonumentListener (UnoccupiedMonument newMonument, Button selectionButton, int popCost) 
	{
		SpecType monumentSpec = MonumentSelectionMenu.speciesMonumentTypeDick[newMonument.GetSpecies()][newMonument.era][selectionButton.GetComponentInChildren<Text>().text];
		selectionButton.onClick.AddListener (delegate { monSelectionMenu.SelectMonument (selectionButton, monumentSpec, popCost); });
	}

	private void OnDisable () 
	{
		foreach (Button butt in selectionButtons) 
		{
			butt.onClick.RemoveAllListeners ();
		}
	}
}
