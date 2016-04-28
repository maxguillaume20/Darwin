using UnityEngine;
using System.Collections;
using RTS;

public class HeroUnit : Unit 
{
//	public Sprite[] uahButtonSprites;
//	protected UnityEngine.Events.UnityAction[] methodArray = new UnityEngine.Events.UnityAction[3];
//
//	protected override void Start() 
//	{
//		base.Start ();
//		player.currentHero = this;
//	}
//
//	public override void SelectTap (Player controller)
//	{
//		base.SelectTap(controller);
//		GameManager.uahMiniPanel.gameObject.SetActive (true);
//		GameManager.heroSelectButton.gameObject.SetActive (false);
//		// switches the uahMiniPanel buttons to the appropriate Hero's abilities, if necessary
//		if (GameManager.uahMiniPanel.lastHero != this) 
//		{
//			for (int i = 0; i < uahButtonSprites.Length; i ++) 
//			{
//				GameManager.uahMiniPanel.uahButtons[i].button.onClick.RemoveAllListeners();
//				GameManager.uahMiniPanel.uahButtons[i].image.sprite = uahButtonSprites[i];
//				GameManager.uahMiniPanel.uahButtons[i].button.onClick.AddListener(methodArray[i]);
//			}
//			GameManager.uahMiniPanel.lastHero = this;
//		}
//	}
//
//	public override void Deselect ()
//	{
//		base.Deselect ();
//		GameManager.uahMiniPanel.gameObject.SetActive (false);
//		GameManager.heroSelectButton.gameObject.SetActive (true);
//	}
//
//	public override void Die ()
//	{
//		base.Die ();
//		GameManager.heroSelectButton.gameObject.SetActive (false);
//	}
}
