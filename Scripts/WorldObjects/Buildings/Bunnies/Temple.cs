
using UnityEngine;
using System.Collections;
using RTS;

public class Temple : Resource
{
	protected override void Awake () 
	{
		base.Awake ();
		unitName = "Worshiper";
	}

	protected override void Start ()
	{
		base.Start ();
		Occupy (player, 0f);
	}

	public override void Die ()
	{
//		isAlive = false;
//		player.RemoveFromWOsDick (this as WorldObject);
//		player.buildings.RemoveBuilding (this);
//		player.stratPoints.RemoveStratPt (this as StrategicPoint);
//		foreach (SpeciesUnitTrainer mobtrainer in connectedSpeciesUnitTrainersDick.Keys) 
//		{
//			mobtrainer.connectedStratsDick.Remove(this);
//			Destroy(connectedSpeciesUnitTrainersDick[mobtrainer]);
//		}
//		connectedSpeciesUnitTrainersDick.Clear();
//		if (selected) 
//		{
//			player.userInput.SelectedObjects.Remove(this);
//			Deselect();
//		}
		if (isAlive) 
		{
			if (player) 
			{
				player.buildings.RemoveBuilding (this);
			}
		}
		base.Die ();
		if (!destroyingGameObject) 
		{
			StartDestroyGameObject();
		}
	}
}
