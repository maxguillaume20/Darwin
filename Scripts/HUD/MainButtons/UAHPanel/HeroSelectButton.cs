using UnityEngine;
using System.Collections;
using RTS;

public class HeroSelectButton : MonoBehaviour
{
	private void Awake() 
	{
		GameManager.heroSelectButton = this;
	}

	private void Start ()
	{
		gameObject.SetActive (false);
	}

	public void SelectHero() 
	{
//		GameManager.HumanPlayer.userInput.SelectCurrentHero ();
	}
}
