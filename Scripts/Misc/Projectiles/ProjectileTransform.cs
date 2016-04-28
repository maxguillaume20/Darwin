using UnityEngine;
using System.Collections;

public class ProjectileTransform : MonoBehaviour 
{
	public static Transform thisTransform { get; set; }

	private void Awake () 
	{
		thisTransform = transform;
	}
}
