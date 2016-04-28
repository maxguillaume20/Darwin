using UnityEngine;
using System.Collections;
using RTS;

public class Trebuchet : RangedUnit 
{
	private static Vector3 horNavObSize { get {return new Vector3 (7.5f, 1f, 3f); } }
	private static Vector3 verNavObSize { get {return new Vector3 (3f, 1f, 6f); } }
	private static float horCapCollHeight = 11f;
	private static float verCapCollHeight = 10f;
	private CapsuleCollider capCollider;

	protected override void Awake ()
	{
		base.Awake ();
		capCollider = GetComponent<CapsuleCollider> ();
	}


	protected override void SetAnimator (Vector3 steeringTarget)
	{
		bool hadVerticalAnimator = hasVerticalAnimator;
		base.SetAnimator (steeringTarget);
		if (!hadVerticalAnimator && hasVerticalAnimator) 
		{
			capCollider.direction = 2;
			capCollider.height = verCapCollHeight;
			navObstacle.size = verNavObSize;
		}
		else if (hadVerticalAnimator && !hasVerticalAnimator) 
		{
			capCollider.direction = 0;
			capCollider.height = horCapCollHeight;
			navObstacle.size = horNavObSize;
		}
	}
}
