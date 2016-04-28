using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestFlowPuzzle : PathPuzzle
{
	public GameObject obstaclesButton;
	private LittleBox originLB;
	private LittleBox destinationLB;
	private int matrixSize = 10;
	private bool settingObstacles;
	private bool settingPaths;
	private Dictionary<Color, PathList> pathEndDick = new Dictionary<Color, PathList>();
	public Color newPathColor;

	public override void Initiate ()
	{
		rowCount = matrixSize;
		columnCount = matrixSize;
		base.Initiate ();
	}

	public override void Enable ()
	{
		base.Enable ();
		obstaclesButton.gameObject.SetActive (true);
		expPanel.StartMoveTouchyBall ();
		for (int i = 0; i < matrixSize; i ++) 
		{
			for (int j = 0; j < matrixSize; j ++) 
			{
				littleBoxMatrix[i][j].images[0].color = Color.clear;
			}
		}
	}

	protected override IEnumerator PuzzleGenerator ()
	{
		yield return null;
	}

	public void SetPaths () 
	{
		settingPaths = !settingPaths;
		if (settingPaths) 
		{
			foreach (Color pathColor in pathDick.Keys) 
			{
				foreach (LittleBox littleBox in pathDick[pathColor]) 
				{
					littleBox.Unoccupy();
				}
			}
			pathDick.Clear ();
			foreach (Color pathColor in pathEndDick.Keys) 
			{
				foreach (LittleBox littleBox in pathEndDick[pathColor]) 
				{
					littleBox.isEndPoint = false;
					littleBox.Unoccupy();
				}
			}
			pathEndDick.Clear ();
		}
	}

	public override void DoAction (LittleBox newLittleBox)
	{
		if (settingPaths) 
		{
			if (!pathDick.ContainsKey (newPathColor)) 
			{
				pathEndDick.Add (newPathColor, new PathList());
				FlowPuzzle.shortenPathsDick.Add (newPathColor, new List<FlowBox>());
				pathDick.Add (newPathColor, new PathList ());
				pathDick[newPathColor].pathColor = newPathColor;
			}
			if (pathEndDick[newPathColor].Count < 2) 
			{
				newLittleBox.isEndPoint = true;
				newLittleBox.Occupy (newPathColor, ExperimentPanel.puzzleSprites[0], 0);
				newLittleBox.images[0].color = Color.clear;
				pathEndDick[newPathColor].Add (newLittleBox);
			}
			else print ("Pick a new path color");
		}
		else 
		{
			base.DoAction (newLittleBox);
		}
	}

	protected override void NewActivePath (LittleBox newLittleBox)
	{
		base.NewActivePath (newLittleBox);
		foreach (FlowBox flowBox in pathDick[activePathColor]) 
		{
			flowBox.SetBackGround (false);
		}
	}
	
	protected override bool IsValidPath (List<LittleBox> newLbList)
	{
		List<FlowBox> shortenList = new List<FlowBox> ();
		for (int i = 1; i < newLbList.Count; i ++)
		{
			if (!newLbList[i].neighbors.Contains(newLbList[i - 1]) || newLbList[i].occupied && (newLbList[i].isEndPoint && (newLbList[i].pathColor != activePathColor || newLbList[i] == pathDick[activePathColor].activatedEndPoint) || !newLbList[i].isEndPoint && newLbList[i].pathColor == activePathColor))
			{
				return false;
			}
			else if (newLbList[i].occupied && !newLbList[i].isEndPoint && newLbList[i].pathColor != activePathColor) 
			{
				shortenList.Add (newLbList[i] as FlowBox);
			}
		}
		foreach (FlowBox flowBox in shortenList) 
		{
			PrepareToShortenPath (flowBox);
		}
		return true;
	}
	
	private void PrepareToShortenPath (FlowBox breakPoint) 
	{
		if (!FlowPuzzle.shortenPathsDick[breakPoint.pathColor].Contains(breakPoint)) 
		{
			if (FlowPuzzle.shortenPathsDick[breakPoint.pathColor].Count > 0 && breakPoint.pathPosition < FlowPuzzle.shortenPathsDick[breakPoint.pathColor][0].savedPathPosition) 
			{
				FlowPuzzle.shortenPathsDick[breakPoint.pathColor].Insert (0, breakPoint);
			}
			else 
			{
				FlowPuzzle.shortenPathsDick[breakPoint.pathColor].Add (breakPoint);
			}
			if (FlowPuzzle.shortenPathsDick[breakPoint.pathColor].Count == 1) 
			{
				for (int j = 0; j < pathDick[breakPoint.pathColor].Count; j++) 
				{
					FlowBox flowBox = pathDick[breakPoint.pathColor][j] as FlowBox;
					flowBox.SavePathInfo ();
					if (pathDick[breakPoint.pathColor][j].isEndPoint) 
					{
						pathDick[breakPoint.pathColor][j].SetSprite (breakPoint.pathColor, ExperimentPanel.puzzleSprites[0], 0);
					}
					else if (pathDick[breakPoint.pathColor][j].pathColor == breakPoint.pathColor)
					{
						pathDick[breakPoint.pathColor][j].images[1].enabled = false;
					}
				}
			}
		}
	}
	
	public override void StopTouching ()
	{
		foreach (Color pathColor in FlowPuzzle.shortenPathsDick.Keys) 
		{
			if (FlowPuzzle.shortenPathsDick[pathColor].Count > 0) 
			{
				BreakPath (pathColor, FlowPuzzle.shortenPathsDick[pathColor][0]);
				FlowPuzzle.shortenPathsDick[pathColor].Clear ();
			}
		}
		if (pathDick.ContainsKey (activePathColor)) 
		{
			foreach (FlowBox flowBox in pathDick[activePathColor]) 
			{
				flowBox.SetBackGround (true);
			}
		}
		base.StopTouching ();
	}
	
	private void BreakPath (Color pathColor, FlowBox breakPoint) 
	{
		pathDick[pathColor].isComplete = false;
		foreach (FlowBox flowBox in pathDick[pathColor]) 
		{
			flowBox.hasSavedInfo = false;
		}
		Color currentPathColor = breakPoint.pathColor;
		Sprite currentSprite = breakPoint.images[1].sprite;
		float spriteRotation = breakPoint.images[1].transform.eulerAngles.z;
		int currentPathPostion = breakPoint.pathPosition;
		List<LittleBox> lbList = new List<LittleBox> ();
		for (int i = 1; i < breakPoint.savedPathPosition; i ++) 
		{
			lbList.Add (pathDick[breakPoint.savedPathColor][i]);
		}
		pathDick [breakPoint.savedPathColor].SelectEndPoint (pathDick [breakPoint.savedPathColor] [0]);
		if (breakPoint.savedPathPosition - 1 > 0) 
		{
			for (int i = 0; i < lbList.Count; i ++) 
			{
				pathDick[breakPoint.savedPathColor].ExtendPath (lbList[i]);
			}
		}
		foreach (FlowBox flowBox in pathDick[breakPoint.savedPathColor]) 
		{
			flowBox.SetBackGround (true);
		}
		breakPoint.Occupy (currentPathColor, currentSprite, (int)spriteRotation);
		breakPoint.pathPosition = currentPathPostion;
	}
	
	public override void CheckIfPuzzleIsComplete ()
	{
		bool allPathsComplete = true;
		int occupiedBoxCount = 0;
		foreach (Color pathColor in pathDick.Keys) 
		{
			if (!pathDick[pathColor].isComplete) 
			{
				allPathsComplete = false;
				break;
			}
			occupiedBoxCount += pathDick[pathColor].Count;
		}
		if (allPathsComplete && occupiedBoxCount == matrixSize * matrixSize) 
		{
			print ("puzzle finished");
		}
	}
}
