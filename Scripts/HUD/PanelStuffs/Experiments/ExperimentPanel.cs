using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class ExperimentPanel : NonOverlayPanel 
{
	private Lab selectedLab;
	public ExperimentPuzzle[] expPuzzles;
	public static ExperimentPuzzle activePuzzle;
	public TouchyBall[] touchyBalls;
	public static TouchyBall activeTouchyBall;
	public static TouchyBallImage tbImage;
	public Text[] mainTexts;
	public Image resImage;
	private int maxMatrixSize = 70;
	public LittleBox firstLittleBox;
	public PuzzleWall firstPuzzleWall;
	public Button cancelButton;
	private bool cancelSelected;
	public bool puzzleActive;
	// 0 = Maze, 1 = Flow, 2 = Debugs
	public int activePuzzleIndex;
	private int lastActivePuzzleIndex = -1;
	private float endPuzzleWaitTime = 2f;
	public static float lbOffSet = 0.04f;
	// [0] = endPoint, [1] = path, [2] = pathTurn, [3] = pathEnd, [4] = endPointConnected, [5] = happyBug, [6] = deadBug
	public Sprite[] pathSprites;
	public static Sprite[] puzzleSprites { get; set; }
	public static LittleBox[] littleBoxArray { get; set; }
	public static PuzzleWall[] extraPuzzleWallArray { get; set; }

	private void Awake () 
	{
		if (GetComponentInParent<PuzzleScene>()) 
		{
			GameManager.inPuzzleScene = true;
		}
		puzzleSprites = pathSprites;
		mainTexts = GetComponentsInChildren<Text> ();
		foreach (ExperimentPuzzle expPuzzle in expPuzzles) 
		{
			expPuzzle.Initiate();
		}
		firstPuzzleWall.gameObject.SetActive (false);
		RectTransform thisRectTransform = GetComponent<RectTransform> ();
		littleBoxArray = new LittleBox[(int)(Mathf.Pow (maxMatrixSize, 2))];
		littleBoxArray [0] = firstLittleBox;
		firstLittleBox.Initiate ();
		for (int i = 1; i < littleBoxArray.Length; i ++) 
		{
			LittleBox newLittleBox = (LittleBox) Instantiate (firstLittleBox);
			newLittleBox.Initiate();
			newLittleBox.rectTransform.SetParent (thisRectTransform);
			newLittleBox.rectTransform.localScale = Vector3.one;
			littleBoxArray[i] = newLittleBox;
			newLittleBox.Reset ();
		}
		firstLittleBox.Reset ();
		extraPuzzleWallArray = new PuzzleWall[maxMatrixSize * 2];
		extraPuzzleWallArray [0] = firstPuzzleWall;
		firstPuzzleWall.Initiate ();
		for (int i = 1; i < extraPuzzleWallArray.Length; i ++) 
		{
			PuzzleWall newPuzzleWall = (PuzzleWall) Instantiate (firstPuzzleWall);
			newPuzzleWall.Initiate ();
			newPuzzleWall.rectTransform.SetParent (thisRectTransform);
			newPuzzleWall.rectTransform.localScale = Vector3.one;
			extraPuzzleWallArray[i] = newPuzzleWall;
			newPuzzleWall.gameObject.SetActive (false);
		}
		firstPuzzleWall.gameObject.SetActive (false);
		tbImage = GetComponentInChildren<TouchyBallImage> ();
		tbImage.Initiate ();
		tbImage.image.rectTransform.sizeDelta = new Vector2 (Screen.width * 0.3f, Screen.width * 0.3f);
	}

	public void StartExperiment(Lab newSelectedLab) 
	{
		selectedLab = newSelectedLab;
		NonOverlayCamera.Enable (panelID);
		while (activePuzzleIndex == lastActivePuzzleIndex) 
		{
			activePuzzleIndex = Random.Range (0, expPuzzles.Length);
		}
		InitiateExperiment ();
	}

	private void InitiateExperiment () 
	{
		puzzleActive = true;
		lastActivePuzzleIndex = activePuzzleIndex;
		activePuzzle = expPuzzles [activePuzzleIndex];
		expPuzzles [activePuzzleIndex].Enable ();
		if (activePuzzle as PathPuzzle) 
		{
			activeTouchyBall = touchyBalls[1];
		}
		else 
		{
			activeTouchyBall = touchyBalls[0];
		}
		activeTouchyBall.SetActivePuzzle ();
	}

	private void OnEnable() 
	{
		if (GameManager.inPuzzleScene) 
		{
			InitiateExperiment ();
		}
	}

	public void DoAction(LittleBox lb) 
	{
		expPuzzles [activePuzzleIndex].DoAction (lb);
	}

	public void StartMoveTouchyBall() 
	{
		StartCoroutine (MoveTouchyBall ());
	}

	private IEnumerator MoveTouchyBall () 
	{
		Vector3 lastMousePosition = Vector3.zero;
		LittleBox lastLittleBox = null;
		Camera activeCamera;
		if (!GameManager.inPuzzleScene) 
		{
			activeCamera = NonOverlayCamera.thisCamera;
		}
		else 
		{
			activeCamera = Camera.main;
		}
		while (puzzleActive)
		{
			// Mouse
			if (Input.GetMouseButton(0) && (Input.mousePosition != lastMousePosition || !activeTouchyBall.gameObject.activeSelf)) 
			{
				lastMousePosition = Input.mousePosition;
				if (!activeTouchyBall.gameObject.activeSelf) 
				{

					activeTouchyBall.gameObject.SetActive(true);
				}
				RaycastHit hit;
				if (Physics.Raycast (activeCamera.ScreenPointToRay (Input.mousePosition), out hit, 100f/*, LayerMask.NameToLayer("UI")*/)) 
				{
					LittleBox newLittleBox = hit.transform.gameObject.GetComponent<LittleBox>();
					if (newLittleBox != null && (newLittleBox != lastLittleBox || Input.GetMouseButtonDown (0)))
					{
						DoAction (newLittleBox);
						activeTouchyBall.TouchBox (newLittleBox);
						lastLittleBox = newLittleBox;
					}
					if (activeTouchyBall.outOfBounds) activeTouchyBall.BackInBounds();
				}
				else 
				{
					activeTouchyBall.OutOfBounds ();
				}
				activeTouchyBall.Move ();
			}
			else if (!Input.GetMouseButton(0) && activeTouchyBall.gameObject.activeSelf) activeTouchyBall.gameObject.SetActive(false);
			// Finger Touch
//			if (Input.touchCount > 0 && Input.GetTouch(0).phase != TouchPhase.Stationary) 
//			{
//				// snaps to middle of closest littlebox
//				if (Input.GetTouch(0).phase == TouchPhase.Moved) 
//				{
//					itb.Move(Input.GetTouch(0).position);
//				}
//				else if (Input.GetTouch(0).phase == TouchPhase.Began) 
//				{
//					itb.gameObject.SetActive(true);
//					itb.Move(Input.GetTouch(0).position);
//				}
//				else itb.gameObject.SetActive(false);
//			}
			yield return null;
		}
	}

	public void CancelPuzzle() 
	{
		if (!cancelSelected) 
		{
			cancelSelected = true;
			cancelButton.image.color = cancelButton.colors.highlightedColor;
			StartCoroutine(CancelCountDown());
		}
		else if (GameManager.inPuzzleScene) 
		{
			gameObject.SetActive (false);
			gameObject.SetActive (true);
		}
		else 
		{
			NonOverlayCamera.Disable ();
		}
	}

	private IEnumerator CancelCountDown() 
	{
		yield return new WaitForSeconds (1.5f);
		cancelSelected = false;
		cancelButton.image.color = cancelButton.colors.normalColor;
	}

	public void EndPuzzle(bool solved) 
	{
		activeTouchyBall.gameObject.SetActive (false);
		puzzleActive = false;
		if (!GameManager.inPuzzleScene) 
		{
			resImage.gameObject.SetActive(true);
			if (solved) 
			{
				resImage.rectTransform.anchorMin = new Vector2 (0.75f, resImage.rectTransform.anchorMin.y);
				resImage.rectTransform.anchorMax = new Vector2 (0.8f, resImage.rectTransform.anchorMax.y);
				mainTexts[0].text = "Successful Experiment\t+" + (int)selectedLab.uniqueStatsArray[0];
				GameManager.HumanPlayer.ChangeResource(ResourceType.Unique, selectedLab.uniqueStatsArray[0]);
			}
			else 
			{
				resImage.rectTransform.anchorMin = new Vector2 (0.7f, resImage.rectTransform.anchorMin.y);
				resImage.rectTransform.anchorMax = new Vector2 (0.75f, resImage.rectTransform.anchorMax.y);
				mainTexts[0].text = "Failed Experiment\t+" + (int)(selectedLab.uniqueStatsArray[1]);
				GameManager.HumanPlayer.ChangeResource(ResourceType.Unique, selectedLab.uniqueStatsArray[1]);
			}
			StartCoroutine (EndPuzzleCoroutine ());
		}
	}

	private IEnumerator EndPuzzleCoroutine() 
	{
		yield return new WaitForSeconds (endPuzzleWaitTime);
		NonOverlayCamera.Disable ();
	}

	private void OnDisable() 
	{
		if (activeTouchyBall) activeTouchyBall.gameObject.SetActive (false);
		if (activePuzzleIndex >= 0) 
		{
			expPuzzles [activePuzzleIndex].Disable ();
		}
		foreach (PuzzleWall puzzleWall in extraPuzzleWallArray) puzzleWall.gameObject.SetActive (false);
		activePuzzle = null;
		cancelSelected = false;
		cancelButton.image.color = cancelButton.colors.normalColor;
		resImage.gameObject.SetActive(false);
		if (!GameManager.inPuzzleScene) 
		{
			GameManager.HumanPlayer.userInput.enabled = true;
			if (selectedLab) 
			{
				selectedLab.StartExperimentCoolDown ();
			}
		}
	}
}
