using UnityEngine;
using System.Collections;

public class RockMonument : MonumentType 
{
	protected override void Awake ()
	{
		base.Awake ();
		monName = "Rock";
	}
}
