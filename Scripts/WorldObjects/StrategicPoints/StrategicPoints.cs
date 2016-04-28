using UnityEngine;
using System.Collections.Generic;

public class StrategicPoints : MonoBehaviour 
{
	public List<StrategicPoint> ownedAllList;
	public List<Resource> ownedResourceList;

	public void AddStratPt(StrategicPoint newstratpt)
	{
		ownedAllList.Add (newstratpt);
		Resource newResource = newstratpt as Resource;
		if (newResource) 
		{
			ownedResourceList.Add(newResource);
		}
	}

	public void RemoveStratPt(StrategicPoint stratPt) 
	{
		ownedAllList.Remove (stratPt);
		Resource resource = stratPt as Resource;
		if (resource) 
		{
			ownedResourceList.Remove(resource);
		}
	}

	public bool ChangeStratPops(int amount) 
	{
		List<int> randomIndices = new List<int>();
		for (int i = 0; i < ownedAllList.Count; i ++) randomIndices.Add(i);
		while (randomIndices.Count > 0) 
		{
			int randomIndex = randomIndices[Random.Range(0, randomIndices.Count)];
			randomIndices.Remove(randomIndex);
			if (amount > 0 || (int)ownedAllList[randomIndex].population > 0) 
			{
				ownedAllList[randomIndex].ChangeLocalPopulation(amount);
				return true;
			}
		}
		return false;
	}
}
