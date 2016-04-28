using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class BuffStatsMonument : Monument
{
	protected float buffAmount;

	protected void ChangeWorldObjectStats (int change, string woName, StatsType statsType, int index) 
	{
		float amount = GameManager.baseStatsDick [woName] [statsType] [index] * buffAmount;
		if (statsType == StatsType.Defense) 
		{
			amount = buffAmount;
		}
		GameManager.playersDick[GetSpecies()].buffedStatsDick [woName] [statsType] [index] += change * amount;
		foreach (WorldObject worldObject in GameManager.playersDick[GetSpecies()].currWorldObjectsDick[woName]) 
		{
			worldObject.statsDick [statsType][index] += change * amount;
		}
	}
	
	protected void ChangeWorldObjectTypeStats (int change, WorldObjectType worldObjectType, StatsType statsType, int index) 
	{
		List<string> worldObjectTypeNameList = GameManager.GetSpeciesWOTList (GetSpecies(), worldObjectType);
		foreach (string woName in worldObjectTypeNameList) ChangeWorldObjectStats (change, woName, statsType, index);
	}
}
