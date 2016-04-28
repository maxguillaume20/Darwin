using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Projectile : MonoBehaviour
{
	protected Rigidbody rigidBody;
	public MeshProjectileController controller;
	public WorldObject shooter;
	protected bool isDisabled; 
	private static float tofExtension = 0.1f;
	
	private void Awake () 
	{
		rigidBody = GetComponent<Rigidbody> ();
		isDisabled = true;
	}

	public virtual void Fly (Vector3 newVelocity, float timeOfFlight) 
	{
		isDisabled = false;
		transform.position = controller.transform.position;
		transform.forward = newVelocity.normalized;
		gameObject.SetActive (true);
		rigidBody.velocity = newVelocity;
		StartCoroutine (FlyCoroutine (timeOfFlight));
	}

	private IEnumerator FlyCoroutine (float timeOfFlight) 
	{
		yield return new WaitForSeconds (timeOfFlight + tofExtension);
		Disable ();
	}

	private void OnCollisionEnter (Collision coll) 
	{
		WorldObject worldObject = coll.gameObject.GetComponent<WorldObject> ();
		if (worldObject && !isDisabled) 
		{
			HitWorldObject(worldObject);
			Disable ();
		}
	}

	protected virtual void HitWorldObject (WorldObject worldObject)
	{
		worldObject.Attack(shooter);
	}

	private void Disable() 
	{
		isDisabled = true;
		gameObject.SetActive (false);
		if (controller) 
		{
			controller.EnqueueProjectile (this);
		}
		else 
		{
			Destroy (gameObject);
		}
	}
}
