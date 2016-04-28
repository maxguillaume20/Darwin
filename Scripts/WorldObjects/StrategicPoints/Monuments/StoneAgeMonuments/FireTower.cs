using UnityEngine;
using System.Collections;
using RTS;

public class FireTower : TowerMonument 
{
	private float attackSpeedMultiplier = 0.5f;

	public override void ChangeMobCount (int change)
	{
		base.ChangeMobCount (change);
		attackArray [1] += GameManager.baseStatsDick [name] [StatsType.Attack] [1] * attackSpeedMultiplier * change;
	}
}
