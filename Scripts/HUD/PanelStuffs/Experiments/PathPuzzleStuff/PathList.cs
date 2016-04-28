using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathList : List<LittleBox>
{
	public Color pathColor;
	public bool isComplete;
	public bool firstActivation = true;
	public LittleBox activatedEndPoint;

	public virtual void SelectEndPoint (LittleBox newActivatedEndPoint) 
	{
		isComplete = false;
		activatedEndPoint = newActivatedEndPoint;
		activatedEndPoint.Occupy (pathColor, ExperimentPanel.puzzleSprites [0], 0);
		activatedEndPoint.pathPosition = 0;
		foreach (LittleBox littleBox in this) 
		{
			if (littleBox != activatedEndPoint && littleBox.pathColor == pathColor) 
			{
				littleBox.Unoccupy ();
			}
		}
		Clear ();
		Add (activatedEndPoint);
	}

	public void MaybeShortenPath (LittleBox newLittleBox) 
	{ 
		if (isComplete) 
		{
			ShortenCompletePath (newLittleBox);
		}
		// If not the current pathEnd
		else if (newLittleBox.pathPosition < Count - 1) 
		{
			ShortenInCompletePath (newLittleBox);
		}
	}

	public void ShortenInCompletePath (LittleBox newPathEnd) 
	{
		for (int i = newPathEnd.pathPosition + 1; i < Count; i++) 
		{
			if (this[i].pathColor == pathColor) 
			{
				this[i].Unoccupy();
			}
		}
		RemoveRange(newPathEnd.pathPosition + 1, Count - 1 - newPathEnd.pathPosition);
		int xNewDiff = this[Count - 1].position[0] - this[Count - 2].position [0];
		int yNewDiff = this[Count - 1].position[1] - this[Count - 2].position [1];
		int rotation = -90 * xNewDiff + 90 * (Mathf.Abs(yNewDiff) - 1 * yNewDiff);
		this[Count - 1].SetSprite (pathColor, ExperimentPanel.puzzleSprites[3], rotation);
	}

	private void ShortenCompletePath (LittleBox newPathEnd) 
	{
		isComplete = false;
		if ((float)newPathEnd.pathPosition / (float) (Count - 1) >= 0.5f) 
		{
			if (newPathEnd.pathPosition < Count - 1) 
			{
				ShortenInCompletePath (newPathEnd);
			}
			else 
			{
				int xNewDiff = this[Count - 1].position[0] - this[Count - 2].position [0];
				int yNewDiff = this[Count - 1].position[1] - this[Count - 2].position [1];
				int rotation = -90 * xNewDiff + 90 * (Mathf.Abs(yNewDiff) - 1 * yNewDiff);
				this[Count - 1].SetSprite (pathColor, ExperimentPanel.puzzleSprites[3], rotation);
			}
		}
		// Changes direction of path
		else 
		{
			List<LittleBox> lbList = new List<LittleBox>();
			for (int i = Count - 1; i >= 0; i --) 
			{
				if (this[i].pathPosition >= newPathEnd.pathPosition) 
				{
					lbList.Add (this[i]);
				}
				else if (this[i].pathColor == pathColor)
				{
					this[i].Unoccupy ();
				}
			}
			Clear ();
			SelectEndPoint (lbList[0]);
			for (int i = 1; i < lbList.Count; i ++) 
			{
				ExtendPath (lbList[i]);
			}
		}
	}

	public void ConnectNeighbors (bool connect, LittleBox lb1, LittleBox lb2) 
	{
		// 0 for vertical movement
		int pos = 0;
		// 1 for horizantal movement
		if (lb2.position[1] - lb1.position[1] != 0) 
		{
			pos = 1;
		}
		if (lb2.position[pos] >= lb1.position[pos]) 
		{
			lb2.puzzleWalls[pos].image.enabled = !connect;
		}
		else 
		{
			lb1.puzzleWalls[pos].image.enabled = !connect;
		}
	}

	public virtual void ExtendPath (LittleBox newLittleBox) 
	{
		if (!isComplete) 
		{
			if (newLittleBox.isEndPoint && newLittleBox != activatedEndPoint) 
			{
				CompletePath(newLittleBox);
			}
			else 
			{
				int xNewDiff = newLittleBox.position [0] - this[Count - 1].position [0];
				int yNewDiff = newLittleBox.position [1] - this[Count - 1].position [1];
				if (Count == 1) 
				{
					int endPointRotation = 90 * -xNewDiff + (90 * (1 - yNewDiff)) * Mathf.Abs (yNewDiff); 
					this[0].SetSprite (pathColor, ExperimentPanel.puzzleSprites[4], endPointRotation);
				}
				else 
				{
					ChangeLastPathEndSprite (xNewDiff, yNewDiff);
				}
				int newRotation = -90 * xNewDiff + 90 * (Mathf.Abs(yNewDiff) - 1 * yNewDiff);
				newLittleBox.Occupy (pathColor, ExperimentPanel.puzzleSprites[3], newRotation);
				newLittleBox.pathPosition = Count;
				Add (newLittleBox);
			}
		}
	}

	protected virtual void CompletePath (LittleBox otherEndPoint) 
	{
		isComplete = true;
		int xNewDiff = otherEndPoint.position[0] - this[Count - 1].position[0];
		int yNewDiff = otherEndPoint.position[1] - this[Count - 1].position[1];
		ChangeLastPathEndSprite (xNewDiff, yNewDiff);
		int zEndRotation = 90 * xNewDiff + (90 * (1 + yNewDiff)) * Mathf.Abs (yNewDiff); 
		otherEndPoint.Occupy (pathColor, ExperimentPanel.puzzleSprites[4], zEndRotation);
		otherEndPoint.pathPosition = Count;
		Add (otherEndPoint);
		PathPuzzle.completedPath = true;
	}

	protected void ChangeLastPathEndSprite (int xNewDiff, int yNewDiff) 
	{
		LittleBox lastPathEndBox = this[Count - 1];
		int xOldDiff = lastPathEndBox.position[0] - this[Count - 2].position[0];
		int yOldDiff = lastPathEndBox.position[1] - this[Count - 2].position[1];
		// if it's a turn
		if (xNewDiff != xOldDiff || yNewDiff != yOldDiff) 
		{
			int zRotation = 0; 
			// if the rotation should not be 0
			if (xNewDiff + yOldDiff < 2 && yNewDiff + xOldDiff > -2) 
			{
				zRotation = 180 - (45 * xNewDiff - 45 * yOldDiff + 45 * yNewDiff - 45 * xOldDiff);
			}
			lastPathEndBox.SetSprite (pathColor, ExperimentPanel.puzzleSprites[2], zRotation);
		}
		// if its a straight line
		else 
		{
			lastPathEndBox.SetSprite (pathColor, ExperimentPanel.puzzleSprites[1], 90 * xNewDiff);
		}
	}
}
