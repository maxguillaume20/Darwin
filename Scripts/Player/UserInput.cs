using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class UserInput : MonoBehaviour 
{
	public bool usingMouseInput;
	public MouseInput mouseInput;
	private Player player;
	private float scrollSpeed = 1.5f;
	public float ZoomSpeed = 1f;
	public List<WorldObject> SelectedObjects = new List<WorldObject>();
	public SelectBox SelectionBox;
	public float touchTime;
	public bool cameramoved;

	void Awake () 
	{
		player = GetComponent<Player> ();
		mouseInput = gameObject.AddComponent<MouseInput> ();
		usingMouseInput = true;
//		if (player != GameManager.HumanPlayer) 
//		{
//			mouseInput.enabled = false;
//			enabled = false;
//		}
	}

	private void OnEnable() 
	{
		touchTime = 0f;
	}

	private void Update () 
	{
		if (!usingMouseInput) 
		{
			FingerInput ();
		}
		else 
		{
			mouseInput.HandleInput ();
		}
	}

	private void FingerInput () 
	{
		if (Input.touchCount > 0) 
		{
			touchTime += Time.deltaTime;
			if (Input.touchCount == 1)
			{
				MoveCameraOrTouchStuff();
			}
			else if (Input.touchCount == 2)
			{
				AdjustSelectionBox();
				if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(1).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Canceled) 
				{
					MultiSelect ();
				}
			}
		}
		else touchTime = 0f;
	}

	private void MoveCameraOrTouchStuff() 
	{
		if ((Input.GetTouch(0).phase == TouchPhase.Ended) && NotOnOpenPanel() && GameManager.FingerInBounds(Input.GetTouch(0).position) && !cameramoved && touchTime > 0.1f)
		{
			// I don't think this is working properly, especially if you have fat fingers
			SelectObjectOrMoveUnits();
		}
		else if ((Input.GetTouch(0).phase == TouchPhase.Ended) && cameramoved)
		{
			cameramoved = false;
			touchTime = 0.0f;
		}
		else if (Input.GetTouch(0).phase == TouchPhase.Moved && NotOnOpenPanel()) 
		{
			MoveCamera ();
			cameramoved = true;
		}
	}

	private void SelectObjectOrMoveUnits()
	{
		// Rays do not detect colliders that they originate in, which is why we have two Vector3s;
		Vector3 rayStartPoint = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		Vector3 worldTouchPoint = WorldTouchPoint (rayStartPoint);
		RaycastHit[] HitObjects = Physics.RaycastAll(rayStartPoint, Camera.main.transform.forward, 100f, GameManager.woLayerMask.value);
		if (HitObjects.Length > 0) 
		{
			foreach (RaycastHit hitObject in HitObjects)
			{
				WorldObject worldobject = hitObject.collider.gameObject.GetComponent<WorldObject>();
				if (worldobject)
				{
					if (player.units.SelectedUnitsCount() > 0 && (!worldobject.IsOwnedBy(player.species) || worldobject as StrategicPoint && player.units.selectedCaravans.Count > 0))
					{
						// move units towards a target of a different species
						player.units.MoveUnits(worldTouchPoint, worldobject);
					} 
					else 
					{
						// select new worldbject
						if (SelectedObjects.Count != 0)
						{
							Deselect();
						}
						SelectObject(hitObject);
					}
				}
			}
		}
		// move units towards empty space
		else if (player.units.SelectedUnitsCount() > 0)
		{

			player.units.MoveUnits(worldTouchPoint, null);
		}
	}

	public void SelectObject(RaycastHit HitObject)
	{
		SelectedObjects.Add(HitObject.transform.GetComponentInParent<WorldObject>());
		if (SelectedObjects[0] as Unit) 
		{
			OpenPanel (SelectedObjects[0].buttons);
		}
		SelectedObjects[0].SelectTap(player);
	}

	public void SelectCapital()
	{
		if (SelectedObjects.Count > 0) 
		{
			Deselect ();
		}
		SelectedObjects.Add (player.capital.GetComponent<WorldObject>());
		// these next 4 lines of code should be in the override method SelectTap() for the Capital
		GameManager.Hud.OpenMainPanel();
		List<RTS.PanelButtonType> buttons = SelectedObjects[0].GetButtons();
		for (int i = 0; i < buttons.Count; i++)
		{
			GameManager.Hud.OpenPanelButton(buttons[i]);
		}
		SelectedObjects[0].SelectTap (player);
	}

	public void OpenPanel(List<PanelButtonType> buttons) 
	{
		GameManager.Hud.OpenMainPanel();
		for (int i = 0; i < buttons.Count; i++)
		{
			GameManager.Hud.OpenPanelButton(buttons[i]);
		}
	}

	public void Deselect()
	{
		foreach (WorldObject wo in SelectedObjects) 
		{
			wo.Deselect();		
		}
		ClearSelectedObjects ();
	}

	public void ClearSelectedObjects () 
	{
		SelectedObjects.Clear ();
		player.units.ClearSelectedUnits ();
		touchTime = 0.0f;
	}

	protected Vector3 WorldTouchPoint (Vector3 inputPoint) 
	{
		float camRotAdjustment = Mathf.Tan (Mathf.Deg2Rad * (90f - Camera.main.transform.rotation.eulerAngles.x)) * inputPoint.y;
		return new Vector3 (inputPoint.x, 0f, inputPoint.z + camRotAdjustment);
	}

	public void MoveCamera()
	{	
		// Camera is rotated 90 degrees on x axis so its y & z axis are switched 
		Camera.main.transform.Translate(new Vector3 (-Input.GetTouch(0).deltaPosition.x * scrollSpeed * Mathf.Pow((Camera.main.orthographicSize / 30.0f),1.2f), -Input.GetTouch(0).deltaPosition.y * scrollSpeed * Mathf.Pow((Camera.main.orthographicSize / 30.0f),1.2f), 0.0f));
	}

	private void AdjustSelectionBox()
	{
		if (!SelectBox.isActive) 
		{
			SelectBox.Enable ();
		}
		Vector3 worldClickedPosition = Camera.main.ScreenToWorldPoint (Input.GetTouch(0).position);
		Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint (Input.GetTouch(1).position);
		float xMin = Mathf.Min (worldMousePosition.x, worldClickedPosition.x);
		float xMax = Mathf.Max (worldMousePosition.x, worldClickedPosition.x);
		float yMin = Mathf.Min (worldMousePosition.y, worldClickedPosition.y);
		float yMax = Mathf.Max (worldMousePosition.y, worldClickedPosition.y);
		float zMin = Mathf.Min (worldMousePosition.z, worldClickedPosition.z);
		float zMax = Mathf.Max (worldMousePosition.z, worldClickedPosition.z);
		SelectBox.AdjustSize (new Vector3 (xMin, yMin, zMin), new Vector3 (xMax, yMax, zMax));
	}

	private void MultiSelect () 
	{
		player.userInput.Deselect ();
		SelectBox.Disable ();
		Vector3 groundClickedPosition = WorldTouchPoint (Camera.main.ScreenToWorldPoint (Input.GetTouch(0).position));
		Vector3 groundMousePosition = WorldTouchPoint (Camera.main.ScreenToWorldPoint (Input.GetTouch(1).position));
		float distance = Mathf.Abs (groundMousePosition.x - groundClickedPosition.x);
		float xMin = Mathf.Min (groundMousePosition.x, groundClickedPosition.x);
		float zMin = Mathf.Min (groundMousePosition.z, groundClickedPosition.z);
		float zMax = Mathf.Max (groundMousePosition.z, groundClickedPosition.z);
		List<MobileWorldObject> mobileWoList = new List<MobileWorldObject> ();
		RaycastHit[] hits = Physics.CapsuleCastAll (new Vector3 (xMin, 0f, zMin), new Vector3 (xMin, 0f, zMax), 0.1f, Vector3.right, distance, LayerMask.GetMask (new string[] {player.species.ToString ()}));
		foreach (RaycastHit hit in hits) 
		{
			MobileWorldObject mobileWO = hit.collider.gameObject.GetComponent<MobileWorldObject> ();
			if (mobileWO && mobileWO as SentryMob == null) 
			{
				mobileWoList.Add (mobileWO);
			}
		}
		foreach (MobileWorldObject mobileWO in mobileWoList) 
		{
			mobileWO.SelectTap (player);
			player.userInput.SelectedObjects.Add (mobileWO as WorldObject);
		}
		if (mobileWoList.Count == 1) 
		{
			player.userInput.OpenPanel (mobileWoList[0].buttons);
		}
	}

	public bool NotOnOpenPanel()
	{
		if (GameManager.Hud.GetMainPanel().gameObject.activeSelf && (!usingMouseInput  && Input.GetTouch (0).position.x < Screen.width * 0.25f || usingMouseInput && Input.mousePosition.x <Screen.width * 0.25f))
		{
			return false;
		}
		return true;
	}

	public List<WorldObject> GetSelectedObjects()
	{
		return SelectedObjects;
	}
}
