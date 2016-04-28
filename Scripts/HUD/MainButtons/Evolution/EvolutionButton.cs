using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;
public class EvolutionButton : MainButton
{
	public EvolutionPanel evolutionPanel;
	public Image image;

	protected override void Awake() 
	{
		base.Awake ();
		image = GetComponent<Image> ();
	}

	public void OpenEvolutionPanel() 
	{
		GameManager.HumanPlayer.userInput.Deselect ();
		foreach (MainButton mb in GameManager.Hud.mainButtons) 
		{
			mb.gameObject.SetActive(false);	
		}
		evolutionPanel.gameObject.SetActive (true);
	}
}
