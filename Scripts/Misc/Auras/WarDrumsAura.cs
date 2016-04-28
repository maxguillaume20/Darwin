using UnityEngine;
using System.Collections;
using RTS;

public class WarDrumsAura : Aura
{
	public float attackSpeedIncrease = 1.3f;
	public float healthIncrease = 1.2f;
	public int resCost;

	private void Awake() 
	{
		duration = 30f;
		timer = duration;
	}

	protected override void Start ()
	{
		base.Start ();
		if (target.tag == "Unit") 
		{
			Unit unit = target as Unit;
			unit.attackArray[4] = unit.attackArray[4] / attackSpeedIncrease;
			unit.healthArray[1] = unit.healthArray[1] * healthIncrease;
			unit.healthArray[0] = unit.healthArray[0] * healthIncrease;
			unit.healthBar.SetWorldObject(unit as WorldObject);
		}
		else Destroy(gameObject);
	}	

	protected override void EndAura ()
	{
		base.EndAura ();
		Unit unit = target as Unit;
		unit.attackArray[4] = unit.attackArray[4] * attackSpeedIncrease;
		unit.healthArray[1] = unit.healthArray[1] / healthIncrease;
		if (unit.healthArray[0] > unit.healthArray[1]) unit.healthArray[0] = unit.healthArray[1];
		unit.healthBar.SetWorldObject(unit as WorldObject);
	}
}
