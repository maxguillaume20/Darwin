using UnityEngine;
using System.Collections;

public class FlowBox : LittleBox
{
	public bool hasSavedInfo;
	public int savedPathPosition;
	public Color savedPathColor;
	public Sprite savedSprite;
	public float savedRotation;

	public override void Unoccupy ()
	{
		base.Unoccupy ();
		if (hasSavedInfo) 
		{
			occupied = true;
			pathColor = savedPathColor;
			pathPosition = savedPathPosition;
			SetBackGround (savedPathColor);
			if (FlowPuzzle.shortenPathsDick[savedPathColor].Contains (this))
			{
				if (FlowPuzzle.shortenPathsDick[savedPathColor].Count == 1) 
				{
					PathPuzzle flowPuzzle = ExperimentPanel.activePuzzle as PathPuzzle;
					foreach (FlowBox flowBox in flowPuzzle.pathDick[savedPathColor]) 
					{
						flowBox.RestoreSavedInfo();
					}
				}
				FlowPuzzle.shortenPathsDick[savedPathColor].Remove (this);
			}
		}
	}

	public void SetBackGround (bool on) 
	{
		if (on) 
		{
			images [0].color = new Color (pathColor.r, pathColor.g, pathColor.b, 0.3f);
		}
		else 
		{
			images[0].color = Color.clear;
		}
	}

	public void SetBackGround (Color backgroundColor) 
	{
		images [0].color = new Color (backgroundColor.r, backgroundColor.g, backgroundColor.b, 0.3f);
	}

	public void SavePathInfo () 
	{
		if (!hasSavedInfo) 
		{
			hasSavedInfo = true;
			savedPathPosition = pathPosition;
			savedPathColor = pathColor;
			savedSprite = images [1].sprite;
			savedRotation = images [1].transform.localEulerAngles.z;
		}
	}

	public void RestoreSavedInfo ()
	{
		if (hasSavedInfo) 
		{
			pathPosition = savedPathPosition;
			Occupy (savedPathColor, savedSprite, (int) savedRotation);
			SetBackGround (true);
			hasSavedInfo = false;
		}
	}

	public override void Reset ()
	{
		hasSavedInfo = false;
		base.Reset ();
	}
}
