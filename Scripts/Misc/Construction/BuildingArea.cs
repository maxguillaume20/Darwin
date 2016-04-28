using UnityEngine;
using System;
using System.Collections.Generic;
using RTS;

public class BuildingArea : MonoBehaviour 
{
	private static float searchRadius = 20f;
	public List<BuildingSlot> buildingSlots = new List<BuildingSlot> ();
	
	public void Initiate () 
	{
		StrategicPoint stratPoint = GetComponentInParent<StrategicPoint> ();
		Collider[] colliders = Physics.OverlapSphere (transform.position, searchRadius, LayerMask.GetMask (new string[] {"BuildingArea"}));
		foreach (Collider cL in colliders) 
		{
			BuildingSlot bS = cL.GetComponent<BuildingSlot> ();
			Vector3 disVector = cL.transform.position - transform.position;
			if (bS && !Physics.Raycast (transform.position, disVector.normalized, disVector.magnitude, LayerMask.GetMask (new string[] {"NavMesh"}))) 
			{
				buildingSlots.Add (bS);
				bS.Initiate (stratPoint);
			}
		}
		gameObject.SetActive (false);
	}

	private void OnEnable () 
	{
		foreach (BuildingSlot bS in buildingSlots) 
		{
			if (bS.isActive && !bS.isOccupied) 
			{
				bS.gameObject.SetActive (true);
			}
		}
	}

	private void OnDisable () 
	{
		foreach (BuildingSlot bS in buildingSlots) 
		{
			bS.gameObject.SetActive (false);
		}
	}

	public void SetSpecies (Species species) 
	{
		foreach (BuildingSlot bS in buildingSlots) 
		{
			bS.species = species;
		}
	}

	public Species GetSpecies () 
	{
		foreach (BuildingSlot bS in buildingSlots) 
		{
			return bS.species;
		}
		return Species.NonPlayer;
	}

	public List<T> GetBuildingsList<T> () 
	{
		List<T> buildingsList = new List<T> ();
		foreach (BuildingSlot bS in buildingSlots) 
		{
			if (bS.currBuilding != null) 
			{
				T thisType = bS.currBuilding.GetComponent<T>();
				if (bS.isActive && bS.isOccupied && thisType != null) 
				{
					buildingsList.Add (thisType);
				}
			}
		}
		return buildingsList;
	}
}

