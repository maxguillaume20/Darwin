using UnityEngine;
using System.Collections;
using RTS;

public class TowerAttack : Attack 
{
	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override float SearchRadius ()
	{
		return thisWorldObject.statsDick[StatsType.RangedStats][0] - 1f;
	}

	protected override void AttackTarget ()
	{
		if (!thisAttackStyle.InAttackRange(thisWorldObject.target)) 
		{
			thisWorldObject.attacking = false;
		}
		else if (ReadyToAttack()) 
		{
			PerformAttack();
		}
	}
	
	protected override void PerformAttack ()
	{
		Fire ();
	}
}

