using UnityEngine;
using System.Collections;

public class SentryMobAttack : MobileWOAttack 
{
	protected SentryMob thisSentryMob;

	protected override void Awake ()
	{
		base.Awake ();
		thisSentryMob = thisMobileWO as SentryMob;
	}

	protected override Vector3 SearchPosition ()
	{
		return thisSentryMob.sentryBase;
	}

	protected override float SearchRadius ()
	{
		return thisSentryMob.GetPatrolRadius ();
	}

	protected override void EndAttack() 
	{
		if (thisWorldObject && thisWorldObject.isAlive) 
		{
			thisWorldObject.attacking = false;
			thisWorldObject.target = null;
			if (!stopChasing) 
			{
				StartFindClosestEnemy ();
				if (!thisWorldObject.attacking) 
				{
					thisSentryMob.StartStruttin ();
				}
			}
		}
		adjustingPosition = false;
	}

	protected override bool StopChasing ()
	{
		chasingTimer += Time.deltaTime;
		float sqrDistanceFromTower = (thisSentryMob.target.transform.position - thisSentryMob.sentryBase).sqrMagnitude;
		if (sqrDistanceFromTower > Mathf.Pow (thisSentryMob.GetPatrolRadius() * 1.33f, 2f)) 
		{
			chasingTimer = 0f;
			stopChasing = true;
			thisMobileWO.orderedToAct = true;
			EndAttack ();
			return true;
		}
		return false;	
	}
}
