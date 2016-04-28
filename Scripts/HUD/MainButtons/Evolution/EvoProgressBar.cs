using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EvoProgressBar : MonoBehaviour {

	public Image image;
	public RectTransform rectTransform;

	private void Awake() 
	{
		image = GetComponent<Image> ();
		rectTransform = GetComponent<RectTransform> ();
	}
}
