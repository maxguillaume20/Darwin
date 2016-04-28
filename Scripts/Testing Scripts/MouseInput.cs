using UnityEngine;
using System.Collections;
using RTS;
using System.Collections.Generic;

public class MouseInput : MonoBehaviour
{
	private float camMoveSpeed = 0.5f;
	private float camMoveBoundary = 0.05f;
	private float camRotAdjustment;
	protected Vector2 screenCenter;
	public Player player;
	protected Vector3 clickedPosition;
	private float zoomScale = 15;
	private int zoomMin = 1;
	private int zoomMax = 100;

	protected virtual void Awake () 
	{
		screenCenter = new Vector2 (Screen.width / 2f, Screen.height / 2f);
	}

	protected virtual void Start () 
	{
		player = GetComponent<Player>();
	}


	public virtual void HandleInput () 
	{
		// TODO rewrite this function, the logic sucks 
		player.userInput.touchTime += Time.deltaTime;
		if (!Input.GetMouseButton(0)) 
		{
			if (Input.GetMouseButtonUp (0) && GameManager.FingerInBounds (Input.mousePosition) && player.userInput.NotOnOpenPanel() && player.userInput.touchTime > 0.1f) 
			{
				if (!SelectBox.isActive) 
				{
					ClickSelect ();
				}
				else 
				{
					MultiSelect ();
				}
			}
			MoveCamera ();
			if (SelectBox.isActive) 
			{
				SelectBox.Disable ();
			} 
		}
		else if (GameManager.FingerInBounds (Input.mousePosition) && player.userInput.NotOnOpenPanel()) 
		{
			if (Input.GetMouseButtonDown (0))
			{
				clickedPosition = Input.mousePosition;
			}
			else if ((Input.mousePosition - clickedPosition).sqrMagnitude > 100f) 
			{
				AdjustSelectionBox ();
			}
		}
		if (Input.GetAxis("Mouse ScrollWheel") != 0f) ZoomCamera (Input.GetAxis ("Mouse ScrollWheel"));
	}	

	public void MoveCamera () 
	{
		if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width || Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height) 
		{
			Vector2 mousePosition = Input.mousePosition;
			Vector2 direction = (mousePosition - screenCenter).normalized;
			float xPos = Camera.main.transform.position.x;
			float zPos = Camera.main.transform.position.z;
			Camera.main.transform.position = new Vector3 (xPos + direction.x * camMoveSpeed, GameManager.cameraHeight, zPos + direction.y * camMoveSpeed);
		}
	}

	protected virtual void ClickSelect () 
	{
		Vector3 rayStartPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 worldTouchPoint = WorldTouchPoint (rayStartPoint);
		RaycastHit[] HitObjects = Physics.RaycastAll(rayStartPoint, Camera.main.transform.forward, 100f, GameManager.woLayerMask.value);
		if (HitObjects.Length > 0) 
		{
			foreach (RaycastHit hitObject in HitObjects)
			{
				WorldObject worldobject = hitObject.collider.gameObject.GetComponent<WorldObject>();
				if (worldobject)
				{
					if (player.units.SelectedUnitsCount() > 0 && (!worldobject.IsOwnedBy(player.species) && (worldobject as StrategicPoint == null || (worldobject as StrategicPoint).occupied) || worldobject as StrategicPoint && player.units.selectedCaravans.Count > 0))
					{
						// move units towards a target of a different species
						player.units.MoveUnits(worldTouchPoint, worldobject);
					} 
					else 
					{
						// select new worldbject
						if (player.userInput.SelectedObjects.Count != 0)
						{
							player.userInput.Deselect();
						}
						player.userInput.SelectObject(hitObject);
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

	protected virtual void MultiSelect () 
	{
		player.userInput.Deselect ();
		SelectBox.Disable ();
		Vector3 groundClickedPosition = WorldTouchPoint (Camera.main.ScreenToWorldPoint (clickedPosition));
		Vector3 groundMousePosition = WorldTouchPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));
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

	protected void AdjustSelectionBox () 
	{
		if (!SelectBox.isActive) 
		{
			SelectBox.Enable ();
		}
		Vector3 worldClickedPosition = Camera.main.ScreenToWorldPoint (clickedPosition);
		Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float xMin = Mathf.Min (worldMousePosition.x, worldClickedPosition.x);
		float xMax = Mathf.Max (worldMousePosition.x, worldClickedPosition.x);
		float yMin = Mathf.Min (worldMousePosition.y, worldClickedPosition.y);
		float yMax = Mathf.Max (worldMousePosition.y, worldClickedPosition.y);
		float zMin = Mathf.Min (worldMousePosition.z, worldClickedPosition.z);
		float zMax = Mathf.Max (worldMousePosition.z, worldClickedPosition.z);
		SelectBox.AdjustSize (new Vector3 (xMin, yMin, zMin), new Vector3 (xMax, yMax, zMax));
	}

	protected Vector3 WorldTouchPoint (Vector3 inputPoint) 
	{
		float camRotAdjustment = Mathf.Tan (Mathf.Deg2Rad * (90f - Camera.main.transform.rotation.eulerAngles.x)) * inputPoint.y;
		return new Vector3 (inputPoint.x, 0f, inputPoint.z + camRotAdjustment);
	}

	protected void ZoomCamera (float zoomAmount) 
	{
		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize + zoomAmount * zoomScale, zoomMin, zoomMax);
	}
}
