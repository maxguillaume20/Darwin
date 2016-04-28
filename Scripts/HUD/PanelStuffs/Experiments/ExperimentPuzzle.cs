using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class ExperimentPuzzle : MonoBehaviour 
{
	protected ExperimentPanel expPanel;
//	public TouchyBall touchyBall;
	public float maxTime;
	public float lbLength, matrixYMin, matrixXMin;
	public int columnCount, rowCount;
	protected List<LittleBox[]> littleBoxMatrix = new List<LittleBox[]>();
//	public LittleBox firstLittleBox;
	

	public virtual void Initiate () 
	{
//		CreateLittleBoxMatrix (columnCount, rowCount);
//		touchyBall.Initiate ();
		expPanel = GetComponentInParent<ExperimentPanel> ();
		gameObject.SetActive (false);
	}

	public virtual void Enable () 
	{
		if (maxTime >= 10)
		{
			expPanel.mainTexts[1].text = "0:" + maxTime.ToString("n1");
		}
		else expPanel.mainTexts[1].text = "0:0" + maxTime.ToString("n1");
		CreateLittleBoxMatrix (columnCount, rowCount);
		gameObject.SetActive (true);
	}

	private void OnEnable () 
	{
		if (this == ExperimentPanel.activePuzzle) 
		{
			StartCoroutine (PuzzleGenerator());
		}
	}

	protected virtual void Start() 
	{

	}

	protected virtual IEnumerator PuzzleGenerator() 
	{
		yield return null;
	}

	public virtual void Disable () 
	{
		foreach (LittleBox[] pooPoo in littleBoxMatrix) 
		{
			for (int peePee = 0; peePee < pooPoo.Length; peePee++) 
			{
				pooPoo[peePee].Reset();
			}
		}
		littleBoxMatrix.Clear ();
		gameObject.SetActive (false);
	}

	public virtual void CheckIfPuzzleIsComplete() 
	{

	}

	public virtual void DoAction (LittleBox littleBox) 
	{
		
	}
	
	protected IEnumerator PuzzleTimer () 
	{
		expPanel.StartMoveTouchyBall ();
		expPanel.mainTexts [0].text = "";
		float time = maxTime;
		while (time > 0 && expPanel.puzzleActive) 
		{
			if (time >= 10)
			{
				expPanel.mainTexts[1].text = "0:" + time.ToString("n1");
			}
			else expPanel.mainTexts[1].text = "0:0" + time.ToString("n1");
			time -= Time.deltaTime;
			yield return null;
		}
		if (expPanel.puzzleActive) 
		{
			expPanel.EndPuzzle(false);
		}
	}

	protected void CreateLittleBoxMatrix (int newColumnCount, int newRowCount) 
	{
		columnCount = newColumnCount;
		rowCount = newRowCount;
		float matrixHeight = 0f;
		matrixYMin = expPanel.mainTexts [0].rectTransform.anchorMax.y - expPanel.mainTexts [0].rectTransform.anchorMin.y + ExperimentPanel.lbOffSet;
		matrixHeight = 1f - (expPanel.mainTexts [1].rectTransform.anchorMax.y - expPanel.mainTexts [1].rectTransform.anchorMin.y) - ExperimentPanel.lbOffSet - matrixYMin;
		if (GameManager.inPuzzleScene) 
		{
			expPanel.mainTexts[0].gameObject.SetActive(false);
			expPanel.mainTexts[1].gameObject.SetActive(false);
		}
		lbLength = 0.5f * matrixHeight / rowCount;
		matrixXMin = 0.5f * (1f - lbLength * columnCount);
		for (int i = 0; i < columnCount; i ++) 
		{
			littleBoxMatrix.Add (new LittleBox[rowCount]);
			ExperimentPanel.extraPuzzleWallArray[i].gameObject.SetActive (true);
			ExperimentPanel.extraPuzzleWallArray[i].rectTransform.anchorMin = new Vector2(matrixXMin + lbLength * i, matrixYMin + lbLength * ((float) Screen.width / Screen.height) * rowCount);
			ExperimentPanel.extraPuzzleWallArray[i].rectTransform.anchorMax = new Vector2(matrixXMin + lbLength * (1 + i), matrixYMin + lbLength * ((float) Screen.width / Screen.height) * rowCount + (lbLength * ((float) Screen.width / Screen.height) * PuzzleWall.horizontalWidth));
			for (int j = 0; j < rowCount; j ++) 
			{
				ExperimentPanel.littleBoxArray[j + i * rowCount].gameObject.SetActive (true);
				ExperimentPanel.littleBoxArray[j + i * rowCount].SetPosition (i, j);
				littleBoxMatrix[i][j] = ExperimentPanel.littleBoxArray[j + i * rowCount];
				if (i == columnCount - 1) 
				{
					ExperimentPanel.extraPuzzleWallArray[j + i + 1].gameObject.SetActive (true);
					ExperimentPanel.extraPuzzleWallArray[j + i + 1].rectTransform.anchorMin = new Vector2(matrixXMin + lbLength * columnCount, matrixYMin + lbLength * ((float) Screen.width / Screen.height) * j);
					ExperimentPanel.extraPuzzleWallArray[j + i + 1].rectTransform.anchorMax = new Vector2(matrixXMin + lbLength * columnCount + (lbLength * PuzzleWall.verticalWidth), matrixYMin + lbLength * ((float) Screen.width / Screen.height) * (1 + j));
				}
			}
		}
		// Find neighbors foreach littleBox
		for (int i = 0; i < littleBoxMatrix.Count; i ++) 
		{
			for (int j = 0; j < littleBoxMatrix[i].Length; j ++) 
			{
				int a = 0;
				int b = -1;
				for (int x = 0; x < 2; x ++) 
				{
					for (int y = 0; y < 2; y ++) 
					{
						if (i + a >= 0 && i + a < columnCount && j + b >= 0 && j + b < rowCount) 
						{
							littleBoxMatrix[i][j].neighbors.Add(littleBoxMatrix[i + a][j + b]);
						}
						b *= -1;
						a *= -1;
					}
					a --;
					b = 0;
				}
			}
		}
	}

//	protected void CreateLittleBoxMatrix (int newColumnCount, int newRowCount) 
//	{
//		columnCount = newColumnCount;
//		rowCount = newRowCount;
//		matrixYMin = expPanel.mainTexts [0].rectTransform.anchorMax.y - expPanel.mainTexts [0].rectTransform.anchorMin.y + ExperimentPanel.lbOffSet;
//		float matrixHeight = 1f - (expPanel.mainTexts [1].rectTransform.anchorMax.y - expPanel.mainTexts [1].rectTransform.anchorMin.y) - ExperimentPanel.lbOffSet - matrixYMin;
//		lbLength = 0.5f * matrixHeight / rowCount;
//		matrixXMin = 0.5f * (1f - lbLength * columnCount);
//		RectTransform thisRectTransfrom = GetComponent<RectTransform> ();
//		PuzzleWall[] extraPuzzleWalls = new PuzzleWall [rowCount + columnCount];
//		// Make extra puzzle walls. Each littleBox only has two walls, bottom and left,
//		// so there would be no top or right walls on the matrix without this
//		for (int i = 0; i < extraPuzzleWalls.Length; i ++) 
//		{
//			PuzzleWall newPuzzleWall = (PuzzleWall) Instantiate (expPanel.firstPuzzleWall);
//			newPuzzleWall.rectTransform.SetParent (thisRectTransfrom);
//			newPuzzleWall.rectTransform.localScale = Vector3.one;
//			extraPuzzleWalls[i] = newPuzzleWall;
//		}
//		for (int i = 0; i < columnCount; i ++) 
//		{
//			littleBoxMatrix.Add (new LittleBox[rowCount]);
//			extraPuzzleWalls[i].GetComponent<RectTransform>().anchorMin = new Vector2(matrixXMin + lbLength * i, matrixYMin + lbLength * ((float) Screen.width / Screen.height) * rowCount);
//			extraPuzzleWalls[i].GetComponent<RectTransform>().anchorMax = new Vector2(matrixXMin + lbLength * (1 + i), matrixYMin + lbLength * ((float) Screen.width / Screen.height) * rowCount + (lbLength * ((float) Screen.width / Screen.height) * PuzzleWall.horizontalWidth));
//			for (int j = 0; j < rowCount; j ++) 
//			{
//				LittleBox newLittleBox = (LittleBox) Instantiate (firstLittleBox);
//				newLittleBox.Initiate ();
//				newLittleBox.rectTransform.SetParent (thisRectTransfrom);
//				newLittleBox.rectTransform.localScale = Vector3.one;
//				newLittleBox.SetPosition (i, j);
//				littleBoxMatrix[i][j] = newLittleBox;
//				if (i == columnCount - 1) 
//				{
//					extraPuzzleWalls[j + i + 1].GetComponent<RectTransform>().anchorMin = new Vector2(matrixXMin + lbLength * columnCount, matrixYMin + lbLength * ((float) Screen.width / Screen.height) * j);
//					extraPuzzleWalls[j + i + 1].GetComponent<RectTransform>().anchorMax = new Vector2(matrixXMin + lbLength * columnCount + (lbLength * PuzzleWall.verticalWidth), matrixYMin + lbLength * ((float) Screen.width / Screen.height) * (1 + j));
//				}
//			}
//		}
//		firstLittleBox.gameObject.SetActive (false);
//		// Find non-diagonal neighbors for each littleBox
//		for (int i = 0; i < littleBoxMatrix.Count; i ++) 
//		{
//			for (int j = 0; j < littleBoxMatrix[i].Length; j ++) 
//			{
//				int a = 0;
//				int b = -1;
//				for (int x = 0; x < 2; x ++) 
//				{
//					for (int y = 0; y < 2; y ++) 
//					{
//						if (i + a >= 0 && i + a < columnCount && j + b >= 0 && j + b < rowCount) 
//						{
//							littleBoxMatrix[i][j].neighbors.Add(littleBoxMatrix[i + a][j + b]);
//						}
//						b *= -1;
//						a *= -1;
//					}
//					a --;
//					b = 0;
//				}
//			}
//		}
//	}
}
