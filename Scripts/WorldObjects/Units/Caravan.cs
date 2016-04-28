using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Caravan : Unit 
{
	public override void SetTarget (WorldObject newTarget, bool ordered)
	{
		base.SetTarget (newTarget, ordered);
		StrategicPoint stratPoint = newTarget as StrategicPoint;
		if (stratPoint && (!stratPoint.occupied || stratPoint.IsOwnedBy (GetSpecies()))) 
		{
			StartCoroutine (TargetStratPoint());
		}
	}

	private IEnumerator TargetStratPoint () 
	{
		yield return null;
		Deselect ();
		player.userInput.SelectedObjects.Remove (this as WorldObject);
		player.units.selectedUnits.Remove (this as Unit);
		StrategicPoint stratPoint = target as StrategicPoint;
		StartMoving (stratPoint.transform.position);
		while (target && target as StrategicPoint == stratPoint && (!mainCollider.bounds.Intersects(target.mainCollider.bounds) || stratPoint.occupied && !stratPoint.IsOwnedBy (GetSpecies()))) 
		{
			yield return null;
		}
		if (target && target as StrategicPoint == stratPoint && mainCollider.bounds.Intersects(target.mainCollider.bounds)) 
		{
			if (selected)
			{
				Deselect();
				player.userInput.SelectedObjects.Remove(this as WorldObject);
				player.units.RemoveFromSelectedUnits(this as Unit);
			}
			if (stratPoint.occupied) 
			{
				stratPoint.ChangeLocalPopulation(currPopCount);
			}
			else 
			{
				stratPoint.Occupy(player, currPopCount);
			}
			player.RemoveFromWOsDick (this as WorldObject);
			Destroy(gameObject);
		}
	}

//	public override void Die ()
//	{
//		base.Die ();
//		if (selected) player.units.selectedCaravans.Remove(this);
//	}

	public override void ChangePopCount (int amount)
	{
		currPopCount += amount;
		if (currPopCount <= 0) 
		{
			healthArray[0] = 0f;
			healthBar.ChangeHP (healthArray [0]);
			Die ();
		}
	}
}
