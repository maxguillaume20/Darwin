using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathTouchyBall : TouchyBall
{
	private PathPuzzle pathPuzzle;
	private static Dictionary <string, int[]> quadStuffDick { get; set; }
	private static Dictionary<int, string[]> quadrantsDick { get; set; }
	private string currentQuadrant = "";
	private int cornerCount;
	private bool clockWise;

	protected override void Awake ()
	{
		base.Awake ();
		quadrantsDick = new Dictionary<int, string[]>();
		quadStuffDick = new Dictionary<string, int[]> {{"Right", new int[] {1, 1}}, {"Bottom", new int[] {-1, 1}}, {"Left", new int[] {-1, -1}}, {"Top", new int[] {1, -1}}}; 
	}

	public override void SetActivePuzzle ()
	{
		base.SetActivePuzzle ();
		pathPuzzle = expPuzzle as PathPuzzle;
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		ExperimentPanel.tbImage.gameObject.SetActive (true);
		if (pathPuzzle.activePath) 
		{
			ExperimentPanel.tbImage.image.color = PathPuzzle.activePathColor;
		}
		else ExperimentPanel.tbImage.image.color = inactiveColor;
	}

	protected override void OnDisable() 
	{
		base.OnDisable ();
		pathPuzzle.StopTouching ();
	}

	public static Dictionary<int, string[]> GetQuadrantsDick () 
	{
		// the first quadrant (key) doesn't have an associated corner
		quadrantsDick.Remove (0);
		return quadrantsDick;
	}

	public override void TouchBox (LittleBox newLittleBox)
	{
		if (newLittleBox.pathColor == PathPuzzle.activePathColor) 
		{
			ExperimentPanel.tbImage.image.color = new Color (PathPuzzle.activePathColor.r, PathPuzzle.activePathColor.g, PathPuzzle.activePathColor.b, 0.3f);
		}
		else if (newLittleBox.isEndPoint)
		{
			ExperimentPanel.tbImage.image.color = inactiveColor;
		}
	}

	public override void OutOfBounds () 
	{
		if (!outOfBounds) 
		{
			exitLocation = rectTransform.anchoredPosition;
			outOfBounds = true;
		}
	}

	public override void BackInBounds ()
	{
		base.BackInBounds ();
		cornerCount = 0;
		currentQuadrant = "";
		quadrantsDick.Clear ();
	}

	public override void Move ()
	{
		base.Move ();
		if (outOfBounds) 
		{
			// Figure out which quadrant it's in
			string newQuadrant = "";
			float currentSlope = (rectTransform.anchoredPosition.y - Screen.height / 2f) / (rectTransform.anchoredPosition.x - Screen.width / 2f);
			if (currentSlope > -1f && currentSlope <= 1f) 
			{
				if (rectTransform.anchoredPosition.x > Screen.width / 2f) 
				{
					newQuadrant = "Right";
				}
				else newQuadrant = "Left";
			} 
			else if (rectTransform.anchoredPosition.y > Screen.height / 2f) 
			{
				newQuadrant = "Top";
			}
			else newQuadrant = "Bottom";
			// Store path
			if (currentQuadrant != newQuadrant) 
			{
				if (cornerCount < 5 && (currentQuadrant == "" || quadrantsDick[cornerCount - 1][1] != newQuadrant)) 
				{
					quadrantsDick.Add (cornerCount, new string[] {newQuadrant, currentQuadrant});
					if (quadrantsDick.Count == 2) 
					{
						if (quadStuffDick[newQuadrant][1] == quadStuffDick[currentQuadrant][0]) 
						{
							clockWise = true;
						}
						else clockWise = false;
					}
					cornerCount ++;
				}
				// if the user goes back to a previous quadrant
				else if (quadrantsDick[cornerCount - 1][1] == newQuadrant)
				{
					quadrantsDick.Remove (cornerCount - 1);
					cornerCount --;
				}
				currentQuadrant = newQuadrant;
			}
			// Delete path if the user goes all the way around the matrix while out of bounds
			if (cornerCount == 5) 
			{
				float crossingPosition = exitLocation.x;
				float currentPosition = rectTransform.anchoredPosition.x;
				if (quadrantsDick[0][0] == "Left" || quadrantsDick[0][0] == "Right") 
				{
					crossingPosition = exitLocation.y;
					currentPosition = rectTransform.anchoredPosition.y;
				}
				if (clockWise && (quadStuffDick[currentQuadrant][1] == -1 && currentPosition > crossingPosition || quadStuffDick[currentQuadrant][1] == 1 && currentPosition < crossingPosition) || !clockWise && (quadStuffDick[currentQuadrant][1] == -1 && currentPosition < crossingPosition || quadStuffDick[currentQuadrant][1] == 1 && currentPosition > crossingPosition)) 
				{
					quadrantsDick.Clear();
					quadrantsDick.Add (0, new string[] {currentQuadrant, ""});
					cornerCount = 1;
				}
			}
		}
	}
}
