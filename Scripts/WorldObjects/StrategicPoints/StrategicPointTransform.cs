using UnityEngine;
using System.Collections;
using RTS;

public class StrategicPointTransform : MonoBehaviour 
{
	public static float revertWaitTime = 3f;
	private static StrategicPointTransform thisSPTransform;
	public static Transform thisTransform;

	private void Awake () 
	{
		thisTransform = transform;
		thisSPTransform = this;
	}
	
	public static void StartRevertCoroutine (StrategicPoint stratPoint) 
	{
		thisSPTransform.StartCoroutine (RevertCoroutine (stratPoint));
	}

	private static IEnumerator RevertCoroutine (StrategicPoint stratPoint) 
	{
		ConstructionPS conPS = ((GameObject)Instantiate (GameManager.GetGameObject ("ConstructionPS"), stratPoint.transform.position, Quaternion.identity)).GetComponent<ConstructionPS> (); 
		conPS.forceEmit = true;
		conPS.Initiate (stratPoint);
		yield return new WaitForSeconds (revertWaitTime);
		stratPoint.Revert ();
		conPS.forceEmit = false;
	}
}
