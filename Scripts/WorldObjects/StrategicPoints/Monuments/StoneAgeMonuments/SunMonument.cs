using UnityEngine;
using System.Collections;

public class SunMonument : MonumentType 
{
	protected override void Awake ()
	{
		base.Awake ();
		monName = "Sun";
	}
}
