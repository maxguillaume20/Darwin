using UnityEngine;
using System.Collections.Generic;
using RTS;

public class MonumentType : MonoBehaviour 
{
	protected Monument thisMonument;
	public string monName;
	protected float maxHealth;
	protected string unitName;
	protected int maxUnits;
	protected float unitTrainingTime;
	public Eras era;

	protected virtual void Awake () 
	{
		thisMonument = GetComponent<Monument> ();
	}

	public virtual void OnSelection () 
	{
		thisMonument.healthArray [1] = maxHealth;
		thisMonument.healthBar.ResetBar ();
		thisMonument.unitName = unitName;
		thisMonument.mobTrainerStatsArray [0] = maxUnits;
		thisMonument.mobTrainerStatsArray [1] = unitTrainingTime;
		thisMonument.localUpgradesList = MakeMonLocalUpgrades ();
	}

	public virtual void ChangeMobCount (int change) 
	{

	}

	public virtual void Unoccupy () 
	{

	}

	protected virtual List<LocalUpgrade[]> MakeMonLocalUpgrades() 
	{
		return new List<LocalUpgrade[]> ();
	}
}
