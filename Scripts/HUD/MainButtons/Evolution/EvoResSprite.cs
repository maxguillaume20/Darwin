using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class EvoResSprite : MonoBehaviour 
{

	void Awake() 
	{
		GetComponent<Image> ().sprite = HUD.speciesResourceSpriteDick [GameManager.HumanPlayer.species] [ResourceType.Unique];
	}
}
