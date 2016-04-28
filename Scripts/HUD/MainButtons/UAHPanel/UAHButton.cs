using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UAHButton : MonoBehaviour 
{
	public Button button;
	public Image image;
	public Text texican;

	protected virtual void Awake() 
	{
		button = GetComponent<Button> ();
		image = GetComponent<Image> ();
		texican = GetComponentInChildren<Text> ();
	}	
}
