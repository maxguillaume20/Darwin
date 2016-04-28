using UnityEngine;
using System.Collections;
using RTS;

public class ProjectileController : MonoBehaviour
{
	protected WorldObject thisRangedWO;
	protected static float gravity;

	protected virtual void Awake () 
	{
		thisRangedWO = GetComponentInParent<WorldObject> ();
	}

	protected virtual void Start () 
	{
		if (gravity == 0) 
		{
			gravity = Physics.gravity.magnitude;
		}
	}

	public void ProjectileFire () 
	{
		if (thisRangedWO.gameObject.activeSelf) 
		{
			ActuallyFire ();
		}
	}

	protected virtual void ActuallyFire () 
	{
		
	}
}
