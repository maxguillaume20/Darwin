using UnityEngine;
using System.Collections;
using RTS;

public class GiantCarrot : Tower 
{
	protected override void Awake ()
	{
		base.Awake ();
		healthArray[0] = healthArray[1];
	}

	private void Update ()
	{
		healthArray[0] -= Time.deltaTime;
		healthBar.ChangeHP (healthArray[0]);
		if (healthArray[0] <= 0) Die ();
	}	

	public override void SelectTap (Player controller)
	{
		healthBar.gameObject.SetActive (true);
	}

	public void IncreaseAttackSpeed () 
	{
		attackArray [1] *= 2f;
	} 
}
