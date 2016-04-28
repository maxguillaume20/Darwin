using UnityEngine;
using System.Collections.Generic;
using RTS;

public class MeshProjectileController : ProjectileController
{
	public string projectileName;
	private Queue<Projectile> pQueue = new Queue<Projectile> ();
	public float groundLaunchAngle;
	private float yComponent;
	private float cosLaunchAngle;

	protected override void Awake ()
	{
		base.Awake ();
		groundLaunchAngle *= Mathf.Deg2Rad;
		yComponent = Mathf.Sin (groundLaunchAngle) * Mathf.Cos (GameManager.gravityAngle);
		cosLaunchAngle = Mathf.Cos (groundLaunchAngle);
	}

	protected override void ActuallyFire ()
	{
		if (pQueue.Count == 0) 
		{
			MakeNewProjectile ();
		}
		// Calculate projectile velocity
		Vector3 disVector = thisRangedWO.target.transform.position - transform.position;
		float horizontalDis = Mathf.Sqrt (Mathf.Pow (disVector.x, 2f) + Mathf.Pow (disVector.z, 2f));
		float launchAxisAngle = Mathf.Atan (disVector.y / horizontalDis);
		float relativeLaunchAngle = groundLaunchAngle - launchAxisAngle;
		float something1 = Mathf.Sin (2f * relativeLaunchAngle) / (gravity * Mathf.Cos (launchAxisAngle));
		float something2 = (2f * Mathf.Sin (launchAxisAngle) * Mathf.Pow (Mathf.Sin (relativeLaunchAngle), 2f)) / (gravity * Mathf.Pow (Mathf.Cos (launchAxisAngle), 2f));
		float pSpeed = Mathf.Sqrt (horizontalDis / Mathf.Abs(something1 - something2));
		float timeOfFlight = 2 * pSpeed * Mathf.Sin (relativeLaunchAngle) / (gravity * Mathf.Cos (launchAxisAngle));
		float zAdjustment = -Physics.gravity.z * timeOfFlight / (2f * pSpeed);
		Vector3 horizontalPos = new Vector3 (transform.position.x, thisRangedWO.transform.position.y, transform.position.z);
		Vector3 direction = (thisRangedWO.target.transform.position - horizontalPos).normalized;
		Vector3 pVelocity = new Vector3 (direction.x * cosLaunchAngle, yComponent, direction.z * cosLaunchAngle + zAdjustment) * pSpeed;
		pQueue.Dequeue ().Fly (pVelocity, timeOfFlight);
	}

	private void MakeNewProjectile ()
	{
		Projectile p = ((GameObject)Instantiate(GameManager.GetGameObject(projectileName))).GetComponent<Projectile> ();
		p.shooter = thisRangedWO;
		p.gameObject.layer = LayerMask.NameToLayer (thisRangedWO.GetSpecies().ToString() + "Projectile");
		p.controller = this;
		p.transform.SetParent (ProjectileTransform.thisTransform);
		pQueue.Enqueue(p);
		p.gameObject.SetActive (false);
	}

	public void EnqueueProjectile (Projectile oldP) 
	{
		pQueue.Enqueue (oldP);
	}
}
