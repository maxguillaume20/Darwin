using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TouchyBall : MonoBehaviour 
{
	public ExperimentPuzzle expPuzzle;
	public bool outOfBounds;
	protected Vector2 exitLocation;
	protected RectTransform rectTransform;
//	public Image bigCircle;
	public static Color activeColor;
	public static Color inactiveColor { get; set; }
	private Vector2 oldPos;
	private static float lbLength;
	private static float matrixXMin;
	private static float matrixYMin;
	private static float matrixColumnCount;
	private static float matrixRowCount;

	protected virtual void Awake() 
	{
		rectTransform = GetComponent<RectTransform> ();
		inactiveColor = new Color (0f, 0f, 0f, 0.3f);
	}

//	public void Initiate () 
//	{
//		bigCircle.GetComponent<RectTransform> ().sizeDelta = new Vector2 (Screen.width * 0.15f, Screen.width * 0.15f);
//	}

	protected virtual void OnEnable() 
	{

	}

	public virtual void SetActivePuzzle () 
	{
		expPuzzle = ExperimentPanel.activePuzzle;
	}

	public virtual void Move () 
	{
		rectTransform.anchoredPosition = Input.mousePosition;
		ExperimentPanel.tbImage.image.rectTransform.anchoredPosition = Input.mousePosition;
	}

	public virtual void TouchBox (LittleBox newLittleBox) 
	{

	}

	public virtual void OutOfBounds () 
	{
		outOfBounds = true;
	}

	public virtual void BackInBounds () 
	{
		outOfBounds = false;
	}

	protected virtual void OnDisable() 
	{
		BackInBounds ();
		ExperimentPanel.tbImage.gameObject.SetActive (false);
	}
}
