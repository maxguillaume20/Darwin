using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class MainButton : MonoBehaviour {
	
	public Button button;

	protected virtual void Awake() 
	{
		button = GetComponent<Button> ();
	}

	protected virtual void Start()
	{

	}
}
