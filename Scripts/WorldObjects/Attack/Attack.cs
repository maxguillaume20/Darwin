using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Attack : MonoBehaviour
{
	protected WorldObject thisWorldObject;
	protected AttackStyle thisAttackStyle;
	public static float searchTimeSpacing = 1f;
	public Dictionary<float, WorldObject> closestEnemyDick = new Dictionary<float, WorldObject> ();
	protected float currRechargeTime;
	protected bool inBattle;
	private float battleTimeish;

	protected virtual void Awake ()
	{
		thisWorldObject = GetComponent<WorldObject>();
		if (thisWorldObject.statsDick.ContainsKey (StatsType.RangedStats)) 
		{
			thisAttackStyle = gameObject.AddComponent<Ranged>();
		}
		else 
		{
			thisAttackStyle = gameObject.AddComponent<Melee>();
		}
	}

	public void StartFindClosestEnemy() 
	{
		StartCoroutine (FindClosestEnemy ());
	}

	private IEnumerator FindClosestEnemy()
	{
		while (!thisWorldObject.attacking && thisWorldObject.isAlive)
		{ 
			if (SearchCondition()) 
			{
				Collider[] colliders = Physics.OverlapSphere(SearchPosition(), SearchRadius(), thisWorldObject.enemyLayerMask.value);
				if (colliders.Length > 0)
				{
					float furthestDistance = Mathf.Pow (2f * SearchRadius (), 2);
					float shortestdistance = furthestDistance;
					foreach (Collider coll in colliders)
					{
						WorldObject targetWorldobject = coll.gameObject.GetComponent<WorldObject>();
						StrategicPoint targetStratpoint = targetWorldobject as StrategicPoint;
						Vector3 disVector = targetWorldobject.transform.position - transform.position;
						if (targetWorldobject && !targetWorldobject.IsOwnedBy(thisWorldObject.GetSpecies()) && (!targetStratpoint || targetStratpoint.occupied) && !Physics.Raycast (transform.position, disVector.normalized, disVector.magnitude, LayerMask.GetMask (new string[] {"NavMesh"}))) 
						{
							float sqrDistance = disVector.sqrMagnitude;
							if (sqrDistance < shortestdistance) 
							{
								closestEnemyDick.Add (sqrDistance, targetWorldobject); 
								shortestdistance = sqrDistance;
							}
						}  
					}
					if (shortestdistance < furthestDistance)
					{
						thisWorldObject.SetTarget(closestEnemyDick[shortestdistance], false);
					}
				}
			}
			closestEnemyDick.Clear();
			yield return new WaitForSeconds(searchTimeSpacing);
		}
	}

	protected virtual bool SearchCondition() 
	{
		return true;
	}

	protected virtual Vector3 SearchPosition () 
	{
		return transform.position;
	}

	protected virtual float SearchRadius () 
	{
		return 25f;
	}

	public virtual void StartAttackCoroutine() 
	{
		if (!thisWorldObject.attacking) 
		{
			thisWorldObject.attacking = true;
			StartCoroutine (AttackCoroutine ());
			if (!inBattle) 
			{
				StartCoroutine (TotalAttackTime ());
			}
		}
	}

	private IEnumerator AttackCoroutine()
	{
		while(thisWorldObject.attacking && thisWorldObject.isAlive)
		{
			if (thisWorldObject.target && thisWorldObject.target.isAlive && thisWorldObject.target.gameObject.activeSelf) 
			{
				currRechargeTime += Time.deltaTime;
				AttackTarget();
			}
			else thisWorldObject.attacking = false;
			yield return null;
		}
		EndAttack();
	}

	protected virtual void AttackTarget() 
	{

	}

	protected virtual void EndAttack() 
	{
		if (thisWorldObject && thisWorldObject.isAlive) 
		{
			thisWorldObject.attacking = false;
			thisWorldObject.target = null;
			StartFindClosestEnemy ();
		}
	}
//
//	protected virtual bool InAttackRange(WorldObject newTarget) 
//	{
//		return false;
//	}

	protected virtual bool ReadyToAttack() 
	{
//		currRechargeTime += Time.deltaTime;
		if (currRechargeTime >= 1f / thisWorldObject.attackArray[1]) 
		{
			currRechargeTime = 0f;
			return true;
		}
		return false;
	}

	protected virtual void PerformAttack() 
	{
		thisWorldObject.StartAttackAnimation ();
	}

	public virtual void Retaliate (WorldObject attacker) 
	{

	}

	protected void Fire () 
	{
		if (thisWorldObject.target) 
		{
			thisAttackStyle.StyleFire();
		}
	}

	// Eventually this will be handled in part by the squadController
	private IEnumerator TotalAttackTime () 
	{
		inBattle = true;
		float notAttackingTimer = 0f;
		float longGapTime = 4f;
		float shortGapTime = 1f;
		while (inBattle) 
		{
			yield return new WaitForSeconds (longGapTime);
			battleTimeish += longGapTime;
			for (float time = 0f; time < shortGapTime; time += Time.deltaTime) 
			{
				battleTimeish += Time.deltaTime;
				if (!thisWorldObject.attacking) 
				{
					notAttackingTimer += Time.deltaTime;
				}
				else 
				{
					notAttackingTimer = 0f;
				}
				yield return null;
			}
			if (notAttackingTimer > 5f) 
			{
				battleTimeish -= notAttackingTimer * longGapTime / shortGapTime;
				inBattle = false;
			}
		}
		if (thisWorldObject.isAlive) 
		{
//			print ("We WOOnnn " + name + " " + battleTimeish);
		}
	}
}
