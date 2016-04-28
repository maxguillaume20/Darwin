using UnityEngine;
using System.Collections.Generic;
using RTS;

public class RangedUnit : Unit 
{
	// [0] = Range
	public float[] rangedStatsArray;
	public ProjectileController thisPC;
	public Vector2[] pcPositions;

	protected override void Awake ()
	{
		base.Awake ();
		thisPC = GetComponentInChildren<ProjectileController> ();
	}

	protected override void SetAnimator (Vector3 steeringTarget)
	{
		base.SetAnimator (steeringTarget);
		if (pcPositions.Length > 0) 
		{
			thisPC.transform.localPosition = pcPositions[animator.GetInteger("Direction")];
			if (animator.GetInteger("Direction") == 1 && steeringTarget.x < transform.position.x) 
			{
				thisPC.transform.Translate (new Vector3 (thisPC.transform.localPosition.x * -2f, 0f, 0f));
			}
			thisPC.transform.RotateAround (transform.position, Vector3.right, HUD.xCameraRotation);
		}
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.RangedStats, rangedStatsArray);
	}
}
