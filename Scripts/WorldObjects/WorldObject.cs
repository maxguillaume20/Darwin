using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class WorldObject : MonoBehaviour 
{	
	public Dictionary<StatsType, float[]> statsDick;
//	public Dictionary<StatsType, float[]> buffedStatsDick;
	// [0] = current Hit points, [1] = max Hit points
	public float[] healthArray;
	// [0] = Damage, [1] = Attack Speed
	public float[] attackArray;
	public List<AttackType> attackTypeList;
	// [0] = Blunt, [1] = Pierce, [2] = Crush, [3] = Incendiary, [4] = Psychological
	public float[] defenseArray;
	public WorldObjectType defenseType;
	protected Animator animator;
	protected bool hasVerticalAnimator;
	protected SpriteParent spriteParent;
	protected Player player;
	public Color playerColor;
	public HealthBar healthBar;
	public List<PanelButtonType> buttons = new List<PanelButtonType>();
	protected string statsText = "";
	public WorldObject target;
	public bool attacking;
	public bool selected;
	public bool isAlive;
	public Species originalSpecies;
	public LayerMask enemyLayerMask;
	protected Attack attackComponent;
	public Collider mainCollider;
	protected bool destroyingGameObject;


	protected virtual void Awake()
	{
		animator = GetComponentInChildren<Animator> ();
		mainCollider = GetComponent<Collider> ();
		buttons.Add (PanelButtonType.StatsButton);
		SetStatsDick ();
		spriteParent = GetComponentInChildren<SpriteParent> ();
		spriteParent.transform.localEulerAngles = new Vector3 (HUD.xCameraRotation, 0f, 0f);
	}

	protected virtual void OnEnable()
	{
		isAlive = true;
	}

	protected virtual void Start () 
	{
		if (!player) 
		{
			SetPlayer(transform.root.GetComponent<Player> ());
		}
		SetLayer ();
		if (GameManager.inBattleGround) 
		{
			healthArray[0] = healthArray[1];
		}
		healthBar = GetComponentInChildren<HealthBar> ();
		if (healthBar) 
		{
			healthBar.transform.RotateAround (transform.position, Vector3.right, 90f - HUD.xCameraRotation);
			healthBar.transform.eulerAngles = new Vector3 (HUD.xCameraRotation, 0f,0f);
			healthBar.SetWorldObject(this);
			healthBar.gameObject.SetActive (false);
		}
	}

	protected void SetLayer () 
	{
		List<string> worldList = new List<string> {"Bunnies", "Deer", "Sheep", "Wolves", "NonPlayer"};
		worldList.Remove (GetSpecies ().ToString ());
		gameObject.layer = LayerMask.NameToLayer (GetSpecies ().ToString ());
		string[] maskArray = new string[worldList.Count];
		for (int i = 0; i < maskArray.Length; i++) 
		{
			maskArray[i] = worldList[i];
		}
		enemyLayerMask = LayerMask.GetMask (maskArray);
	}

	public virtual void SetTarget (WorldObject newTarget, bool ordered) 
	{
		target = newTarget;
		if (target && attackComponent != null && !target.IsOwnedBy(GetSpecies()))
		{
			attackComponent.StartAttackCoroutine();
		}
	}
	
	public virtual void Damage (List<AttackType> damageTypeList, float damage) 
	{
		foreach (AttackType damageType in damageTypeList) 
		{
			healthArray[0] -= damage * ((1f - defenseArray[GameManager.attackTypeDickToArray[damageType]]) / (float) damageTypeList.Count);
		}
		if (healthBar) healthBar.ChangeHP (healthArray[0]);
		if (healthArray[0] <= 0) 
		{
			Die();
		}
	}

	public void Attack(WorldObject attacker) 
	{
		if (isAlive) 
		{
			Damage (attacker.attackTypeList, attacker.attackArray [0]);
			if (isAlive) 
			{
				Retaliate(attacker);
			}
			else if (attacker.GetSpecies() == Species.Deer && !GameManager.inBattleGround) 
			{
				GameManager.playersDick[Species.Deer].ChangeResource(ResourceType.Unique, healthArray[1] / 10f);
			}
		}
	}

	
	public void StartAttackAnimation () 
	{
		animator.SetBool ("InEvent", true);
	}

	protected void Retaliate(WorldObject attacker) 
	{
		if (attackComponent != null) attackComponent.Retaliate(attacker);
	}

	public List<PanelButtonType> GetButtons()
	{
		return buttons;
	}

	public Player GetPlayer() 
	{
		return player;
	}

	public void SetPlayer (Player newowner) 
	{
		player = newowner;
		if (player) 
		{
			playerColor = player.color;
		}
	}

	protected virtual void SetAnimator (Vector3 steeringTarget) 
	{

	}

	private void HandleAnimationEvents (int eventIndex) 
	{
		GameManager.animationEventsArray [name] [eventIndex] ();
	}

	public virtual void SelectTap(Player controller)
	{
		if (healthBar) healthBar.gameObject.SetActive (true);
		selected = true;
	}

	public virtual void Deselect()
	{
		if (healthBar) healthBar.gameObject.SetActive (false);
		selected = false;
		if (GameManager.Hud.mainpanel.gameObject.activeSelf) GameManager.Hud.ActuallyClosePanel ();
	}

	public Species GetSpecies()
	{
		if (player) return player.species;
		else return Species.NonPlayer;
	}

	public virtual string GetNameText() 
	{
		if (gameObject.name.Contains("(Clone)")) 
		{
			return gameObject.name.Remove(gameObject.name.Length - 7);
		}
		else 
		{
			 return gameObject.name;
		}
	}

	public virtual string GetHealthText()
	{
		return (int)healthArray[0] + " / " + (int)healthArray[1];
	}

	public string GetStatsText()
	{
		return statsText;
	}

	public float GetLocalStat (StatsType statsType, int statsIndex) 
	{
		return statsDick [statsType] [statsIndex] - player.upgradedStatsDick [name] [statsType] [statsIndex] - player.buffedStatsDick [name] [statsType] [statsIndex] + GameManager.baseStatsDick [name] [statsType] [statsIndex];
	}

	public virtual string GetFunctionButton1Text() 
	{
		return "ass";
	}

	public bool IsOwnedBy(Species controllerSpecies) 
	{
		if(GetSpecies().Equals(controllerSpecies)) 
		{
			return true;
		} 
		return false;
	}

	public Species GetOriginalSpecies() 
	{
		return originalSpecies;
	}

	public virtual void Die() 
	{ 
		if (isAlive) 
		{
			isAlive = false;
			attacking = false;
			target = null;
			if (healthBar) 
			{
				healthBar.gameObject.SetActive (false);
			}
			if (GameManager.inBattleGround) 
			{
				GameManager.battleController.selectedWOList.Remove (this as WorldObject);
			}
			else if (player) 
			{
				player.RemoveFromWOsDick (this);
			}
			if (selected) 
			{
				GameManager.HumanPlayer.userInput.SelectedObjects.Remove(this);
				Deselect();
			}
			gameObject.SetActive (false);
		}
	}

	protected void StartDestroyGameObject ()
	{
		if (!destroyingGameObject) 
		{
			destroyingGameObject = true;
			GameObjectList.StartDestroyGameObject (gameObject);
		}
	}
//
//	private IEnumerator DestroyGameObject () 
//	{
//		yield return new WaitForSeconds (destroyWaitTime);
//		Destroy (gameObject);
//	}

	public virtual void SetStatsDick () 
	{
		statsDick = new Dictionary<StatsType, float[]> {{StatsType.Health, healthArray}};
		// if the defense array has not been set manually in the prefab
		if (defenseArray.Length == 0) 
		{
			WorldObjectType thisDefenseType = WorldObjectType.Empty;
			if (defenseType == WorldObjectType.LightUnit || defenseType == WorldObjectType.HeavyUnit || defenseType == WorldObjectType.Siege) 
			{
				thisDefenseType = defenseType;
			}
			else if (this as Building) 
			{
				thisDefenseType = WorldObjectType.Building;
			}
			if (thisDefenseType != WorldObjectType.Empty) 
			{
				defenseArray = GameManager.DuplicateArray (GameManager.baseDefenseTypeArraysDick[thisDefenseType]);
			}
		}
		statsDick.Add (StatsType.Defense, defenseArray);
		if (attackArray.Length > 0) 
		{
			statsDick.Add (StatsType.Attack, attackArray);
		}
	} 
}
