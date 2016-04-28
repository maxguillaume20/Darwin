using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathPuzzle :  ExperimentPuzzle
{
	public Dictionary<Color, PathList> pathDick = new Dictionary<Color, PathList>();
	private Dictionary<string, LittleBox> cornersDick = new Dictionary<string, LittleBox> ();
	public static Color activePathColor { get; set; }
	public static bool completedPath;
	public bool activePath;
	protected LittleBox activatedEndPoint;

	public override void Enable ()
	{
		base.Enable ();
		cornersDick.Add ("LeftBottom", littleBoxMatrix [0] [0]);
		cornersDick.Add ("BottomLeft", littleBoxMatrix [0] [0]);
		cornersDick.Add ("LeftTop", littleBoxMatrix [0] [rowCount - 1]);
		cornersDick.Add ("TopLeft", littleBoxMatrix [0] [rowCount - 1]);
		cornersDick.Add ("RightTop", littleBoxMatrix [rowCount - 1] [rowCount - 1]);
		cornersDick.Add ("TopRight", littleBoxMatrix [rowCount - 1] [rowCount - 1]);
		cornersDick.Add ("RightBottom", littleBoxMatrix [rowCount - 1] [0]);
		cornersDick.Add ("BottomRight", littleBoxMatrix [rowCount - 1] [0]);
	}

	public override void Disable ()
	{
		completedPath = false;
		activePath = false;
		pathDick.Clear ();
		cornersDick.Clear ();
		base.Disable ();
	}

	public override void DoAction (LittleBox littleBox)
	{
		if (activePath) 
		{
			MaybeAdjustPath (littleBox);
		}
		else if (littleBox.occupied) 
		{
			NewActivePath (littleBox);
		}
	}

	protected virtual void NewActivePath (LittleBox newLittleBox) 
	{
		activePath = true;
		activePathColor = newLittleBox.pathColor;
		if (newLittleBox.isEndPoint) 
		{
			activatedEndPoint = newLittleBox;
			pathDick[activePathColor].SelectEndPoint (newLittleBox);
		}
		else  
		{
			pathDick[activePathColor].MaybeShortenPath (newLittleBox);
		}
	}

	public virtual void StopTouching () 
	{
		activePath = false;
		activatedEndPoint = null;
		if (completedPath) 
		{
			CheckIfPuzzleIsComplete ();
			completedPath = false;
		}
	}

	protected void MaybeAdjustPath (LittleBox newLittleBox) 
	{
		if (newLittleBox == activatedEndPoint) 
		{
			pathDick[activePathColor].SelectEndPoint (newLittleBox);
		}
		else if (newLittleBox.pathColor == activePathColor && !newLittleBox.isEndPoint) 
		{
			pathDick[activePathColor].MaybeShortenPath (newLittleBox);
		}
		else if (!pathDick[activePathColor].isComplete)
		{
			MaybeExtendPath (newLittleBox);
		}
	}

	public override void CheckIfPuzzleIsComplete () 
	{
//		activePath = false;
		bool puzzleFinished = true;
		foreach (Color pathColor in pathDick.Keys) 
		{
			if (!pathDick[pathColor].isComplete) 
			{
				puzzleFinished = false;
				break;
			}
		}
		if (puzzleFinished) 
		{
			expPanel.EndPuzzle (true);
		}
	}

	protected virtual void MaybeExtendPath (LittleBox newLittleBox) 
	{
		List<LittleBox> newLittleBoxList;
		LittleBox currentPathEnd = pathDick [activePathColor][pathDick [activePathColor].Count - 1];
		if (ExperimentPanel.activeTouchyBall.outOfBounds) 
		{
			newLittleBoxList = OutOfBoundsPath (newLittleBox, currentPathEnd);
		}
		else 
		{
			newLittleBoxList = InBoundsPath (newLittleBox, currentPathEnd);
		}
		if (IsValidPath(newLittleBoxList))
		{
			for (int i = 1; i < newLittleBoxList.Count; i ++) 
			{
				pathDick[activePathColor].ExtendPath (newLittleBoxList[i]);
			}
		}
//		else if (newLittleBox.isEndPoint && newLittleBox.pathColor == activePathColor && newLittleBox != pathDick[activePathColor].activatedEndPoint) 
//		{
//			pathDick[activePathColor].SelectEndPoint(newLittleBox);
//		}
	}

	protected virtual bool IsValidPath (List<LittleBox> newLbList) 
	{
		return false;
	}
	
	private List<LittleBox> OutOfBoundsPath (LittleBox destinationLB, LittleBox originLB) 
	{
		List<LittleBox> newLittleBoxList = new List<LittleBox> ();
		List<LittleBox> wayPointLBsList = new List<LittleBox> {originLB};
		Dictionary<int, string[]> quadsDick = PathTouchyBall.GetQuadrantsDick ();
		for (int i = 1; i <= quadsDick.Count; i ++) 
		{
			string cornerName = quadsDick[i][0] + quadsDick[i][1];
			if (cornersDick[cornerName] != originLB && cornersDick[cornerName] != destinationLB) 
			{
				wayPointLBsList.Add (cornersDick[cornerName]);
			}
		}
		wayPointLBsList.Add (destinationLB);
		for (int i = 0; i < wayPointLBsList.Count - 1; i ++) 
		{
			int axis = 1;
			if (wayPointLBsList[i + 1].position[1] == wayPointLBsList[i].position[1]) 
			{
				axis = 0;
			}
			int change = (wayPointLBsList[i + 1].position[axis] - wayPointLBsList[i].position[axis]) / Mathf.Abs (wayPointLBsList[i + 1].position[axis] - wayPointLBsList[i].position[axis]);
			for (int j = 0; Mathf.Abs (j) < Mathf.Abs (wayPointLBsList[i + 1].position[axis] - wayPointLBsList[i].position[axis]); j += change) 
			{
				int x = wayPointLBsList[i].position[0] + (1 - axis) * j;
				int y = wayPointLBsList[i].position[1] + axis * j;
				newLittleBoxList.Add (littleBoxMatrix[x][y]);
			}
		}
		newLittleBoxList.Add (destinationLB);
		return newLittleBoxList;
	}
				             
	private List<LittleBox> InBoundsPath (LittleBox destinationLB, LittleBox originLB) 
 	{
		List<LittleBox> newLittleBoxList = new List<LittleBox> ();
		float slope = (float) (destinationLB.position[1] - originLB.position[1]) / (float) (destinationLB.position[0] - originLB.position[0]);
		int xDirection = 0;
		if (destinationLB.position[0] != originLB.position[0]) 
		{
			xDirection = (destinationLB.position[0] - originLB.position[0]) / Mathf.Abs (destinationLB.position[0] - originLB.position[0]);
		}
		int yDirection = 0;
		if (destinationLB.position[1] != originLB.position[1]) 
		{
			yDirection = (destinationLB.position[1] - originLB.position[1]) / Mathf.Abs (destinationLB.position[1] - originLB.position[1]);
		}
		int Y = 0;
		for (int x = 0; Mathf.Abs (x) <= Mathf.Abs (destinationLB.position[0] - originLB.position[0]) && (newLittleBoxList.Count == 0 || newLittleBoxList[newLittleBoxList.Count - 1] != destinationLB); x += xDirection) 
		{
			newLittleBoxList.Add (littleBoxMatrix[originLB.position[0] + x][originLB.position[1] + Y]);
			if (newLittleBoxList[newLittleBoxList.Count - 1] == destinationLB) break;
			if ((Mathf.Abs (x) + 1) * Mathf.Abs (slope) >= Mathf.Abs (Y))
			{
				for (int y = Y; Mathf.Abs (y) + 1 < (Mathf.Abs (x) + 1) * Mathf.Abs (slope) && (newLittleBoxList.Count == 0 || newLittleBoxList[newLittleBoxList.Count - 1] != destinationLB); y += yDirection) 
				{
					newLittleBoxList.Add (littleBoxMatrix[originLB.position[0] + x][originLB.position[1] + y + yDirection]);
					Y += yDirection;
				}
			}
		}
		return newLittleBoxList;
	}
}
