using UnityEngine;
using System.Collections;

public class AttackStyle : MonoBehaviour 
{
	protected WorldObject thisWorldObject;

	protected virtual void Awake () 
	{
		thisWorldObject = GetComponent<WorldObject> ();
	}

	public virtual bool InAttackRange (WorldObject target) 
	{
		return false;
	}

	public virtual void StyleFire () 
	{

	}
}
