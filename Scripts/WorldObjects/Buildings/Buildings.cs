using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buildings : MonoBehaviour 
{
	public List<Building> currentBuildings = new List<Building> ();

	public void AddBuilding (Building building) 
	{
		currentBuildings.Add (building);
	}

	public void RemoveBuilding( Building building) 
	{
		currentBuildings.Remove (building);
	}
}
