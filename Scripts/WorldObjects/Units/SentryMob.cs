using UnityEngine;
using System.Collections;
using RTS;

public class SentryMob : MobileWorldObject 
{
	public Sentry sentryTower;
	public Vector3 sentryBase;
	private static float noTowerPatrolRadius = 15f;
	private bool isStruttin;

	protected override void Awake ()
	{
		base.Awake ();
		attackComponent = gameObject.AddComponent<SentryMobAttack> ();
	}

	protected override void Start () 
	{
		base.Start ();
		sentryBase = sentryTower.transform.position;
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		attacking = false;
		attackComponent.StartFindClosestEnemy ();
		navAgent.enabled = true;
		healthArray [0] = healthArray [1];
		if (healthBar) healthBar.ResetBar ();
		isStruttin = false;
		if (sentryTower) StartStruttin ();
	}

	public void StartStruttin () 
	{
		if (!isStruttin) 
		{
			StartCoroutine (StruttMyThang ());
		}
	}

	private IEnumerator StruttMyThang () 
	{
		isStruttin = true;
		bool firstStrutt = true;
		while (!attacking) 
		{
			if (!firstStrutt) 
			{
				float waitTime = Random.Range (5f, 10f);
				yield return new WaitForSeconds (waitTime);
			}
			else 
			{
				firstStrutt = false;
				yield return new WaitForSeconds (2f);
			}
			if (!attacking) 
			{
				Vector3 position = RandomPositionAroundTower ();
				NavMeshHit navMeshHit;
				if (NavMesh.SamplePosition(position, out navMeshHit, 1f, NavMesh.AllAreas)) 
				{
					StartMoving (position);
				}
			}
		}
		isStruttin = false;
	}

	private Vector3 RandomPositionAroundTower () 
	{
		float maxXDis = 0.2f * GetPatrolRadius ();
		float xCoord = Random.Range (-1f, 1f) * maxXDis;
		float maxZDis = Mathf.Sqrt (Mathf.Pow (maxXDis, 2) - Mathf.Pow (xCoord, 2));
		float zCoord = Random.Range (-1f, 1f) * maxZDis;
		return new Vector3 (xCoord + sentryBase.x, 0f, zCoord + sentryBase.z);
	}

	public float GetPatrolRadius () 
	{
		if (sentryTower && sentryTower.isAlive) 
		{
			return sentryTower.patrolRadius;
		}
		else 
		{
			return noTowerPatrolRadius;
		}
	}

	public override void StopMoving ()
	{
		base.StopMoving ();
		orderedToAct = false;
		if (!attacking) 
		{
			StartStruttin ();
		}
	}
//
//	public override void Die() 
//	{
//		isAlive = false;
//		attacking = false;
//		isMoving = false;
//		selected = false;
//		healthBar.gameObject.SetActive (false);
//		navAgent.enabled = false;
//		navObstacle.enabled = false;
//		gameObject.SetActive  (false);
//		transform.position = sentryTower.transform.position;
//		sentryTower.MobDied ();
//	}

	public override void SetMobTrainer (MobTrainer thisMobTrainer)
	{
		base.SetMobTrainer (thisMobTrainer);
		sentryTower = thisMobTrainer as Sentry;
	}
}
