using UnityEngine;
using System.Collections;

public class CarrotProjectile : Projectile 
{
	protected override void HitWorldObject (WorldObject worldObject)
	{
		base.HitWorldObject (worldObject);
		if (!worldObject || !worldObject.isAlive) 
		{
			GiantCarrot giantCarrot = shooter as GiantCarrot;
			giantCarrot.IncreaseAttackSpeed();
		}
	}
}
