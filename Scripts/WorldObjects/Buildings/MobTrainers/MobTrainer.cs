using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class MobTrainer : Building 
{
	// [0] = maxUnits, [1] = trainingTime
	public float[] mobTrainerStatsArray;
	public string unitName;
	protected Vector3 spawnPoint;
	public int currentMobCount;
	protected float currTrainingTime;
	// currMaxTrainingTime is used in case there is a change in training time while training
	protected float currMaxTrainingTime;
	public bool training;
	protected List<MobileWorldObject> mobList = new List<MobileWorldObject> ();
	public Dictionary<StatsType, float[]> locUpUnitStatsDick = new Dictionary<StatsType, float[]> ();

	protected override void Awake ()
	{
		base.Awake ();
		if(transform.position.z < 150) 
		{
			spawnPoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 10);
		}
		else 
		{
			spawnPoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 10);
		}
	}

	protected override void Start ()
	{
		base.Start ();
		if (GameManager.baseStatsDick.ContainsKey (unitName)) 
		{
			foreach (StatsType statsType in GameManager.baseStatsDick[unitName].Keys) 
			{
				locUpUnitStatsDick.Add (statsType, new float[GameManager.baseStatsDick[unitName][statsType].Length]);
			}
			for (int i = 0; i < mobTrainerStatsArray[0]; i++) 
			{
				InstantiateNewUnit ();
			}
		}
	}

	private void InstantiateNewUnit () 
	{
		if (mobList.Count < mobTrainerStatsArray[0]) 
		{
			mobList.Add(player.AddUnit(unitName, transform.position));
			mobList[mobList.Count - 1].gameObject.SetActive(false);
			foreach (StatsType statsType in locUpUnitStatsDick.Keys) 
			{
				for (int i = 0; i < locUpUnitStatsDick[statsType].Length; i ++) 
				{
					mobList[mobList.Count - 1].statsDick[statsType][i] += locUpUnitStatsDick[statsType][i];
				}
			}
			(mobList[mobList.Count - 1]).SetMobTrainer (this);
			mobList[mobList.Count - 1].isAlive = false;
		}
	}

	protected virtual bool AbleToTrain ()
	{
		return currentMobCount < mobTrainerStatsArray[0];
	}

	public virtual void StartTraining () 
	{
		if (!training && AbleToTrain()) 
		{
			StartCoroutine (Train ());
		}
	}

	protected virtual IEnumerator Train () 
	{
		training = true;
		while (AbleToTrain ()) 
		{
			currMaxTrainingTime = mobTrainerStatsArray[1] * 1f;
			for (currTrainingTime = 0f; currTrainingTime < currMaxTrainingTime && AbleToTrain(); currTrainingTime += Time.deltaTime) yield return null;
			if (currTrainingTime >= currMaxTrainingTime) CreateMob();
		}
		training = false;
	}

	public float GetTrainingProgress () 
	{
		return currTrainingTime / currMaxTrainingTime;
	}

	protected virtual void CreateMob() 
	{
		currentMobCount ++;
		foreach (MobileWorldObject mob in mobList) 
		{
			if (!mob.isAlive) 
			{
				ResetMob (mob);
				break;
			}
		}
	}

	public virtual void MobDied () 
	{
		currentMobCount --;
	}

	protected virtual void ResetMob (MobileWorldObject newMob) 
	{
		newMob.isAlive = true;
		newMob.healthArray[0] = newMob.healthArray[1];
		if (newMob.healthBar) newMob.healthBar.ResetBar ();
		newMob.transform.position = spawnPoint;
	}

	public virtual string GetTrainingText () 
	{
		return "";
	}

	public MobileWorldObject GetMob (int mobIndex) 
	{
		return mobList [mobIndex];
	}

	public int GetMobCount () 
	{
		return mobList.Count;
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.MobTrainerStats, mobTrainerStatsArray);
	}
}
