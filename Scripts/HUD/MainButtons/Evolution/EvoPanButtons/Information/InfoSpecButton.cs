using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class InfoSpecButton : SpecializationButton
{

	protected override void Awake ()
	{
		base.Awake ();
		era = Eras.Information;
	}

	public override void FirstSelect() 
	{
		if (GameManager.evoPanel.selectedEPB != this)
		{
			if (!GameManager.HumanPlayer.unlockedMonumentsDick[era].Contains (specTypeArray[mainArrayIndex])) 
			{
				GameManager.HumanPlayer.unlockedMonumentsDick[era].Add (specTypeArray[mainArrayIndex]);
			}
			otherSpecButton.button.interactable = false;
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener( delegate {Upgrade();});
		}
	}
}
