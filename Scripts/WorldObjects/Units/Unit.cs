using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Unit : MobileWorldObject 
{
	public bool garrisoned;
	public int currPopCount;
	private bool returningHome;
	private SpeciesUnitTrainer unitTrainer;

	protected override void Awake() 
	{
		base.Awake ();
		healthArray[0] = healthArray[1];
		if (attackArray.Length > 0) 
		{
			attackComponent = gameObject.AddComponent<MobileWOAttack>();
		}
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		if (healthBar) 
		{
			healthBar.ChangeHP(healthArray [0]);
			healthBar.gameObject.SetActive(false);
		}
		if (attackComponent != null && attackComponent.enabled == true) attackComponent.StartFindClosestEnemy();
	}

	protected override void Start () 
	{
		base.Start ();
		gameObject.tag = "Unit";
	}

	public override void SelectTap(Player controller)
	{
		base.SelectTap (controller);
		if (player) player.units.SelectUnit (this);
	}

	public override void SetTarget(WorldObject newTarget, bool ordered)
	{
		base.SetTarget (newTarget, ordered);
		if (returningHome && newTarget != unitTrainer) 
		{
			returningHome = false;
		}
	}

	public virtual void ChangePopCount (int amount) 
	{
		currPopCount += amount;
		healthArray [0] += healthArray [1] * (float)amount / unitTrainer.mobPopCount;
		healthBar.ChangeHP (healthArray [0]);
		if ((int)healthArray[0] <= 0) 
		{
			healthArray[0] = 0f;
			Die ();
		}
	}

	public void StartReturnHome() 
	{
		SetTarget (unitTrainer, true);
		StartCoroutine (ReturnHome ());
	}

	private IEnumerator ReturnHome() 
	{
//		attacking = false;
		returningHome = true;
		StartMoving (unitTrainer.transform.position);
		while(navAgent.pathPending) yield return null;
		while (returningHome) 
		{
			if (mainCollider.bounds.Intersects (unitTrainer.mainCollider.bounds)) 
			{
				orderedToAct = false;
				isMoving = false;
				returningHome = false;
				garrisoned = true;
				target = null;
				if (navAgent.enabled) navAgent.ResetPath();
				player.units.squadController.RemoveUnitFromCurrentSquad (this);
				healthBar.gameObject.SetActive (false);
				if (selected) 
				{
					GameManager.HumanPlayer.userInput.SelectedObjects.Remove(this);
					Deselect();
				}
				gameObject.SetActive(false);
				unitTrainer.UnitReturn(this);
			}
			yield return new WaitForSeconds(1f);
		}
	}

	public override void Die ()
	{
		garrisoned = false;
		returningHome = false;
		if (GameManager.inBattleGround) 
		{
			GameManager.battleController.player.units.squadController.RemoveUnitFromCurrentSquad(this);
			GameManager.battleController.player.units.RemoveFromSelectedUnits (this);
		}
		else if (isAlive && player) 
		{
			GameManager.HumanPlayer.units.RemoveFromSelectedUnits(this);
			Pop_Dynamics_Model.modelStatsDick [player.species][StatsType.Population] -= currPopCount;
			if (player.Equals(GameManager.HumanPlayer))
			{
				int resAmount = (int) Pop_Dynamics_Model.modelStatsDick [player.species][StatsType.Population];
				GameManager.Hud.populationText.text = resAmount.ToString();		
			}
		}
		base.Die ();
	}

	public override void SetMobTrainer (MobTrainer thisMobTrainer) 
	{
		base.SetMobTrainer (thisMobTrainer);
		unitTrainer = thisMobTrainer as SpeciesUnitTrainer;
	}
}

