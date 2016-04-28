using UnityEngine;
using System.Collections;
using RTS;

public class MobileWorldObject : WorldObject
{
	// [0] = movement speed
	public float[] mobileStatsArray;
	public bool isMoving;
	protected Vector3 curSteeringTarget;
	protected NavMeshAgent navAgent;
	protected NavMeshObstacle navObstacle;
	private float maximumNavAgentRadius;
	private float miniaturizedNavRadius = 0.01f;
	private static float increaseNavRadiusTime = 1f;
	private bool increasingNavRadius;
	public int squadInt;
	private Vector3 lastPosition;
	public bool orderedToAct;
	protected MobTrainer mobTrainer;
//	private static float spriteYOffset = 0.15f;

	protected override void Awake ()
	{
		base.Awake ();
		navAgent = GetComponent<NavMeshAgent> ();
		navObstacle = GetComponent<NavMeshObstacle> ();
		maximumNavAgentRadius = navAgent.radius;
//		spriteRenderer.transform.position = new Vector3 (spriteRenderer.transform.position.x, spriteYOffset, spriteRenderer.transform.position.z);
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		isMoving = false;
		animator.SetBool ("IsMoving", isMoving);
		increasingNavRadius = false;
		navAgent.enabled = true;
		navAgent.radius = miniaturizedNavRadius;
		navAgent.avoidancePriority = 51;
		navAgent.ResetPath ();
	}

	public override void SetTarget (WorldObject newTarget, bool ordered)
	{
		orderedToAct = ordered;
		base.SetTarget (newTarget, ordered);
	}

	public void PauseNavAgent (bool pause) 
	{
		if (pause) 
		{
			navAgent.speed = 0f;
		}
		else 
		{
			navAgent.speed = mobileStatsArray[0];
		}
	}

	public void SetNavAgentDestination(Vector3 position, float sampSearchSpace) 
	{
		NavMeshHit navMeshHit;
		if (NavMesh.SamplePosition(position, out navMeshHit, sampSearchSpace, NavMesh.AllAreas)) 
		{
			StartMoving (navMeshHit.position);
		}
	}

	public virtual void StartMoving (Vector3 newDestination) 
	{
		if (!isMoving) 
		{
			StartCoroutine(Move(newDestination)); 
		}
		else if (navAgent.enabled)
		{
			navAgent.SetDestination(newDestination);
		}
		AdjustNavAgentRadius ();
	}

	private void AdjustNavAgentRadius() 
	{
		if (attacking) 
		{
			StartIncreaseNavRadius ();
		}
		else 
		{
			navAgent.radius = miniaturizedNavRadius;
		}
	}
	
	public void SetMovingNavAgentSpeed () 
	{
		if (isMoving) 
		{
			navAgent.speed = mobileStatsArray [0];
		}
	}

	private IEnumerator Move(Vector3 startingDestination) 
	{
		isMoving = true;
		if (navObstacle) navObstacle.enabled = false;
		yield return new WaitForSeconds (0.1f);
		if (isMoving) 
		{
			navAgent.enabled = true;
			navAgent.speed = mobileStatsArray[0];
			navAgent.avoidancePriority = 49;
			navAgent.SetDestination (startingDestination);
			animator.SetBool ("IsMoving", isMoving);
//			animator.enabled = true;
			curSteeringTarget = transform.position; 
			lastPosition = Vector3.zero;
			while (isMoving && isAlive)
			{
				if (curSteeringTarget != navAgent.steeringTarget) 
				{
					if (navAgent.steeringTarget == navAgent.destination) 
					{
						SetFormation ();
					}
					curSteeringTarget = navAgent.steeringTarget;
					SetAnimator(curSteeringTarget);
				}
				// something is going on here..
//				if (!navAgent.enabled) 
//				{
//					print (name);
//				}
				if (transform.position == navAgent.destination || !navAgent.pathPending && lastPosition == transform.position && navAgent.enabled && navAgent.remainingDistance < 0.5f) 
				{
					StopMoving();
				}
				lastPosition = transform.position;
				yield return null;
			}
		}
	}

	private void SetFormation () 
	{
		if (attacking) 
		{
			StartCoroutine (IncreaseNavRadius ());
		}
		if (squadInt > 0) 
		{
			player.units.squadController.SetFormation(squadInt, navAgent.destination, curSteeringTarget, target);
		}
	}

