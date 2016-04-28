using UnityEngine;
using System.Collections;

public class StickMonument : MonumentType 
{
	protected override void Awake ()
	{
		base.Awake ();
		monName = "Stick";
	}
}
