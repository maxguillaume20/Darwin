using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class BattleController : MouseInput 
{
	public List<WorldObject> selectedWOList = new List<WorldObject> ();

	protected override void Awake () 
	{
		base.Awake ();
		GameManager.battleController = this;
		GameManager.inBattleGround = true;
	}

	protected override void Start ()
	{

	}

	private void Update () 
	{
		HandleInput ();
	}

	public override void HandleInput () 
	{
		if (!Input.GetMouseButton(0)) 
		{
			if (Input.GetMouseButtonUp (0))
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
		else if (GameManager.FingerInBounds (Input.mousePosition))
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

//	public override void HandleInput () 
//	{
//		if (!Input.GetMouseButton(0)) 
//		{
//			if (Input.GetMouseButtonUp (0)) 
//			{
//				if (!multiSelect) 
//				{
//					ClickSelect ();
//				}
//				else 
//				{
//					MultiSelect ();
//				}
//				
//			}
//			else if (MouseOnScreen()) MoveCamera ();
//		}
//		else if (Input.GetMouseButtonDown (0)) 
//		{
//			clickedPosition = Input.mousePosition;
//		}
//		else if ((Input.mousePosition - clickedPosition).magnitude > 10f) 
//		{
//			AdjustSelectionBox ();
//		}
//		ZoomCamera (Input.GetAxis ("Mouse ScrollWheel"));
//		if (Input.GetMouseButtonUp (1)) 
//		{
//			DeselectAll ();
//		}
//	}

	protected override void ClickSelect () 
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
					if (player.units.SelectedUnitsCount() > 0 && (!worldobject.IsOwnedBy(player.species) || worldobject as StrategicPoint && player.units.selectedCaravans.Count > 0))
					{
						// move units towards a target of a different species
						player.units.MoveUnits(worldTouchPoint, worldobject);
					} 
					else 
					{
						// select new worldbject
						if (selectedWOList.Count != 0)
						{
							DeselectAll();
						}
						SelectWorldOject (worldobject);
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

	protected override void MultiSelect () 
	{
		DeselectAll ();
		SelectBox.Disable ();
		Vector3 groundClickedPosition = WorldTouchPoint (Camera.main.ScreenToWorldPoint (clickedPosition));
		Vector3 groundMousePosition = WorldTouchPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));
		float distance = Mathf.Abs (groundMousePosition.x - groundClickedPosition.x);
		float xMin = Mathf.Min (groundMousePosition.x, groundClickedPosition.x);
		float zMin = Mathf.Min (groundMousePosition.z, groundClickedPosition.z);
		float zMax = Mathf.Max (groundMousePosition.z, groundClickedPosition.z);
		RaycastHit[] hits = Physics.CapsuleCastAll (new Vector3 (xMin, 0f, zMin), new Vector3 (xMin, 0f, zMax), 0.1f, Vector3.right, distance, LayerMask.GetMask (new string[] {player.species.ToString ()}));
		foreach (RaycastHit hit in hits) 
		{
			Unit unit = hit.collider.gameObject.GetComponent<Unit> ();
			if (unit) 
			{
				SelectWorldOject (unit as WorldObject);
			}
		}
	}

	private void SelectWorldOject (WorldObject selectedWO) 
	{
		selectedWOList.Add (selectedWO);
		selectedWO.healthBar.gameObject.SetActive (true);
		selectedWO.selected = true;
		if (selectedWO as Unit && selectedWO.IsOwnedBy (player.species)) 
		{
			player.units.SelectUnit (selectedWO as Unit);
		}
	}

	protected virtual void DeselectAll () 
	{
		foreach (WorldObject selectedWO in selectedWOList) 
		{
			selectedWO.healthBar.gameObject.SetActive (false);
			selectedWO.selected = false;
		}
		selectedWOList.Clear ();
		player.units.ClearSelectedUnits ();
	}
}
