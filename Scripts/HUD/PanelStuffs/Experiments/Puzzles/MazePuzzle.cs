using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class MazePuzzle : PathPuzzle 
{
	private List<LittleBox> cellList = new List<LittleBox>();
	private Dictionary<LittleBox, List<LittleBox>> connectedCells = new Dictionary<LittleBox, List<LittleBox>>();
	private Dictionary<LittleBox, LittleBox> forkDeadEndDick = new Dictionary<LittleBox, LittleBox> ();
	private float randomSelectChance = 0.2f;
	public static LittleBox finishEndPoint { get; set; }
	// [0] = mouse, [1] = cheese
	public Sprite[] mazeSprites;
	public Color pathColor = Color.red;
//	private Color finishEndPointColor = Color.blue;
//	private Color startEndPointColor = Color.red;
	private int cellsPerFrame = 5;

	public override void Initiate ()
	{
		base.Initiate ();
	}

	public override void Enable() 
	{
		expPanel.mainTexts [0].text = "Get the Cheese";
		if (!GameManager.inPuzzleScene) 
		{
			columnCount = Random.Range (9, 15);
			rowCount = Random.Range (6, 9);
		}
		else 
		{
			expPanel.mainTexts [0].text = "";
		}
		pathDick.Add (pathColor, new MazePathList ());
		pathDick [pathColor].pathColor = pathColor;

		if (maxTime >= 10)
		{
			expPanel.mainTexts[1].text = "0:" + maxTime.ToString("n1");
		}
		else expPanel.mainTexts[1].text = "0:0" + maxTime.ToString("n1");
		base.Enable ();
	}

	public override void Disable ()
	{
		expPanel.mainTexts [0].text = "";
		forkDeadEndDick.Clear ();
		finishEndPoint = null;
		connectedCells.Clear ();
		cellList.Clear ();
		base.Disable ();
	}

	protected override IEnumerator PuzzleGenerator () 
	{
		int xPos = Random.Range(0, columnCount), yPos = Random.Range(0, rowCount);
		finishEndPoint = littleBoxMatrix [xPos] [yPos];
		cellList.Add (finishEndPoint);
		finishEndPoint.occupied = true;
//		finishEndPoint.images [0].color = finishEndPointColor;
		finishEndPoint.pathPosition = 0;
//		finishEndPoint.debugText.text = finishEndPoint.pathPosition.ToString ();
		LittleBox startEndPoint = finishEndPoint;
		List<LittleBox> deadEndList = new List<LittleBox> ();
//		Dictionary<LittleBox, LittleBox> forkDeadEndDick = new Dictionary<LittleBox, LittleBox> ();
		// Generate Maze
		while (cellList.Count > 0) 
		{
			for (int z = 0; z < cellsPerFrame && cellList.Count > 0; z ++) 
			{
				LittleBox currentCell = cellList[GetCellListIndex()];
				if (!connectedCells.ContainsKey(currentCell)) 
				{
					connectedCells.Add(currentCell, new List<LittleBox>());
				}
				List<int> neighborIndices = new List<int> {0, 1, 2, 3};
				if (currentCell.neighbors.Count < neighborIndices.Count) 
				{
					neighborIndices.RemoveRange(currentCell.neighbors.Count, neighborIndices.Count - currentCell.neighbors.Count);
				} 
				// Randomly selects neighbors until it finds an unoccupied one, if all are occupied,
				// removes cell from cellList
				for( int i = 0; i < currentCell.neighbors.Count; i ++) 
				{
					int randNeighborInt = neighborIndices[Random.Range(0, neighborIndices.Count)];
					neighborIndices.Remove(randNeighborInt);
					LittleBox neighborCell = currentCell.neighbors[randNeighborInt];
					// if neighbor is unoccupied, add neighbor to list
					if (!neighborCell.occupied) 
					{
						AddLBtoCellList(neighborCell, currentCell);
						// keeps track of forks (cells with 3 or more connected neighbors)
						if (connectedCells[currentCell].Count == 3)
						{
							currentCell.images[0].color = Color.yellow;
							forkDeadEndDick.Add (currentCell, currentCell);
						}
						break;
					}
					else if (i == currentCell.neighbors.Count - 1) 
					{
						cellList.Remove (currentCell);
						if (connectedCells[currentCell].Count == 2) 
						{
							currentCell.images[0].color = Color.clear;
						}
						else if (connectedCells[currentCell].Count == 1) 
						{
//							if (currentCell != startEndPoint && currentCell != finishEndPoint) 
//							{
								currentCell.images[0].color = Color.cyan;
//							}
							deadEndList.Add (currentCell);
						}
						break;
					}
				}
			}
			yield return null;
//			yield return new WaitForSeconds (1f);
		}
		// Get deadEnd Paths
		LittleBox startFork = null;
		int furthestDisance = 0;
		for (int z = 0; z < deadEndList.Count; z ++)
		{
			LittleBox deadEnd = deadEndList[z];
			LittleBox fork = ConnectFurthestDeadEndToFork (deadEnd, deadEnd.pathPosition, connectedCells[deadEnd][0].pathPosition - deadEnd.pathPosition);
			if (fork) 
			{
				if (forkDeadEndDick[fork] != fork) 
				{
					forkDeadEndDick[fork].images[0].color = Color.blue;
				}
				forkDeadEndDick[fork] = deadEnd;
				forkDeadEndDick[fork].images[0].color = Color.cyan;
			}
			if (deadEnd.pathPosition > furthestDisance) 
			{
				furthestDisance = deadEnd.pathPosition;
				startEndPoint = deadEnd;
				startFork = fork;
			}
				yield return null;
		}

		// Find longest path
		List<LittleBox> tempForkList = new List<LittleBox> ();
		float longestPathDistance = startEndPoint.pathPosition;
		float currentPathDistance = startEndPoint.pathPosition - startFork.pathPosition;
		startFork.images [0].color = Color.green;
		startFork.isTraversed = true;
		tempForkList.Add (startFork);
		while (tempForkList.Count > 0) 
		{
			yield return null;
			for (int z = 0; z < (int) (cellsPerFrame / 5) && tempForkList.Count > 0; z ++) 
			{
				LittleBox currentFork = tempForkList[tempForkList.Count - 1];
				for (int i = 0; i < connectedCells[currentFork].Count; i ++) 
				{
					LittleBox neighbor = connectedCells[currentFork][i];
					if (!neighbor.isTraversed) 
					{
						LittleBox connectedFork = FindConnectedForks (neighbor, currentFork);
						float forkedPathDis = Mathf.Abs (connectedFork.pathPosition - currentFork.pathPosition);
						currentPathDistance += forkedPathDis;
						float deadEndPathDis = Mathf.Abs (connectedFork.pathPosition - forkDeadEndDick[connectedFork].pathPosition);
						if (currentPathDistance + deadEndPathDis > longestPathDistance) 
						{
							longestPathDistance = currentPathDistance + deadEndPathDis;
							finishEndPoint.images[0].color = Color.blue;
							finishEndPoint = forkDeadEndDick[connectedFork];
						}
						connectedFork.images[0].color = Color.green;
						tempForkList.Add (connectedFork);
						break;
					}
					if (i == connectedCells[currentFork].Count - 1) 
					{
						currentFork.images[0].color = Color.magenta;
						if (tempForkList.Count > 1) 
						{
							currentPathDistance -= Mathf.Abs (currentFork.pathPosition - tempForkList[tempForkList.Count - 2].pathPosition);
						}
						tempForkList.Remove (currentFork);
					}
				}
			}
		}
		for (int i = 0; i < columnCount; i ++) 
		{
			for ( int j = 0; j < rowCount; j++) 
			{
				littleBoxMatrix[i][j].Unoccupy ();
			}
		}
		startEndPoint.SetAsEndPoint (pathColor);
//		startEndPoint.images [1].sprite = ExperimentPanel.puzzleSprites [0];
//		startEndPoint.images [1].color = Color.red;
		startEndPoint.images [1].sprite = mazeSprites [0];
		startEndPoint.images [1].color = Color.white;
		finishEndPoint.images [1].enabled = true;
//		finishEndPoint.images [1].sprite = ExperimentPanel.puzzleSprites [0];
//		finishEndPoint.images [1].color = Color.blue;
		finishEndPoint.images [1].sprite = mazeSprites [1];
		StartCoroutine (PuzzleTimer ());
	}

	private LittleBox ReturnBoxValue (LittleBox lb) 
	{
		return lb;
	}

	private LittleBox FindConnectedForks (LittleBox neighbor, LittleBox lastCell) 
	{
		neighbor.isTraversed = true;
		if (connectedCells[neighbor].Count > 2) 
		{
			return neighbor;
		}
		else 
		{
			neighbor.images[0].color = Color.red;
			foreach (LittleBox newNeighbor in connectedCells[neighbor]) 
			{
				if (newNeighbor != lastCell) 
				{
					return FindConnectedForks (newNeighbor, neighbor);
				}
			}
		}
		return null;
	}

	private LittleBox ConnectFurthestDeadEndToFork (LittleBox cell, int startingPosition, int direction) 
	{
		cell.images[0].color = Color.blue;
		cell.isTraversed = true;
		foreach (LittleBox neighbor in connectedCells[cell]) 
		{
			if (connectedCells[neighbor].Count > 2 && Mathf.Abs(startingPosition - neighbor.pathPosition) > Mathf.Abs (forkDeadEndDick[neighbor].pathPosition - neighbor.pathPosition))
			{
				return neighbor;
			}
			else if (connectedCells[neighbor].Count <= 2 && neighbor.pathPosition - cell.pathPosition == direction) 
			{
				return ConnectFurthestDeadEndToFork (neighbor, startingPosition, direction);
			}
		}
		return null;
	}

	private int GetCellListIndex() 
	{
		if (Random.Range (0f, 1f) < randomSelectChance && cellList.Count > 1) 
		{
			// Not including the firstCell in this range ensures that it will only have one neighbor
			// so that there's no problems when finding the longest path which compares cells' distances from the firstCell
			return Random.Range (1, cellList.Count);
		}
		return cellList.Count - 1;
	}

	private void AddLBtoCellList(LittleBox neighborCell, LittleBox currentCell) 
	{
		neighborCell.images [0].color = Color.green;
		cellList.Add (neighborCell);
		pathDick[pathColor].ConnectNeighbors (true, currentCell, neighborCell);
		connectedCells [currentCell].Add (neighborCell);
		if(!connectedCells.ContainsKey(neighborCell)) connectedCells.Add(neighborCell, new List<LittleBox>());
		connectedCells [neighborCell].Add (currentCell);
		neighborCell.pathPosition = currentCell.pathPosition + 1;
		neighborCell.occupied = true;
//		neighborCell.debugText.text = neighborCell.pathPosition.ToString (); 
	}

	protected override bool IsValidPath (List<LittleBox> newLbList) 
	{
		for (int i = 1; i < newLbList.Count; i ++)
		{
			if (!connectedCells[newLbList[i - 1]].Contains(newLbList[i]) || newLbList[i].occupied && (!newLbList[i].isEndPoint || newLbList[i] == activatedEndPoint))
			{
				return false;
			}
		}
		return true;
	}
}