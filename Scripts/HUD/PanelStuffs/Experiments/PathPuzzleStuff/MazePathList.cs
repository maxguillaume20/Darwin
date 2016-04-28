using UnityEngine;
using System.Collections;

public class MazePathList : PathList
{
	public override void SelectEndPoint (LittleBox newActivatedEndPoint)
	{
		isComplete = false;
		activatedEndPoint = newActivatedEndPoint;
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

	public override void ExtendPath (LittleBox newLittleBox)
	{
		if (!isComplete) 
		{
			if (newLittleBox == MazePuzzle.finishEndPoint) 
			{
				CompletePath(newLittleBox);
			}
			else 
			{
				int xNewDiff = newLittleBox.position [0] - this[Count - 1].position [0];
				int yNewDiff = newLittleBox.position [1] - this[Count - 1].position [1];
				if (Count > 1) 
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

	protected override void CompletePath (LittleBox otherEndPoint)
	{
		isComplete = true;
		int xNewDiff = otherEndPoint.position[0] - this[Count - 1].position[0];
		int yNewDiff = otherEndPoint.position[1] - this[Count - 1].position[1];
		ChangeLastPathEndSprite (xNewDiff, yNewDiff);
		otherEndPoint.pathPosition = Count;
		Add (otherEndPoint);
		ExperimentPanel.activePuzzle.CheckIfPuzzleIsComplete ();

	}
}
