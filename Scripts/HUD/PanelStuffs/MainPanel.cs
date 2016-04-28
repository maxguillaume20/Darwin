using UnityEngine;
using UnityEngine.UI;


public class MainPanel : MonoBehaviour {

	public GameObject[] MPthings;
	public  static RectTransform rectTransform { get; set; }

	private void Awake() 
	{
		rectTransform = GetComponent<RectTransform> ();
	}
}
