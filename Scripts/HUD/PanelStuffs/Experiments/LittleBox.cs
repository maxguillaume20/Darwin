using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class LittleBox : MonoBehaviour 
{
	public int[] position = new int[2];
	public int pathPosition;
	public List<LittleBox> neighbors = new List<LittleBox>();
	// [0] = background, [1] = path, [2] = horizontal wall, [3] = vertical wall
	public Image[] images;
	public bool isEndPoint;
	public Color pathColor;
	public Text debugText;
	public PuzzleWall[] puzzleWalls;
	public bool occupied;
	public Vector2 centerPosition;
	public RectTransform rectTransform;
	public float timer;
	public bool isTraversed;

	public void Initiate() 
	{
		debugText = GetComponentInChildren<Text> ();
		images = new Image[2];
		Image[] imagesArray = GetComponentsInChildren<Image> ();
		images[0] = imagesArray [0];
		images [1] = imagesArray [1];
		images [1].enabled = false;
		puzzleWalls = GetComponentsInChildren<PuzzleWall> ();
		foreach (PuzzleWall puzzleWall in puzzleWalls) 
		{
			puzzleWall.Initiate();
			puzzleWall.rectTransform.anchorMin = Vector2.zero;
		}
		puzzleWalls [0].rectTransform.anchorMax = new Vector2 (PuzzleWall.verticalWidth, 1);
		puzzleWalls [1].rectTransform.anchorMax = new Vector2 (1, PuzzleWall.horizontalWidth);
		pathPosition = -1;
		rectTransform = GetComponent<RectTransform> ();
	}

	public void SetPosition (int x, int y) 
	{
		position [0] = x;
		position [1] = y;
		rectTransform.anchorMin = new Vector2 (ExperimentPanel.activePuzzle.matrixXMin + ExperimentPanel.activePuzzle.lbLength * x, ExperimentPanel.activePuzzle.matrixYMin + ExperimentPanel.activePuzzle.lbLength * ((float) Screen.width / Screen.height)* y);
		rectTransform.anchorMax = new Vector2 (ExperimentPanel.activePuzzle.matrixXMin + ExperimentPanel.activePuzzle.lbLength * (1 + x), ExperimentPanel.activePuzzle.matrixYMin + ExperimentPanel.activePuzzle.lbLength * ((float) Screen.width / Screen.height) * (1 + y));
		BoxCollider boxCollider = GetComponent<BoxCollider> ();
		boxCollider.size = new Vector3 (Screen.width * (rectTransform.anchorMax.x - rectTransform.anchorMin.x), Screen.height * (rectTransform.anchorMax.y - rectTransform.anchorMin.y), 1f);
		centerPosition = new Vector2 ((rectTransform.anchorMax.x + rectTransform.anchorMin.x) / 2f * Screen.width, (rectTransform.anchorMax.y + rectTransform.anchorMin.y) / 2f * Screen.height);
	}

	public void SetAsEndPoint (Color newPathColor) 
	{
		isEndPoint = true;
		Occupy (newPathColor, ExperimentPanel.puzzleSprites[0], 0);
		images [0].color = Color.clear;
	}

	public virtual void Occupy (Color newPathColor, Sprite pathSprite, int zRotation) 
	{
		occupied = true;
		pathColor = newPathColor;
		SetSprite (newPathColor, pathSprite, zRotation);
	}

	public void SetSprite (Color newPathColor, Sprite pathSprite, int zRotation) 
	{
		images[1].enabled = true;
		images [1].color = newPathColor;
		images[1].sprite = pathSprite;
		images[1].transform.localEulerAngles = new Vector3 (0, 0, zRotation);
	}

	public virtual void Unoccupy () 
	{
		pathPosition = -1;
		images [0].color = Color.clear;
		if (!isEndPoint) 
		{
			pathColor = Color.black;
			occupied = false;
			images [1].enabled = false;
		}
		else 
		{
			SetSprite (pathColor, ExperimentPanel.puzzleSprites[0], 0);
		}
		isTraversed = false;
		debugText.text = "";
	}

	public virtual void Reset() 
	{
		puzzleWalls[0].image.enabled = true;
		puzzleWalls[1].image.enabled = true;
		images [0].enabled = true;
		images [0].color = Color.clear;
		images [1].transform.localEulerAngles = Vector3.zero;
		images [1].color = Color.white;
		isEndPoint = false;
		neighbors.Clear ();
		debugText.text = "";
		Unoccupy ();
		gameObject.SetActive (false);
	}
}
