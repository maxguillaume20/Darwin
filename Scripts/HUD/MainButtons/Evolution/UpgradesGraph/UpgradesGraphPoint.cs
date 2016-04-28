using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradesGraphPoint : MonoBehaviour 
{

	public Image image;

	public void StartBitch () 
	{
		image = GetComponent<Image> ();
	}
}
