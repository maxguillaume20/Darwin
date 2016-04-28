using UnityEngine;
using System.Collections;
using RTS;

public class AI : MonoBehaviour 
{
	private AIManager[] managersArray = new AIManager[] {new StrategyManager (), new ResourceManager(), new ConstructionManger ()}; 
	private Player player;
	// Use this for initialization
	void Awake () 
	{
		player = gameObject.GetComponent<Player> ();
		if (player == GameManager.HumanPlayer) 
		{
			enabled = false;
		}
	}

	void Update () 
	{
		foreach (AIManager manager in managersArray) 
		{

		}
	}
}
