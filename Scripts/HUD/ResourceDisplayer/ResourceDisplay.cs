using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class ResourceDisplay : MonoBehaviour 
{
	private RectTransform thisRectTransform;
	public Image resImage;
	public Text resText;
	private RectTransform textRectTransform;
	public Vector3 worldPoint;
	public float activeTimer;

	private void Awake() 
	{
		resImage = GetComponent<Image> ();
		resText = GetComponentInChildren<Text> ();
	}


	public void Activate (Vector3 newWorldPoint, Species species, ResourceType resType, int amount)  
	{
		gameObject.SetActive (true);
		worldPoint = newWorldPoint;
		Vector3 startPoint = Camera.main.WorldToScreenPoint (worldPoint);
		startPoint.Set (startPoint.x, startPoint.y, 0f);
		transform.position = startPoint;
		resImage.sprite = HUD.speciesResourceSpriteDick [species] [resType];
		resText.text = string.Format ("+{0}", amount);
	}
}
