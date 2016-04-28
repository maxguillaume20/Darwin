using UnityEngine;
using System.Collections;

public class Melee : AttackStyle 
{
	public override bool InAttackRange (WorldObject target)
	{
		if (thisWorldObject.mainCollider.bounds.Intersects(target.mainCollider.bounds))
		{
			return true;
		}
		return false;
	}

	public override void StyleFire ()
	{
		thisWorldObject.target.Attack (thisWorldObject);
	}
}
