using UnityEngine;
using System.Collections;

public class ParticleProjectileController : ProjectileController
{
	protected ParticleSystem thisParticleSystem;
	public int projectileSpeed;

	protected override void Awake () 
	{
		base.Awake ();
		thisParticleSystem = GetComponent<ParticleSystem> ();
	}
	
	private void OnParticleCollision (GameObject other) 
	{
		WorldObject hitWO = other.GetComponent<WorldObject> ();
		if (hitWO) 
		{
			hitWO.Attack (thisRangedWO);
		}
	}
	
	protected override void ActuallyFire () 
	{
		Vector3 pVelocity = (thisRangedWO.target.transform.position - transform.position).normalized * projectileSpeed;
		float pLifeTime = thisRangedWO.statsDick[RTS.StatsType.RangedStats][0] / projectileSpeed;
		thisParticleSystem.Emit (transform.position, pVelocity, thisParticleSystem.startSize, pLifeTime, thisParticleSystem.startColor);
	}
}
