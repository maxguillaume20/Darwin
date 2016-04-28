using UnityEngine;
using System.Collections;

public class BuildingSlot : MonoBehaviour 
{
	public bool isActive;
	public bool isOccupied;
	public RTS.Species species;
	public StrategicPoint stratPoint;
	public Building currBuilding;
	private SpriteRenderer spRenderer;

	public void Initiate (StrategicPoint stratPoint) 
	{
		this.stratPoint = stratPoint;
		if (gameObject.activeSelf) 
		{
			spRenderer = GetComponentInChildren<SpriteRenderer> ();
			spRenderer.transform.localEulerAngles = new Vector3 (HUD.xCameraRotation, spRenderer.transform.localEulerAngles.y, spRenderer.transform.localEulerAngles.z);
			gameObject.SetActive (false);
		}
	}

	public void SetOccupation (bool isOccupied) 
	{
		this.isOccupied = isOccupied;
		if (!isOccupied) 
		{
			currBuilding = null;
		}
//		spRenderer.sprite = BuildMenu.SetBuildingSlotSprite (isOccupied);
	}

	public void SetActive (bool isActive) 
	{
		this.isActive = isActive;
		gameObject.SetActive (isActive);
	}
}
