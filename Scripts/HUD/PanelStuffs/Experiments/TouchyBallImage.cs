using UnityEngine;
using System.Collections;

public class TouchyBallImage : MonoBehaviour 
{
	public UnityEngine.UI.Image image;

	public void Initiate () 
	{
		image = GetComponent<UnityEngine.UI.Image> ();
		gameObject.SetActive (false);
	}
}
