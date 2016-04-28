using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FlowPuzzle : PathPuzzle 
{
	private int matrixSize;
	private int pathCount;
	private float generationTime = 3f;
//	private Dictionary<Color, PathList> pathDick = new Dictionary<Color, PathList>();
//	private Dictionary<Color, List<LittleBox>> pathDick = new Dictionary<Color, List<LittleBox>>();
	private Color[] pathColorArray = new Color[] {Color.magenta, Color.red, Color.blue, Color.green, new Color (1f, 0.5f, 0f), Color.cyan, Color.white, Color.grey, new Color (1f, 0f, 0.5f), new Color (0.5f, 1f, 0f),  new Color (0.5f, 0f, 1f), new Color (0f, 1f, 0.5f), new Color (0f, 0.5f, 1f)};
//	private Dictionary<Color, string> pathDebugDick = new Dictionary<Color, string> {{Color.magenta, "Magenta"}, {Color.red, "Red"}, {Color.blue, "blue"}, {new Color (1f, 0.5f, 0f), "Orange"}, {Color.cyan, "cyan"}, {Color.white, "white"}, {Color.grey, "grey"}};
	private List<Color> pathColorList = new List<Color>();
//	private Dictionary <Color, bool> isFinishedPathDick = new Dictionary<Color, bool>();
	private Dictionary<int, int[]> pathCountRangeDick = new Dictionary<int, int[]> {{5, new int[] {4,6}}, {6, new int[] {4,7}}, {7, new int[] {5,8}}, {8, new int[] {5,9}}, {9, new int[] {6,10}}};
	public static Dictionary<Color, List<FlowBox>> shortenPathsDick { get; set; }

	public override void Initiate ()
	{
		shortenPathsDick = new Dictionary<Color, List<FlowBox>>();
		base.Initiate ();
	}

	public override void Enable ()
	{
		matrixSize = Random.Range (5, 9);
		pathCount = Random.Range (pathCountRangeDick [matrixSize] [0], pathCountRangeDick [matrixSize] [1]);
		rowCount = matrixSize;
		columnCount = matrixSize;
		if (pathCount > pathColorArray.Length) 
		{
			pathCount = pathColorArray.Length;
		}
		if (pathCount < matrixSize / 2 + 1) 
		{
			pathCount = matrixSize / 2 + 1;
		}
		// Initiate Dicks and Lists
		List<Color> randomColorList = new List<Color> ();
		for (int i = 0; i < pathCount; i++) 
		{
			pathDick.Add (pathColorArray[i], new PathList ());
			pathDick[pathColorArray[i]].pathColor = pathColorArray[i];
			shortenPathsDick.Add (pathColorArray[i], new List<FlowBox>());
			randomColorList.Add (pathColorArray[i]);
		}
		// randomizes colors
		while (randomColorList.Count > 0) 
		{
			pathColorList.Add (randomColorList[Random.Range (0, randomColorList.Count)]);
			randomColorList.Remove (pathColorList[pathColorList.Count - 1]);
		}
		expPanel.mainTexts [0].text = "Connect the Dots";
		if (maxTime >= 10)
		{
			expPanel.mainTexts[1].text = "0:" + maxTime.ToString("n1");
		}
		else expPanel.mainTexts[1].text = "0:0" + maxTime.ToString("n1");
		base.Enable ();
	}

	public override void Disable ()
	{
		shortenPathsDick.Clear ();
		pathColorList.Clear ();
		base.Disable ();
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
		if (!shortenPathsDick[breakPoint.pathColor].Contains(breakPoint)) 
		{
			if (shortenPathsDick[breakPoint.pathColor].Count > 0 && breakPoint.pathPosition < shortenPathsDick[breakPoint.pathColor][0].savedPathPosition) 
			{
				shortenPathsDick[breakPoint.pathColor].Insert (0, breakPoint);
			}
			else 
			{
				shortenPathsDick[breakPoint.pathColor].Add (breakPoint);
			}
			if (shortenPathsDick[breakPoint.pathColor].Count == 1) 
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
		foreach (Color pathColor in shortenPathsDick.Keys) 
		{
			if (shortenPathsDick[pathColor].Count > 0) 
			{
				BreakPath (pathColor, shortenPathsDick[pathColor][0]);
				shortenPathsDick[pathColor].Clear ();
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
			expPanel.EndPuzzle (true);
		}
	}

	protected override IEnumerator PuzzleGenerator () 
	{
		// Initialize
		if (pathCount < matrixSize) 
		{
			// Starts with straight paths and then wraps the appropriate number of paths around shorten paths 
			// to fill the entire matrix
			for (int i = 0; i < pathCount; i ++) 
			{
				for (int j = 0; j < matrixSize; j ++) 
				{
					littleBoxMatrix [i] [j].images [1].enabled = true;
					littleBoxMatrix [i] [j].images [1].color = pathColorList [i];
					littleBoxMatrix [i] [j].pathColor = pathColorList [i];
 					pathDick [pathColorList [i]].Add (littleBoxMatrix [i] [j]);
				}
			}
//				 Shorten Paths
			for (int i = 0; i < matrixSize - pathCount; i ++) 
			{
				for (int j = 0; j < matrixSize - pathCount - i; j ++) 
				{
					littleBoxMatrix[pathCount - 1 - i][matrixSize - 1 - j].images[1].enabled = false;
					pathDick[pathColorList[pathCount - 1 - i]].Remove (littleBoxMatrix[pathCount - 1 - i][matrixSize - 1 - j]);
				}
			}
				// Extend appropraite paths
			for (int i = 0; i < pathCount; i ++) 
			{
				LittleBox endLittleBox = pathDick[pathColorList[i]][pathDick[pathColorList[i]].Count - 1];
				// if the box to the right of the endLittleBox is empty
				if (!littleBoxMatrix[endLittleBox.position[0] + 1][endLittleBox.position[1]].images[1].enabled) 
				{
					int x = endLittleBox.position[0] + 1;
					while (x < matrixSize && !littleBoxMatrix[x][endLittleBox.position[1]].images[1].enabled)  
					{
						littleBoxMatrix[x][endLittleBox.position[1]].images[1].enabled = true;
						littleBoxMatrix[x][endLittleBox.position[1]].pathColor = endLittleBox.pathColor;
						pathDick[endLittleBox.pathColor].Add (littleBoxMatrix[x][endLittleBox.position[1]]);
						x ++;
					}
					int y = endLittleBox.position[1] - 1;
					while (y >= 0) 
					{
						littleBoxMatrix[x - 1][y].images[1].enabled = true;
						littleBoxMatrix[x - 1][y].pathColor = endLittleBox.pathColor;
						pathDick[endLittleBox.pathColor].Add (littleBoxMatrix[x - 1][y]);
						y --;
					}
				}
			}
		}
		else 
		{
			// Starts from a corner and then snakes across the matrix
			int pathLength = matrixSize * matrixSize / pathCount;
			int pathColorIndex = -1;
			int totalCount = -1;
			for (int i = 0; i < matrixSize; i++) 
			{
				for (int j = 0; j < matrixSize; j ++) 
				{
					totalCount ++;
					int y = j;
					if (i % 2 == 1) 
					{
						y = matrixSize - 1 - j;
					}
					// if new path
					if (totalCount % pathLength == 0) 
					{
						pathColorIndex ++;
						if (pathColorIndex > pathCount - 1) 
						{
							pathColorIndex = pathCount - 1;
							pathDick [pathColorList [pathColorIndex]] [pathDick [pathColorList [pathColorIndex]].Count - 1].images [1].enabled = false;
						}
					}
					littleBoxMatrix [i] [y].pathColor = pathColorList [pathColorIndex];
					pathDick [pathColorList [pathColorIndex]].Add (littleBoxMatrix [i] [y]);
				}
			}
		}

		// Adjust Sprites for Generation
		foreach (Color pathColor in pathDick.Keys) 
		{
			for (int i = 0; i < pathDick[pathColor].Count; i ++) 
			{
				pathDick [pathColor] [i].images [0].color = Color.clear;
				pathDick [pathColor] [i].images [1].enabled = false;
				pathDick [pathColor] [i].images [1].sprite = ExperimentPanel.puzzleSprites [0];
				if (i == 0 || i == pathDick [pathColor].Count - 1) 
				{
					pathDick [pathColor] [i].images [1].enabled = true;
					pathDick [pathColor] [i].images [1].color = pathDick [pathColor] [i].pathColor;
				}
			}
		}

		// Rotate - !!Doesn't actually rotate
//		 0, 0 = up then right (0); 0, 1 = right then down (90); 1, 0 = down then left (180); 1,1 = left then up (270)
//		 axis2 then axis1
		int firstRandomIndex = Random.Range (0, 2);
		int secondRandomIndex = Random.Range (0, 2);
		if (firstRandomIndex != 0 || secondRandomIndex != 0)
		{
			List<LittleBox[]> tempLittleBoxMatrix = new List<LittleBox[]> ();
			for (int i = 0; i < matrixSize; i ++) 
			{
				tempLittleBoxMatrix.Add (new LittleBox[matrixSize]);
			}
			int axis1 = (matrixSize - 1) * Mathf.Abs(firstRandomIndex - secondRandomIndex);
			int axis1Change = -1 * Mathf.Abs(firstRandomIndex - secondRandomIndex) + 1 - Mathf.Abs(firstRandomIndex - secondRandomIndex);;
			int axis2Change = 1 - firstRandomIndex - 1 * firstRandomIndex;
			for (int i = 0; i < matrixSize; i ++) 
			{
				int axis2 = (matrixSize - 1) * firstRandomIndex;
				for (int j = 0; j < matrixSize; j ++) 
				{
					int x = axis2 * secondRandomIndex + axis1 * (1 - secondRandomIndex);
					int y = axis1 * secondRandomIndex + axis2 * (1 - secondRandomIndex);
					littleBoxMatrix[i][j].SetPosition (x, y);
					tempLittleBoxMatrix[x][y] = littleBoxMatrix[i][j];
					axis2 += axis2Change;
				}
				axis1 += axis1Change;
			}
			for (int i = 0; i < matrixSize; i ++) 
			{
				for (int j = 0; j < matrixSize; j ++) 
				{
					littleBoxMatrix[i][j] = tempLittleBoxMatrix[i][j];
				}
			}
		}

//		 Generate
		for (float time = 0f; time < generationTime; time += Time.deltaTime) 
		{
			for (int crunch = 0; crunch < 10; crunch ++) 
			{
				Color randomColor = pathColorList[Random.Range(0, pathCount)];
				int randomPathEndInt = Random.Range(0, 2) * (pathDick[randomColor].Count - 1);
				LittleBox subtractedLB = pathDick[randomColor][randomPathEndInt];
				if (pathDick[subtractedLB.pathColor].Count > 3) 
				{
					List<int> randomNeighborList = new List<int>();
					for (int i = 0; i < subtractedLB.neighbors.Count; i ++) randomNeighborList.Add (i);
					while (randomNeighborList.Count > 0) 
					{
						int randomNeighborIndex = Random.Range (0, randomNeighborList.Count);
						LittleBox randomNeighborLB = subtractedLB.neighbors [randomNeighborList[randomNeighborIndex]];
						randomNeighborList.Remove (randomNeighborList[randomNeighborIndex]);
						// if the neighbor is:
						// not in the same path that the subtractedLB was
						// an EndPoint of another path
						// the only neighbor in its respective path of the subtractedLB
						if (randomNeighborLB.pathColor != subtractedLB.pathColor && (pathDick[randomNeighborLB.pathColor][0] == randomNeighborLB || pathDick[randomNeighborLB.pathColor][pathDick[randomNeighborLB.pathColor].Count - 1] == randomNeighborLB) && !OtherNeighborInSamePath(subtractedLB, randomNeighborLB)) 
						{
							pathDick[randomColor].Remove (subtractedLB);
							if (randomPathEndInt == 0) 
							{
								pathDick[randomColor][0].images[1].enabled = true;
								pathDick[randomColor][0].images[1].color = subtractedLB.pathColor;
							}
							else 
							{
								pathDick[randomColor][pathDick[subtractedLB.pathColor].Count - 1].images[1].enabled = true;
								pathDick[randomColor][pathDick[subtractedLB.pathColor].Count - 1].images[1].color = subtractedLB.pathColor;
							}
							randomNeighborLB.images[1].enabled = false;
							subtractedLB.images[1].enabled = true;
							subtractedLB.images[1].color = randomNeighborLB.pathColor;
							subtractedLB.pathColor = randomNeighborLB.pathColor;
							if (pathDick[randomNeighborLB.pathColor][0] == randomNeighborLB) 
							{
								pathDick[randomNeighborLB.pathColor].Insert (0, subtractedLB);
							}
							else pathDick[randomNeighborLB.pathColor].Add (subtractedLB);
							break;
						}
					}
				}
			}
			yield return null;
//			yield return new WaitForSeconds (1f);
		}

		// Clear paths
		foreach (Color pathColor in pathDick.Keys) 
		{
			for (int i = 0; i < pathDick[pathColor].Count; i ++) 
			{
				if (i != 0 && i != pathDick[pathColor].Count - 1) 
				{
					pathDick[pathColor][i].Unoccupy ();
				}
				else 
				{
					pathDick[pathColor][i].SetAsEndPoint (pathColor);
				}
			}
			pathDick[pathColor].Clear();
		}
		yield return new WaitForSeconds (0.25f);
		StartCoroutine (PuzzleTimer ());
	}
	

	private bool OtherNeighborInSamePath (LittleBox subtractedLB, LittleBox randomNeighborLB) 
	{
		foreach (LittleBox otherNeighbor in subtractedLB.neighbors)
		{
			if (otherNeighbor != randomNeighborLB && otherNeighbor.pathColor == randomNeighborLB.pathColor)  
			{
				return true;
			}  
		}
		return false;
	}
}
