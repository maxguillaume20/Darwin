using UnityEngine;
using System.Collections;
using RTS;

public class TowerMonument : Monument
{
	// [0] = Range
	public float[] rangedStatsArray;
	public ProjectileController thisPC;
	
	protected override void Awake ()
	{
		base.Awake ();
		thisPC = GetComponentInChildren<ProjectileController> ();
		thisPC.transform.RotateAround (transform.position, Vector3.right, HUD.xCameraRotation);
		attackComponent = gameObject.AddComponent<TowerAttack> ();
		thisPC.gameObject.SetActive (false);
		attackComponent.StartFindClosestEnemy ();
	}

	public override void ChangeMobCount (int change)
	{
		if (currentMobCount > 0 && !thisPC.gameObject.activeSelf || currentMobCount <= 0 && thisPC.gameObject.activeSelf) 
		{
			attacking = false;
			if (!thisPC.gameObject.activeSelf) 
			{
				thisPC.gameObject.SetActive (true);
				attackComponent.StartFindClosestEnemy ();
			}
			else 
			{
				thisPC.gameObject.SetActive (false);
			}
		}
	}
	
	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.RangedStats, rangedStatsArray);
	}
}
