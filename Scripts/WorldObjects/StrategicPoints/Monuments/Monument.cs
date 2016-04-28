using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Monument : StrategicPoint 
{
	private UnoccupiedMonument unoccupiedMon;

	public void Initiate (UnoccupiedMonument newUnoccupiedMon) 
	{
		transform.SetParent (StrategicPointTransform.thisTransform);
		Occupy (newUnoccupiedMon.GetPlayer (), newUnoccupiedMon.population);
		buildingArea.buildingSlots = newUnoccupiedMon.buildingArea.buildingSlots;
		foreach (BuildingSlot bS in buildingArea.buildingSlots)
		{
			bS.Initiate (this);
		}
		unoccupiedMon = newUnoccupiedMon;
	}

	// condenses the functions CreateMob and DestroyMob into one function so child monuments can do less overrides
	public virtual void ChangeMobCount (int change) 
	{
		
	}

	protected override void CreateMob() 
	{
		base.CreateMob ();
		ChangeMobCount (1);
	}

	protected override void DestroyMob() 
	{
		base.DestroyMob ();
		ChangeMobCount (-1);
	}

	public override void Revert ()
	{
		unoccupiedMon.Enable ();
		Destroy (gameObject);
	}
}
