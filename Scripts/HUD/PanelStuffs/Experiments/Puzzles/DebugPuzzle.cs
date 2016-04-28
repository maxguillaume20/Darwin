using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DebugPuzzle : ExperimentPuzzle 
{
	private int matrixSize = 4;
	private float generationTime = 3f;
	private bool isGenerating;
	public Sprite[] deBugSprites;
	private List<LittleBox> resolvedBugsList = new List<LittleBox> (); 
	private int bugsCount;
	
	public override void Enable ()
	{
		rowCount = matrixSize;
		columnCount = matrixSize;
		expPanel.mainTexts [0].text = "Debug";
		bugsCount = 0;
		base.Enable ();
		for (int i = 0; i < matrixSize; i ++) 
		{
			for (int j = 0; j < matrixSize; j ++) 
			{
				littleBoxMatrix[i][j].images[1].rectTransform.anchorMin = new Vector2 (PuzzleWall.horizontalWidth, PuzzleWall.verticalWidth); 
			}
		} 
	}
	
	protected override IEnumerator PuzzleGenerator ()
	{
		isGenerating = true;
//		int numberOfDoublePress=5;
		int lastX=-1;
		int lastY=-1;
		for (int i = 0;i < matrixSize;i ++)
		{
			for (int j = 0;j < matrixSize;j ++)
			{
				littleBoxMatrix [i] [j].images[0].color=Color.gray;
			}
		}
//		for (int i = 0; i < numberOfDoublePress; i ++)
		for (float time = 0f; time < generationTime || bugsCount <= 1; time += Time.deltaTime)
		{
			int pressX=Random.Range(0, matrixSize-1);
			int pressY=Random.Range(0, matrixSize-1);
			if (pressX != lastX || pressY != lastY)
			{
				Press (littleBoxMatrix[pressX][pressY]);
				lastX=pressX;
				lastY=pressY;
			}
			yield return null;
//			yield return new WaitForSeconds (1f);
		}
		isGenerating = false;
		StartCoroutine (ResetBoxs ());
		StartCoroutine (PuzzleTimer ());
	}
	
	
	private void Press(LittleBox littleBox)
	{
		foreach (LittleBox neighbor in littleBox.neighbors) 
		{
			if (!neighbor.occupied) 
			{
				CreateBug (neighbor);
			}
			else 
			{
				ResolveBug (neighbor);
			}
		}
		if (!littleBox.occupied) 
		{
			CreateBug (littleBox);
		}
		else 
		{
			ResolveBug (littleBox);
		}
	}
	
	public override void DoAction(LittleBox littleBox) 
	{
		base.DoAction (littleBox);
		Press (littleBox);
		if (bugsCount == 0) 
		{
			expPanel.EndPuzzle (true);
		}
	}

	private void CreateBug (LittleBox littleBox) 
	{
		bugsCount ++;
		resolvedBugsList.Remove (littleBox);
		littleBox.occupied = true;
//		littleBox.images[0].color = Color.yellow;
		littleBox.images[1].enabled = true;
		littleBox.images [1].sprite = ExperimentPanel.puzzleSprites[5];
	}

	private void ResolveBug (LittleBox littleBox) 
	{
		bugsCount --;
		littleBox.occupied = false;
		if (!isGenerating) 
		{
			littleBox.images[1].sprite = ExperimentPanel.puzzleSprites[6];
			littleBox.timer = 0f;
			resolvedBugsList.Add (littleBox);
		}
		else
		{
			littleBox.images[1].enabled = false;
//			littleBox.images[0].color = Color.gray;
		}
	}

	private IEnumerator  ResetBoxs() 
	{
		List<LittleBox> resetList = new List<LittleBox> ();
		while (expPanel.gameObject.activeSelf) 
		{
			if (resolvedBugsList.Count > 0) 
			{
				foreach (LittleBox littleBox in resolvedBugsList) 
				{
					littleBox.timer += Time.deltaTime;
					if (littleBox.timer >= 0.6f) 
					{
						resetList.Add (littleBox);
					}
				}
				foreach (LittleBox littleBox in resetList) 
				{
					resolvedBugsList.Remove (littleBox);
					littleBox.images[1].enabled = false;
//					littleBox.images[0].color = Color.gray;
				}
				resetList.Clear ();
			}
			yield return null;
		}
	}

	private void OnDisable () 
	{
		if (this == ExperimentPanel.activePuzzle) 
		{
			for (int i = 0; i < matrixSize; i ++) 
			{
				for (int j = 0; j < matrixSize; j ++) 
				{
					littleBoxMatrix[i][j].images[1].rectTransform.anchorMin = new Vector2 (0, 0); 
				}
			} 
		}
	}
}
