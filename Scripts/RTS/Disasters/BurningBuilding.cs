using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class BurningBuilding : MonoBehaviour 
{
	private static List<AttackType> burnList { get { return new List<AttackType> {AttackType.Incendiary}; } }
	public Building building;
	private float burnTime;
	private float damage;

	void Start() 
	{
		burnTime = building.healthArray[1] / 2;
		// rememeber that buildings are susceptible to Incendiary attacks, originally by a factor of 2
		damage = 1.25f;
		StartCoroutine (Burn ());
	}

	private IEnumerator Burn() 
	{
		while (burnTime > 0 && building != null) 
		{
			yield return new WaitForSeconds(1f);
			burnTime --;
			if (building) building.Damage(burnList, damage);
		}
		Destroy (gameObject);
	}

}
