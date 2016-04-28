using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Sentry : MobTrainer
{
	public float patrolRadius;

	protected override void Awake ()
	{
		base.Awake ();
		if(transform.position.z < 150) 
		{
			spawnPoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 5f);
		}
		else spawnPoint = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 5f);
	}

	protected override void Start ()
	{
		base.Start ();
		if (GameManager.inBattleGround) 
		{
			StartTraining ();
		}
	}

	protected override void FinishConstruction ()
	{
		base.FinishConstruction ();
		StartTraining ();
	}

	protected override IEnumerator Train () 
	{
		training = true;
		while (AbleToTrain ()) 
		{
			yield return new WaitForSeconds (mobTrainerStatsArray[1]);
			if (AbleToTrain()) CreateMob ();
		}
		training = false;
	}

	protected override void ResetMob (MobileWorldObject newMob)
	{
		base.ResetMob (newMob);
		newMob.gameObject.SetActive (true);
	}

	public override void MobDied ()
	{
		base.MobDied ();
		StartTraining ();
	}
}
