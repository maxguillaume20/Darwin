using UnityEngine;
using System.Collections;
using RTS;

public class CheckButton : MonoBehaviour 
{
	private void Awake() 
	{
		GameManager.checkButton = this;
		gameObject.SetActive (false);
	}
}
