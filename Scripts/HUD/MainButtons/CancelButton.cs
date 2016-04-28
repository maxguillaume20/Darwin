using UnityEngine;
using System.Collections;
using RTS;

public class CancelButton : MonoBehaviour 
{
	private void Awake() 
	{
		GameManager.cancelButton = this;
		gameObject.SetActive (false);
	}
}
