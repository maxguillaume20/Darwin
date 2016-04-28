using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Disasters : MonoBehaviour 
{
	public GameObject[] disasterPrefabs;
	private float burningBuildingChance = 0.25f;
	private float burningBuildingFrequency = 120f;

	private void Awake() 
	{
//		GameManager.disasters = this;
	}

	private GameObject GetDisaster(string name) 
	{
		foreach (GameObject go in disasterPrefabs) 
		{
			if (go.name == name) return go;
		}
		return null;
	}

	public void StartDisaster(string disasterName) 
	{
		StartCoroutine (disasterName);
	}

	private IEnumerator BurnBuildings() 
	{
		while (!GameManager.GameOver) 
		{
			for (float timer = 0f; timer < burningBuildingFrequency; timer += Time.deltaTime) yield return null;
			float chance = Random.Range(0f, 1f);
			if (chance < burningBuildingChance) // burn baby
			{
				List<int> burnList = new List<int> {0, 1, 2, 3};
				while (burnList.Count > 0) 
				{
					int x = Random.Range(0,burnList.Count);
					int y = burnList[x];
					burnList.Remove(y);
					int buildingsCount = GameManager.playersDick[GameManager.speciesArray[y]].buildings.currentBuildings.Count;
					if (buildingsCount > 0) 
					{
						int randomBuilding = Random.Range(0, buildingsCount);
						Building poorBuilding = GameManager.playersDick[GameManager.speciesArray[y]].buildings.currentBuildings[randomBuilding];
//						if (!poorBuilding.burning) 
//						{
							GameObject burningBuilding = (GameObject) Instantiate(GetDisaster("BurningBuilding"), poorBuilding.transform.position, Quaternion.identity); 
							burningBuilding.GetComponent<BurningBuilding>().building = poorBuilding;
							break;
//						}
					}
				}
			}
		}
	}
}
