using UnityEngine;
//using UnityEditor;
using System.Collections;

public class ConstructionPS : MonoBehaviour 
{
	private ParticleSystem[] psArray;
//	private SerializedObject[] psObjects;
	private static float diminishTime = 2f;
	private Building thisBuilding;
	public bool forceEmit;

	public void Initiate (Building newBuilding) 
	{
		thisBuilding = newBuilding;
		psArray = GetComponentsInChildren<ParticleSystem> ();
//		psObjects = new SerializedObject[psArray.Length];
		for (int i = 0; i < psArray.Length; i ++) 
		{
			psArray[i].transform.localEulerAngles = new Vector3 (HUD.xCameraRotation, 0f, 0f);
//			psObjects[i] = new SerializedObject (psArray[i]);
		}
//		Vector3 colliderExtents = thisBuilding.mainCollider.bounds.extents;
//		for (int i = 0; i < psObjects.Length; i ++) 
//		{
//			psObjects[i].FindProperty ("ShapeModule.boxX").floatValue = colliderExtents.x;
//			psObjects[i].FindProperty ("ShapeModule.boxY").floatValue = colliderExtents.y;
//			psObjects[i].FindProperty ("ShapeModule.boxZ").floatValue = colliderExtents.z;
//			psObjects[i].ApplyModifiedProperties ();
//		}
		StartCoroutine (ConstructionCoroutine ());
	}

	private IEnumerator ConstructionCoroutine () 
	{
		while ((thisBuilding.isAlive && (thisBuilding.constructing || thisBuilding.repairing)) || forceEmit) yield return null;
		float[] originalStartSizes = new float[psArray.Length];
		for (int i = 0; i < originalStartSizes.Length; i ++) 
		{
			originalStartSizes[i] = psArray[i].startSize;
		}
		for (float timer = 0f; timer < diminishTime; timer +=Time.deltaTime * 2f) 
		{
			for (int i = 0; i < originalStartSizes.Length; i ++) 
			{
				psArray[i].startSize = Mathf.Lerp (originalStartSizes[i], 0f, timer / diminishTime);
			}
			yield return null;
		}
		for (int i = 0; i < originalStartSizes.Length; i ++) 
		{
			psArray[i].enableEmission = false;
		}
		yield return new WaitForSeconds (diminishTime / 2f);
		Destroy (gameObject);
	}
}
