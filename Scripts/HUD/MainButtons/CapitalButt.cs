using UnityEngine;
using System.Collections;
using RTS;

public class CapitalButt : MainButton 
{
	
	
	public void SelectCapital()
	{
		GameManager.HumanPlayer.userInput.SelectCapital ();
	}

}
