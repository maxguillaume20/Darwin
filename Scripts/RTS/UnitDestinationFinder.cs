using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitDestinationFinder : MonoBehaviour 
{
	public int unitCount;
	public float particleSizes;
	private Vector3 lastClickPos;
	private bool firstClick = true;
	private float spacing = 5f;
	private new ParticleSystem particleSystem;

	private void Awake() 
	{
		particleSystem = GetComponent<ParticleSystem> ();
	} 

	private void Update() 
	{
		if (Input.GetMouseButtonDown(0)) 
		{
			Vector3 wtp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (firstClick) 
			{
				lastClickPos = wtp;
				particleSystem.Emit(new Vector3 (wtp.x, wtp.y - 5, wtp.z), Vector3.zero, particleSizes, 1000f, Color.red);
			}
			else 
			{
				CalculateDestinations(new Vector3 (wtp.x, wtp.y - 5, wtp.z), Quaternion.LookRotation(wtp - lastClickPos));
			}
			firstClick = !firstClick;
		}
	}

	private void CalculateDestinations(Vector3 destination, Quaternion direction) 
	{
		float xPos = 0f;
		float zPos = 0f;
		float sqrtUnitCount = Mathf.Sqrt (unitCount);
		float cosTheta = Mathf.Cos (-Mathf.Deg2Rad * direction.eulerAngles.magnitude);
		float sinTheta = Mathf.Sin (-Mathf.Deg2Rad * direction.eulerAngles.magnitude);
		for(int i = 0; i < unitCount; i ++) 
		{
			// Placement
			if (i < Mathf.CeilToInt (sqrtUnitCount) * (Mathf.RoundToInt (sqrtUnitCount) - 1)) 
			{
				xPos = -spacing * (Mathf.CeilToInt (sqrtUnitCount) - 1) / 2f + (i % Mathf.CeilToInt (sqrtUnitCount)) * spacing;
			}
			else 
			{
				int remainder = unitCount - Mathf.CeilToInt (sqrtUnitCount) * (Mathf.RoundToInt (sqrtUnitCount) - 1);
				xPos = -spacing * (remainder - 1) / 2f  + (i % Mathf.CeilToInt (sqrtUnitCount)) * spacing;
			}
			zPos = spacing * (Mathf.RoundToInt (sqrtUnitCount) - 1) / 2f - (i / Mathf.CeilToInt (sqrtUnitCount)) * spacing;
			// Rotation
			float newX = cosTheta * (xPos) - sinTheta * (zPos);
			float newZ = sinTheta * (xPos) + cosTheta * (zPos);
			particleSystem.Emit( new Vector3 (newX + destination.x, 25f, newZ + destination.z), Vector3.zero, particleSizes, 10000f, Color.green);
		}
	}
}
