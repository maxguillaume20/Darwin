using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PuzzleWall : MonoBehaviour 
{
	public Image image;
	public RectTransform rectTransform;
	public static float verticalWidth { get { return 0.1f; } }
	public static float horizontalWidth { get { return 0.1f; } }

	public void Initiate () 
	{
		image = GetComponent<Image> ();
		rectTransform = GetComponent<RectTransform> ();
	}
}
