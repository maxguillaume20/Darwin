using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Aura : MonoBehaviour 
{
	public WorldObject target;
	public float timer;
	public float duration;
	public Species species;

	protected virtual void Start() 
	{
		transform.position = target.transform.position;
		transform.localScale = target.transform.localScale;
	}

	protected virtual void Update() 
	{
		if (timer > 0) 
		{
			timer -= Time.deltaTime;
		}
		else 
		{
			EndAura();
			Destroy(gameObject);
		}
	}

	protected virtual void EndAura() 
	{
		Destroy (gameObject);
	}

	public virtual void RestorePrefab() 
	{

	}
}
