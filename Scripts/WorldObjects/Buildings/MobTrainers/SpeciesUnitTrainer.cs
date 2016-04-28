using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public class SpeciesUnitTrainer : SpeciesMobTrainer 
{
	public bool garrisoningUnits;
	public StrategicPoint mainStratPoint;
	public LineRenderer stratLine;
	private static float stratLineYPos = -4f;

	public void SetMainStratPoint (StrategicPoint newMainStratPoint) 
	{
		newMainStratPoint.connectedSpeciesTrainers.Add (this);
		mainStratPoint = newMainStratPoint;
		GameObject connectingLine = (GameObject) Instantiate (GameManager.GetGameObject("StraightLine"));
		LineRenderer lineRenderer = connectingLine.GetComponent<LineRenderer>();
		Color stratLineColor = new Color (player.color.r, player.color.g, player.color.b, 0.7f);
		lineRenderer.SetColors (stratLineColor, stratLineColor);
		lineRenderer.SetPosition(0, new Vector3 (transform.position.x, stratLineYPos, transform.position.z));
		lineRenderer.SetPosition(1, new Vector3 (newMainStratPoint.transform.position.x, stratLineYPos, newMainStratPoint.transform.position.z));
		stratLine = lineRenderer;
		stratLine.gameObject.SetActive (false);
	}

	public override void SelectTap (Player controller)
	{
		base.SelectTap (controller);
		if (mainStratPoint)
		{
			stratLine.gameObject.SetActive(true);
		}
	}

	public override void Deselect ()
	{
		base.Deselect ();
		if (stratLine)
		{
			stratLine.gameObject.SetActive(false);
		}
	}

	protected override void FinishConstruction ()
	{
		buttons.Add (PanelButtonType.UnitsMenu);
		buttons.Add (PanelButtonType.FunctionButton1);
		base.FinishConstruction ();
	}

	public int GetGarrisonedUnitsCount() 
	{
		int garrisonCount = 0;
		foreach (Unit unit in mobList) 
		{
			if (unit.garrisoned) garrisonCount++;
		}
		return garrisonCount;
	}

	protected override bool AbleToTrain ()
	{
		int idleUnits = 0;
		if (mainStratPoint) 
		{
			idleUnits = mainStratPoint.GetIdleUnits ();
		}
		return GetCurrentUnits () < idealMobCount && mobPopCount <= idleUnits;
	}

	protected override void ResetMob (MobileWorldObject newMob)
	{
		base.ResetMob (newMob);
		Unit newUnit = newMob as Unit;
		newUnit.garrisoned = true;
		newUnit.currPopCount = mobPopCount;
		if (mainStratPoint) 
		{
			mainStratPoint.SpeciesUnitTrainerUnitMade(mobPopCount);
		}
	}

	protected override void DestroyMob() 
	{
		foreach (Unit unit in mobList)
		{
			if (unit.garrisoned) 
			{
				unit.isAlive = false;
				unit.garrisoned = false;
				if (mainStratPoint) 
				{
					mainStratPoint.population += unit.currPopCount;
				}
				currentMobCount --;
				break;
			}	
		}
	}

	public override void MobDied ()
	{
		base.MobDied ();
		StartTraining ();
	}

	public override string GetFunctionButton1Text ()
	{
		if (GetGarrisonedUnitsCount() > 0 || garrisoningUnits) return "Deploy";
		return "Garrison";
	}

	public void DeployGarrisonUnits() 
	{
		if (GetGarrisonedUnitsCount() > 0) 
		{
			List<MobileWorldObject> depolyList = new List<MobileWorldObject> ();
			foreach( Unit unit in mobList) 
			{
				if (unit.garrisoned) 
				{
					unit.garrisoned = false;
					depolyList.Add (unit as MobileWorldObject);
					unit.transform.position = spawnPoint;
					unit.gameObject.SetActive (true);
				}
			}
			player.units.squadController.MakeSquad (depolyList);
			player.units.squadController.SetFormation (depolyList[0].squadInt, spawnPoint, transform.position, null);
		}
		else 
		{
			foreach( Unit unit in mobList) 
			{
				if (!unit.garrisoned && unit.isAlive) 
				{
					unit.StartReturnHome();
				}
			}
			StartCoroutine(GarrisoningUnits());
		}
		FunctionButton1.SetText (GetFunctionButton1Text ());
	}

	private IEnumerator GarrisoningUnits() 
	{
		garrisoningUnits = true;
		while (garrisoningUnits) 
		{
			int unitsLeftCount = 0;
			foreach (Unit unit in mobList) 
			{
				if (unit.isAlive && !unit.garrisoned) 
				{
					unitsLeftCount++;
					if (unit.target != this as WorldObject) 
					{
						garrisoningUnits = false;
						break;
					}
				}
			}
			if (unitsLeftCount == 0) garrisoningUnits = false;
			yield return new WaitForSeconds(1f);
		}
	}

	public void UnitReturn(Unit unit) 
	{
		if (GetGarrisonedUnitsCount() > idealMobCount) DestroyMob();
	}

	public override void Die ()
	{
		if (isAlive) 
		{
			if (mainStratPoint)
			{
				mainStratPoint.connectedSpeciesTrainers.Remove(this);
				Destroy(stratLine.gameObject);
			}
			foreach (Unit unit in mobList) 
			{
				if (unit.garrisoned) 
				{
					unit.Die ();
					if (player && player.Equals(GameManager.HumanPlayer))
					{
						int resAmount = (int) Pop_Dynamics_Model.modelStatsDick [player.species][StatsType.Population];
						GameManager.Hud.populationText.text = resAmount.ToString();		
					}
				}
			}
		}
		base.Die ();
	}

//	public void SetConnectedBuildingsDick(Dictionary<StrategicPoint, LineRenderer> connectedDick) 
//	{
//		foreach (StrategicPoint stratPoint in connectedDick.Keys) 
//		{
//			connectedStratsDick.Add(stratPoint, connectedDick[stratPoint]);
//		}
//	}
}
