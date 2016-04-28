using UnityEngine;
using System.Collections;

public class LumberYard : Resource 
{
	protected override void MakeLocalUpgrades ()
	{
		base.MakeLocalUpgrades ();
		localUpgradesList [0] [0].costArray = new float[] {100f, 125f, 0f};
	}
}
