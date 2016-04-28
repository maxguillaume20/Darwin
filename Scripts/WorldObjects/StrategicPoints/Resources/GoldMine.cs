using UnityEngine;
using System.Collections;
using RTS;

public class GoldMine : Resource 
{
	protected override void MakeLocalUpgrades ()
	{
		base.MakeLocalUpgrades ();
		localUpgradesList [0] [0].costArray = new float[] {50f, 175f, 0f};
	}
}
