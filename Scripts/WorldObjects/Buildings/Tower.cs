using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Tower : Building 
{
	// [0] = Range
	public float[] rangedStatsArray;
	public ProjectileController thisPC;

	protected override void Awake ()
	{
		base.Awake ();
		attackComponent = gameObject.AddComponent<TowerAttack> ();
		attackComponent.StartFindClosestEnemy ();
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.RangedStats, rangedStatsArray);
	}
}
