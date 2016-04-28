using UnityEngine;
using System.Collections;
using RTS;

public class ResourceMonument : Monument 
{
	//[0] = gatheringAmount, [1] = gatheringTime
	public float[] resourceStatsArray;
	public ResourceType resourceType;
//	protected float gatheringTime;
//	protected float gatheringAmount;
	private bool gatheringResources;

	public override void ChangeMobCount (int change)
	{
		if (!gatheringResources) 
		{
			StartCoroutine (GatherResources());
		}
	}

	private IEnumerator GatherResources() 
	{
		gatheringResources = true;
		while (occupied && currentMobCount > 0) 
		{
			yield return new WaitForSeconds (resourceStatsArray[1]);
			if (occupied && GetCurrentUnits() > 0) 
			{
				GameManager.playersDick[GetSpecies()].ChangeResource (resourceType, resourceStatsArray[0] * currentMobCount);
			}
		}
		gatheringResources = false;
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.ResourceStats, resourceStatsArray);
	}
}
