using UnityEngine;
using System.Collections;

public class NatureMonument : MonumentType 
{
	protected override void Awake ()
	{
		base.Awake ();
		monName = "Nature";
	}
}