	public virtual void StopMoving() 
	{
		isMoving = false;
		animator.SetBool ("IsMoving", isMoving);
		if (navAgent.enabled) 
		{
			navAgent.ResetPath ();
		}
		if (attacking) 
		{
			SetAnimator (target.transform.position);
			navAgent.enabled = false;
			navObstacle.enabled = true;
		}
		else 
		{
			orderedToAct = false;
			if (squadInt > 0) 
			{
				player.units.squadController.SquadStop (squadInt);
			}
			else 
			{
				StartIncreaseNavRadius ();
			}
		}
	}

	protected override void SetAnimator (Vector3 steeringTarget)
	{
		if (!animator.enabled) 
		{
			animator.enabled = true;
		}
		float xx = steeringTarget.x - transform.position.x;
		float direction = (steeringTarget.z - transform.position.z) / Mathf.Abs (xx);
		if ((direction >= 1 || direction <= -1)) 
		{
			hasVerticalAnimator = true;
			spriteParent.transform.localEulerAngles = new Vector3 (HUD.xCameraRotation, 0f, 0f);
			int zDirection = (int)((steeringTarget.z - transform.position.z) / Mathf.Abs (steeringTarget.z - transform.position.z));
			animator.SetInteger ("Direction", (1 - zDirection));
		}
		else
		{
			hasVerticalAnimator = false;
			animator.SetInteger("Direction", 1);
			if (xx < 0 && spriteParent.transform.localEulerAngles.y < 100f || xx > 0f && spriteParent.transform.localEulerAngles.y > 100f)
			{
				spriteParent.transform.Rotate (new Vector3 (0f, 180f, 0f));
			}
		}
	}

	public virtual void EnableNavObstacle () 
	{
//		navAgent.avoidancePriority = 48;
//		navAgent.enabled = true;
	}

	public void StartIncreaseNavRadius () 
	{
		if (!increasingNavRadius) StartCoroutine (IncreaseNavRadius ());
	}

	private IEnumerator IncreaseNavRadius () 
	{
		increasingNavRadius = true;
		if (!isMoving) 
		{
			navAgent.speed = 0f;
		}
		for (float time = (navAgent.radius - miniaturizedNavRadius) / (maximumNavAgentRadius - miniaturizedNavRadius) * increaseNavRadiusTime; time < increaseNavRadiusTime && (!isMoving || attacking) && gameObject.activeSelf; time += Time.deltaTime) 
		{
			IncreaseNavRadius (time / increaseNavRadiusTime);
			yield return null;
		}
		increasingNavRadius = false;
		navAgent.speed = mobileStatsArray[0];
		navAgent.radius = maximumNavAgentRadius;
		if (isAlive && !isMoving && !attacking && navAgent.enabled) 
		{
			navAgent.ResetPath();
		}
	}

	protected virtual void EndIncreaseNavRadius () 
	{
//		navAgent.avoidancePriority = 51;
//		navAgent.enabled = false;
	}

	private void IncreaseNavRadius(float ratio) 
	{
		if (ratio >= 0f && ratio <= 1f) 
		{
			navAgent.radius = Mathf.Lerp(miniaturizedNavRadius, maximumNavAgentRadius, ratio);
		}
	}

	public virtual void SetMobTrainer (MobTrainer thisMobTrainer) 
	{
		mobTrainer = thisMobTrainer;
	}

	public override void Die ()
	{
		if (isAlive) 
		{
			isMoving = false;
			increasingNavRadius = false;
			orderedToAct = false;
			navAgent.enabled = false;
			if (navObstacle) 
			{
				navObstacle.enabled = false;
			}
			if (player) 
			{
				player.units.squadController.RemoveUnitFromCurrentSquad(this);
			}
			if (mobTrainer && mobTrainer.isAlive) 
			{
				mobTrainer.MobDied ();
			}
		}
		base.Die ();
		if (!destroyingGameObject && (mobTrainer == null || !mobTrainer.isAlive))
		{
			StartDestroyGameObject ();
		}
	}

//	protected override void StartDestroyGameObject ()
//	{
//		base.StartDestroyGameObject ();
//		navObstacle.enabled = false;
//		navAgent.enabled = false;
//	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.MobileStats, mobileStatsArray);
	}
}
