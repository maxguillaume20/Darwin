using UnityEngine;
using System.Collections.Generic;
using RTS;

public class NonRelatedUnitTrainer : MobTrainer 
{
	public float[] costArray;
	private Queue<string> trainingQueue = new Queue<string> ();

	public override void StartTraining ()
	{
		trainingQueue.Enqueue (unitName);
		base.StartTraining ();
	}

	public int GetTrainingQueueCount () 
	{
		return trainingQueue.Count;
	}

	public void CancelTraining () 
	{
		trainingQueue.Dequeue ();
		player.CancelPurchase (costArray);
	}

	protected override bool AbleToTrain ()
	{
		return trainingQueue.Count > 0;
	}

	protected override void CreateMob ()
	{
		base.CreateMob ();
		trainingQueue.Dequeue ();
	}

	protected override void ResetMob (MobileWorldObject newMob)
	{
		base.ResetMob (newMob);
		newMob.gameObject.SetActive (true);
	}

	public override string GetTrainingText ()
	{
		string trainingText = unitName + "s\n" + currentMobCount + " / " + mobTrainerStatsArray[0];
		if (trainingQueue.Count > 1) 
		{
			TrainingProgressBar.countText.text = "(" + (trainingQueue.Count - 1) + ")";
		}
		return trainingText;
	}

	protected override void FinishConstruction ()
	{
		buttons.Add (PanelButtonType.UnitsMenu);
		base.FinishConstruction ();
	}

	public override void SetStatsDick ()
	{
		base.SetStatsDick ();
		statsDick.Add (StatsType.Cost, costArray);
	}
}
