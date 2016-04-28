using UnityEngine;
using System.Collections;

public class MobileWOAttack : Attack 
{
	protected MobileWorldObject thisMobileWO;
	private Vector3 lastTargetPosition;
	protected bool adjustingPosition;
	protected Vector3 beforeAttackPosition;
	protected float chasingTimer;
	protected bool stopChasing;
	private WorldObject lastOrderedTarget;
	private float lastRecordedDistance;
	private bool recordedLastDistance;

	protected override void Awake ()
	{
		base.Awake ();
		thisMobileWO = thisWorldObject as MobileWorldObject;
	}

	public override void StartAttackCoroutine ()
	{
		stopChasing = false;
		adjustingPosition = true;
		if (!thisMobileWO.orderedToAct) 
		{
			beforeAttackPosition = transform.position;
		}
		else 
		{
			lastOrderedTarget = IHateComputers(thisMobileWO.target);
		}
		base.StartAttackCoroutine ();
//		AdjustPosition ();
	}

	protected override bool SearchCondition ()
	{
		if (stopChasing && (beforeAttackPosition - transform.position).sqrMagnitude < 4f) 
		{
			stopChasing = false;
		}
		return !thisMobileWO.orderedToAct && !stopChasing;
	}

	protected override void AttackTarget ()
	{
		if (!thisAttackStyle.InAttackRange (thisMobileWO.target) && !StopChasing())
		{
			AdjustPosition();
		}
		else if (ReadyToAttack()) 
		{
			PerformAttack();
		}
	}
	
	protected virtual bool StopChasing () 
	{
		if (!thisMobileWO.orderedToAct) 
		{
			int oldTime = (int) chasingTimer;
			chasingTimer += Time.deltaTime;
			if ((int)chasingTimer - oldTime > 0) // record distance once per second
			{
				recordedLastDistance = false;
			}
			float currentDistance = Mathf.Abs (thisMobileWO.target.transform.position.x - thisMobileWO.transform.position.x) + Mathf.Abs (thisMobileWO.target.transform.position.y - thisMobileWO.transform.position.y);
			if (!recordedLastDistance) 
			{
				lastRecordedDistance = currentDistance + 1f;
				recordedLastDistance = true;
			}
			if (currentDistance >= SearchRadius() * 1.5f && chasingTimer > 2f && currentDistance > lastRecordedDistance) 
			{
				chasingTimer = 0f;
				stopChasing = true;
				recordedLastDistance = false;
				EndAttack ();
				return true;
			}
			else if (currentDistance < SearchRadius() * 1.5f)
			{
				chasingTimer = 0f;
			}
		}
		return false;
	}

	protected void AdjustPosition() 
	{
		if (!adjustingPosition || (thisMobileWO.target.transform.position - lastTargetPosition).sqrMagnitude > Mathf.Pow ((thisMobileWO.mainCollider.bounds.extents.x + thisMobileWO.mainCollider.bounds.extents.z) / 2f, 2)) 
		{
			lastTargetPosition = thisMobileWO.target.transform.position;
			thisMobileWO.StartMoving (thisMobileWO.target.transform.position);
			adjustingPosition = true;
		}
	}

	protected override bool ReadyToAttack ()
	{
		chasingTimer = 0f;
		if (adjustingPosition) 
		{
			adjustingPosition = false; 
			thisMobileWO.StopMoving();
		}
		return base.ReadyToAttack ();
	}

	protected override void EndAttack()
	{
		base.EndAttack();
		if (thisMobileWO && thisMobileWO.isAlive)
		{
			if (!thisMobileWO.orderedToAct) 
			{
				thisMobileWO.StartMoving (beforeAttackPosition);
			}
			else if (lastOrderedTarget && !lastOrderedTarget.isAlive)
			{
				thisMobileWO.StopMoving ();
			}
		}
		lastOrderedTarget = null;
//		adjustingPosition = false;
	}

	public override void Retaliate (WorldObject attacker)
	{
		if (!thisMobileWO.orderedToAct && !thisWorldObject.attacking) 
		{
			thisWorldObject.SetTarget (attacker, false);
		}
	}

	private WorldObject IHateComputers (WorldObject thisTarget)
	{
		return thisTarget;
	}
}
