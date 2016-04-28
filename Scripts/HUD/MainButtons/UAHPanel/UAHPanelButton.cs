using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class UAHPanelButton : MonoBehaviour
{
	protected TempBuilding tempBuilding;
	protected Eras era;
	public Button button;
	public Text[] texticals;
	protected string[] uahNames;
	protected bool setObject;

	protected virtual void Awake() 
	{
		button = GetComponent<Button> ();
		button.interactable = false;
		texticals = GetComponentsInChildren<Text> ();
	}

	public Eras GetEra() 
	{
		return era;
	}

	protected IEnumerator ChangeTextColorToRed() 
	{
		texticals[1].color = Color.red;
		for (float timer = 0.0f; timer < 1f; timer += Time.deltaTime) yield return null;
		texticals[1].color = Color.black;
	}
	
	protected bool EnoughResources(string name)
	{
		Building building = GameManager.GetGameObject(name).GetComponent<Building> ();
		if (building) 
		{
			float[] costs = building.buildingCostArray;
			for (int i = 0; i < costs.Length; i++) 
			{
				if (costs[i] > 0 && GameManager.HumanPlayer.GetResource(GameManager.resourceTypeArraytoDick[i]) < costs[i]) 
				{
					StartCoroutine(ChangeTextColorToRed());
					return false;
				}
			}
		}
		return true;
	}	
}
