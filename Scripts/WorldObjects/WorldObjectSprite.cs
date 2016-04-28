using UnityEngine;
using System.Collections;

public class WorldObjectSprite : MonoBehaviour 
{
	private Animator animator;

	private void Awake () 
	{
		animator = GetComponent<Animator> ();
	}

	// these two methods, FireEvent and SetEvent, are called by events in animation clips
	private void FireEvent (string methodName) 
	{
		transform.SendMessageUpwards (methodName);
	}

	// animation events do not support passing a bool as a parameter, which is why inEvent is an int 
	private void SetEvent (int inEvent) 
	{
		if (inEvent == 1) 
		{
			animator.SetBool ("InEvent", true);
		}
		else if (inEvent == 0) 
		{
			animator.SetBool ("InEvent", false);
		}
	}
}
