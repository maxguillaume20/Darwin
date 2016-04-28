using UnityEngine;
using System.Collections;
using RTS;

public class Ranged : AttackStyle 
{
	protected ProjectileController thisPC;

	protected override void Awake ()
	{
		base.Awake ();
		thisPC = GetComponentInChildren<ProjectileController> ();
	}

	public override bool InAttackRange (WorldObject target)
	{ 
		Vector3 distance = target.transform.position - transform.position;
		if (distance.sqrMagnitude <= Mathf.Pow (thisWorldObject.statsDick[StatsType.RangedStats][0], 2f))
		{
			return true;
		}
		return false;
	}

	public override void StyleFire ()
	{
		thisPC.ProjectileFire ();
	}
}
