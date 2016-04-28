using UnityEngine;
using System.Collections.Generic;

public class SquadController : MonoBehaviour 
{
	private Dictionary<int, List<MobileWorldObject>> squadDick = new Dictionary<int, List<MobileWorldObject>> ();
	private Dictionary<int, int> squadStopDick = new Dictionary<int, int>();
	private Dictionary<int, bool> squadFormationSetDick = new Dictionary<int, bool> ();
	private static float formationSpacing = 1f;
	private int squadTotalCount;

	public void MakeSquad (List<MobileWorldObject> unitsList) 
	{
		if (unitsList.Count > 1) 
		{
			squadTotalCount++;
			AddNewSquad(squadTotalCount);
			foreach (MobileWorldObject unit in unitsList) 
			{
				RemoveUnitFromCurrentSquad(unit);
				squadDick[squadTotalCount].Add(unit);
				unit.squadInt = squadTotalCount;
			}
		}
		else if (unitsList.Count == 1) 
		{
			if (unitsList[0].squadInt > 0) 
			{
				RemoveUnitFromCurrentSquad(unitsList[0]);
			}
		} 
//		print ("SquadTotal: " + squadDick.Count);
	}

	public void MakeSquad (List<Unit> unitList) 
	{
		List<MobileWorldObject> poopList = new List<MobileWorldObject> ();
		foreach (Unit unit in unitList) 
		{
			poopList.Add (unit as MobileWorldObject);
		}
		MakeSquad (poopList);
	}

	private void AddNewSquad(int newSquadIndex) 
	{
		squadDick.Add (newSquadIndex, new List<MobileWorldObject> ());
		squadStopDick.Add (newSquadIndex, 0);
		squadFormationSetDick.Add (newSquadIndex, false);
	}

	public void RemoveUnitFromCurrentSquad (MobileWorldObject unit) 
	{
		if (unit.squadInt > 0) 
		{
			squadDick[unit.squadInt].Remove(unit);
			if (squadDick[unit.squadInt].Count == 0)
			{
				squadDick.Remove(unit.squadInt);
				squadStopDick.Remove (unit.squadInt);
				squadFormationSetDick.Remove(unit.squadInt);
			}
			unit.squadInt = 0;
		}
	}

	public void SquadStop(int squadIndex) 
	{
		if (squadIndex > 0) 
		{
			squadStopDick[squadIndex]++;
			if (squadStopDick[squadIndex] == squadDick[squadIndex].Count) 
			{
				MobileWorldObject[] unitArray = squadDick [squadIndex].ToArray ();
				for (int i = 0; i < unitArray.Length; i++) 
				{
					unitArray[i].StartIncreaseNavRadius ();
					RemoveUnitFromCurrentSquad(unitArray[i]);
				}
			}
		}
	}

	public void SetFormation(int squadIndex, Vector3 destination, Vector3 lastSteeringTartget, WorldObject target) 
	{
		if (squadIndex > 0 && !squadFormationSetDick[squadIndex]) 
		{
			squadFormationSetDick[squadIndex] = true;
			Quaternion direction = Quaternion.LookRotation (destination - lastSteeringTartget);
			if (target == null) 
			{
				SetBasicFormation(squadIndex, destination, direction);
			}
			else if (!target.IsOwnedBy(squadDick[squadIndex][0].GetSpecies()))
			{
				SetAttackFormation (squadIndex, destination, direction);
			}
		}
	}

	private void SetBasicFormation(int squadIndex, Vector3 destination, Quaternion direction) 
	{
		float sqrtUnitsCount = Mathf.Sqrt (squadDick[squadIndex].Count);
		float cosRotation = Mathf.Cos (-Mathf.Deg2Rad * direction.eulerAngles.magnitude);
		float sinRotation = Mathf.Sin (-Mathf.Deg2Rad * direction.eulerAngles.magnitude);
		for (int i = 0; i < squadDick[squadIndex].Count; i++) 
		{
			squadDick[squadIndex][i].SetNavAgentDestination(BasicFormationPosition(destination, sqrtUnitsCount, cosRotation, sinRotation, i), formationSpacing * (int) sqrtUnitsCount);
		}
	}

	private Vector3 BasicFormationPosition(Vector3 destination, float sqrtUnitCount, float cosRotation, float sinRotation, int unitIndex) 
	{
		float xPos = 0f;
		float zPos = 0f;
		// Placement
		if (unitIndex < Mathf.CeilToInt (sqrtUnitCount) * (Mathf.RoundToInt (sqrtUnitCount) - 1)) 
		{
			xPos = -formationSpacing * (Mathf.CeilToInt (sqrtUnitCount) - 1) / 2f + (unitIndex % Mathf.CeilToInt (sqrtUnitCount)) * formationSpacing;
		}
		else 
		{
			int remainder = (int)(sqrtUnitCount * sqrtUnitCount) - Mathf.CeilToInt (sqrtUnitCount) * (Mathf.RoundToInt (sqrtUnitCount) - 1);
			xPos = -formationSpacing * (remainder - 1) / 2f  + (unitIndex % Mathf.CeilToInt (sqrtUnitCount)) * formationSpacing;
		}
		zPos = formationSpacing * (Mathf.RoundToInt (sqrtUnitCount) - 1) / 2f - (unitIndex / Mathf.CeilToInt (sqrtUnitCount)) * formationSpacing;
		// Rotation
		float newX = cosRotation * (xPos) - sinRotation * (zPos);
		float newZ = sinRotation * (xPos) + cosRotation * (zPos);
		return new Vector3 (newX + destination.x, destination.y, newZ + destination.z);
	}

	private void SetAttackFormation(int squadIndex, Vector3 destination, Quaternion direction) 
	{
		MobileWorldObject[] mobileWOArray = squadDick [squadIndex].ToArray ();
		foreach (MobileWorldObject unit in mobileWOArray) 
		{
			RemoveUnitFromCurrentSquad(unit);
		}
	}
}
