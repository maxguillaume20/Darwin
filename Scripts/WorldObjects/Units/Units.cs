using UnityEngine;
using System.Collections.Generic;
using System;
using RTS;

public class Units : MonoBehaviour 
{
	public List<Unit> selectedUnits = new List<Unit> ();
	public List<Caravan> selectedCaravans = new List<Caravan>();
	public SquadController squadController;

	private void Awake() 
	{
		squadController = GetComponentInChildren<SquadController> ();
	}

	public void SelectUnit(Unit unit)
	{

		if (!selectedUnits.Contains(unit)) 
		{
			selectedUnits.Add (unit);
			if (unit as Caravan) selectedCaravans.Add(unit as Caravan);
		}
	}

	public void RemoveFromSelectedUnits(Unit unit)
	{
//		if (selectedUnits.Contains(unit)) 
//		{
			selectedUnits.Remove(unit);
			if(unit as Caravan) selectedCaravans.Remove(unit as Caravan);
//		}
	}

	public void MoveUnits(Vector3 wTP, WorldObject newtarget)
	{
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(wTP, out navMeshHit, 2.5f, NavMesh.AllAreas)) 
		{
			MakeSquad ();
			for (int i = 0; i < selectedUnits.Count; i ++) 
			{
				selectedUnits[i].SetTarget (newtarget, true);
				if (newtarget == null) 
				{
					selectedUnits[i].StartMoving (navMeshHit.position);
				}
			}
		}
	}

	private void MakeSquad () 
	{
		List<MobileWorldObject> mobileWOList = new List<MobileWorldObject> ();
		foreach (Unit unit in selectedUnits) mobileWOList.Add (unit as MobileWorldObject);
		squadController.MakeSquad(mobileWOList);
	}
	
	public void ClearSelectedUnits()
	{
		selectedUnits.Clear ();
		selectedCaravans.Clear ();
	}

	public int SelectedUnitsCount()
	{
		return selectedUnits.Count;
	}
}
