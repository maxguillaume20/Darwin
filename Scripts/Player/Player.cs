using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class Player : MonoBehaviour 
{
	public bool gameOver;
	public Species species; 
	public UserInput userInput;
	public Dictionary<Eras, List<SpecType>> unlockedMonumentsDick = new Dictionary<Eras,List<SpecType>> {{Eras.StoneAge, new List<SpecType>()}, {Eras.Renaissance, new List<SpecType>()}, {Eras.Information, new List<SpecType>()}};
	public Eras Era;
	public float startgold, startwood,startunique;
	private float totalUnique;
	private int maxTotalUnique = 1000;
	private Dictionary<ResourceType, float> ResourcesDict;
	public Units units;
	public static int newEraLocUpIncrease = 2;
	public StrategicPoints stratPoints;
	public Buildings buildings;
	public TempBuilding tempBuilding;
	public Color color;
	public Capital capital;
	public HeroUnit currentHero;
	public Dictionary<string, List<WorldObject>> currWorldObjectsDick = new Dictionary<string, List<WorldObject>>(); 
	public Dictionary<string, Dictionary<StatsType, float[]>> upgradedStatsDick = new Dictionary<string, Dictionary<StatsType, float[]>>();
	public Dictionary<string, Dictionary<StatsType, float[]>> buffedStatsDick = new Dictionary<string, Dictionary<StatsType, float[]>>();

	// Currently, the only script that inherits from Player is BattlePlayer, which is a testing script
	protected virtual void Awake()
	{
		capital = GetComponentInChildren<Capital> ();
		stratPoints = GetComponentInChildren<StrategicPoints> ();
		tempBuilding = GetComponentInChildren<TempBuilding> ();
		tempBuilding.gameObject.SetActive (false);
		Era = Eras.StoneAge;
		ResourcesDict = new Dictionary<ResourceType, float> ();
		ResourcesDict.Add (ResourceType.Gold, startgold);
		ResourcesDict.Add (ResourceType.Wood, startwood);
		ResourcesDict.Add (ResourceType.Unique, startunique);
		units = GetComponentInChildren< Units >();
		buildings = GetComponentInChildren< Buildings >(); 
		if (species != Species.Wolves) 
		{
			species = PlayerManager.GetSpecies ();
		}
		GameManager.playersDick.Add(species, this);
		gameObject.name = species.ToString ();
		color = Pop_Dynamics_Model.speciesColorDick [species];
		if (species == PlayerManager.playerSpecies)
		{
			GameManager.HumanPlayer = this;
			userInput = gameObject.AddComponent<UserInput>();
		}
		else 
		{
			gameObject.AddComponent<AI>();
		}
		InitializeStatsDicks ();
	} 

	protected void InitializeStatsDicks () 
	{
		List<string> worldObjectNames = GameManager.GetSpeciesWOTList (species, WorldObjectType.WorldObject);
		foreach (string woName in worldObjectNames) 
		{
			currWorldObjectsDick.Add(woName, new List<WorldObject>());
			upgradedStatsDick.Add(woName, new Dictionary<StatsType, float[]>());
			buffedStatsDick.Add(woName, new Dictionary<StatsType, float[]>());
			foreach (StatsType statsType in GameManager.baseStatsDick[woName].Keys) 
			{
				upgradedStatsDick[woName].Add(statsType,  GameManager.DuplicateArray (GameManager.baseStatsDick[woName][statsType]));
				buffedStatsDick[woName].Add(statsType, new float[GameManager.baseStatsDick[woName][statsType].Length]);
			}
		}
	}

	protected virtual void Start() 
	{
		EvolutionPanel.playerEvoProgressDick.Add (this, 0);
	}

	public MobileWorldObject AddUnit (string unitName, Vector3 SpawnPoint)
	{
		GameObject newUnit = (GameObject)Instantiate(GameManager.GetGameObject(unitName), SpawnPoint, Quaternion.identity);
		newUnit.name = unitName;
		newUnit.transform.parent = units.transform;
		WorldObject wo = newUnit.GetComponent<WorldObject> ();
		wo.SetPlayer (this);
		AddToWOsDick (wo);
		return wo as MobileWorldObject;
	}

	public void AddBuilding (string buildingName, Vector3 BuildPoint)
	{
		GameObject newBuilding = (GameObject)Instantiate (GameManager.GetGameObject(buildingName), BuildPoint, Quaternion.identity);
		newBuilding.name = buildingName;
		newBuilding.transform.parent = buildings.transform;
		newBuilding.transform.localScale = GameManager.GetGameObject (buildingName).transform.localScale;
		Building building = newBuilding.GetComponent<Building> ();
		buildings.AddBuilding (building);
		building.SetPlayer (this);
		AddToWOsDick (building as WorldObject);
		foreach (ResourceType resource in BuildMenu.buildingCostDick[buildingName].Keys)
		{
			ChangeResource(resource, -BuildMenu.buildingCostDick[buildingName][resource]);
		}
	}

	public void AddToWOsDick (WorldObject worldObject) 
	{
		currWorldObjectsDick[worldObject.name].Add(worldObject);
		SetWorldObjectStats (worldObject);
	}

	private void SetWorldObjectStats (WorldObject worldObject) 
	{
		foreach (StatsType statsType in worldObject.statsDick.Keys) 
		{
			if (statsType != StatsType.Health)
			{
				for (int i = 0; i < worldObject.statsDick[statsType].Length; i++) 
				{
					worldObject.statsDick[statsType][i] = upgradedStatsDick[worldObject.name][statsType][i] + buffedStatsDick[worldObject.name][statsType][i];
				}
			}
			else 
			{
				float currHealthPercentage = worldObject.statsDick[statsType][0] / worldObject.statsDick[statsType][1];
				worldObject.statsDick[statsType][1] = upgradedStatsDick[worldObject.name][statsType][1] + buffedStatsDick[worldObject.name][statsType][1];
				worldObject.statsDick [statsType] [0] = worldObject.statsDick [statsType][1] * currHealthPercentage;
				if (worldObject.healthBar) worldObject.healthBar.SetWorldObject (worldObject);
			}
		}
	}

	public void RemoveFromWOsDick(WorldObject wo) 
	{
		currWorldObjectsDick [wo.name].Remove (wo);
	}

	public void ChangePopulation(int amount) 
	{
		if (!gameOver) 
		{
			if(!stratPoints.ChangeStratPops(amount) && !ChangeUnitPop(amount)) 
			{
				Pop_Dynamics_Model.modelStatsDick[species][StatsType.Population] = 0f;
				capital.Die();
			}
			if (this.Equals(GameManager.HumanPlayer))
			{
				int resAmount = (int) Pop_Dynamics_Model.modelStatsDick[species][StatsType.Population];
				GameManager.Hud.populationText.text = resAmount.ToString();		
			}
		}
	}

	private bool ChangeUnitPop(int amount) 
	{
		// this only gets called when the amount is negative and all the owned StratPoints have zero population
		// if there are no owned Strat Points, then you've already lost because you have no capital
		List<string> unitNameList = GameManager.GetSpeciesWOTList (species, WorldObjectType.Unit);
		List<int> randomNameIndices = new List<int>();
		for (int i = 0; i < unitNameList.Count; i ++) randomNameIndices.Add(i);
		while (randomNameIndices.Count > 0) 
		{
			int randomNameIndex = randomNameIndices[Random.Range(0, randomNameIndices.Count)];
			randomNameIndices.Remove(randomNameIndex);
			List<WorldObject> currUnitsList = currWorldObjectsDick[unitNameList[randomNameIndex]];
			if (currUnitsList.Count > 0) 
			{
				Unit unit = currUnitsList[Random.Range(0, currUnitsList.Count)] as Unit;
				// unit.ChangePopCount can change currUnitsList.Count if the unit dies
				unit.ChangePopCount(amount);
				// double checks if unit died, and if it did, checks to make sure it wasn't the last unit
				if (unit && (int)unit.healthArray[0]> 0 || currUnitsList.Count > 0 && randomNameIndices.Count > 0) 
				{
					return true;
				}
			}
		}
		return false;
	}

	public void ChangeResource(ResourceType resource, float amount)
	{
		ResourcesDict[resource] += amount;
		if (resource == ResourceType.Unique && amount > 0) 
		{
			GameManager.evoPanel.ChangeEvoProgress (this, amount / maxTotalUnique);
		}
		if (this.Equals(GameManager.HumanPlayer))
		{
			int resAmount = (int) ResourcesDict[resource];
			GameManager.Hud.ResourceTexts[resource].text = resAmount.ToString();
		}
	}
	
	public void CancelPurchase (float[] costArray) 
	{
		for (int i = 0; i < costArray.Length; i ++) 
		{
			ResourceType resType = GameManager.resourceTypeArraytoDick[i];
			ResourcesDict [resType] += costArray[i];
			if (this.Equals(GameManager.HumanPlayer))
			{
				int resAmount = (int) ResourcesDict[resType];
				GameManager.Hud.ResourceTexts[resType].text = resAmount.ToString();		
			}
		}
	}

	public void NewEra() 
	{
		Era = GameManager.orderEraDick [GameManager.eraOrderDick [Era] + 1];
		foreach (Building cB in buildings.currentBuildings) 
		{
			cB.SetRemainingLocalUpgrades (newEraLocUpIncrease);
		}
		foreach (StrategicPoint oS in stratPoints.ownedAllList) 
		{
			oS.SetRemainingLocalUpgrades (newEraLocUpIncrease);
		}
	}

	public int GetMaxTotalUnique() 
	{
		return maxTotalUnique;
	}

	public float GetResource(ResourceType resource)
	{
		return ResourcesDict[resource];
	}

	public void LoseGame() 
	{
		gameOver = true;
	}
}
