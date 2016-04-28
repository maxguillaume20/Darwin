using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class UAHPanel : MonoBehaviour 
{
//	private static UAHMainButton uahMainButton { get; set; }
//	private static MenuButton menuButton { get; set; }
//	public UAHPanelButton[] uahPanButts;
//
//	void Awake() 
//	{
//		GameManager.uahPanel = this;
//		uahPanButts = GetComponentsInChildren<UAHPanelButton> ();
//	}
//
//	void Start() 
//	{
//		gameObject.SetActive (false);
//	}
//
//	private void OnEnable() 
//	{
//		if (menuButton) menuButton.gameObject.SetActive (false);
//	}
//
//	private void OnDisable() 
//	{
//		if (uahMainButton) uahMainButton.gameObject.SetActive (true);
//		if (menuButton) menuButton.gameObject.SetActive (true);
//	}
//
//	public void CloseUAHPanel()
//	{
//		gameObject.SetActive (false);
//	}
//
//	public void NewEra (Eras newEra) 
//	{
//		foreach (UAHPanelButton butt in uahPanButts) 
//		{
//			if (butt.GetEra() == newEra) 
//			{
//				butt.GetComponent<Button>().interactable = true;
//			}
//		}
//	}
//
//	public static void SetUAHMainButton (UAHMainButton caacaa) 
//	{
//		uahMainButton = caacaa;
//	}
//
//	public static void SetMenuButton (MenuButton peepee) 
//	{
//		menuButton = peepee;
//	}
}
