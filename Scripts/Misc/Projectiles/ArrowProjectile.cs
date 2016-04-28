using UnityEngine;
using System.Collections;

public class ArrowProjectile : Projectile 
{
	public override void Fly (Vector3 newVelocity, float timeOfFlight)
	{
		rigidBody.angularVelocity = Vector3.zero;
		base.Fly (newVelocity, timeOfFlight);
		// Calculates torque vector to rotate arrow 90 degrees along it's flight path
		Vector3 endDirection = (rigidBody.velocity + Physics.gravity * timeOfFlight).normalized;
		Vector3 axisOfRotation = Vector3.Cross (transform.forward, endDirection).normalized;
		Vector3 deltaAngularVelocity = axisOfRotation * Mathf.PI / (2f * timeOfFlight);
		Quaternion q = transform.rotation * rigidBody.inertiaTensorRotation;
		Vector3 torque = q * Vector3.Scale(rigidBody.inertiaTensor, (Quaternion.Inverse(q) * deltaAngularVelocity));
		rigidBody.AddTorque (torque, ForceMode.Impulse);
	}
}
