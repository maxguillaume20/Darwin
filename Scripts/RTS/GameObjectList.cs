using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public class GameObjectList : MonoBehaviour 
{	
	public GameObject[] gameObjectsArray;
	public Dictionary<string, GameObject> gameObjectDick = new Dictionary<string, GameObject>(); 
	public Dictionary<Species, Dictionary<WorldObjectType, List<string>>> speciesWOTDick = new Dictionary<Species, Dictionary<WorldObjectType, List<string>>> ();
	public Dictionary<Species, Dictionary<AttackType, List<string>>> speciesAttackTypesDick = new Dictionary<Species, Dictionary<AttackType, List<string>>> ();
	private static bool created =  false;

	void Awake() 
	{
		if(!created) 
		{
			created = true;
			DontDestroyOnLoad(transform.gameObject);
			GameManager.SetGameObjectList(this);
			// Initialize 
			foreach (Species species in GameManager.speciesArray) 
			{
				speciesWOTDick.Add(species, new Dictionary<WorldObjectType, List<string>>());
				foreach (WorldObjectType woT in GameManager.worldObjectTypeArray) 
				{
					speciesWOTDick[species].Add(woT, new List<string>());
				}
				speciesAttackTypesDick.Add(species, new Dictionary<AttackType, List<string>>());
				foreach (AttackType attackType in GameManager.attackTypeArray) 
				{
					speciesAttackTypesDick[species].Add(attackType, new List<string>());
				}
			}
			Dictionary<Type, Tuple<WorldObjectType, Type>> typeDick = new Dictionary<Type, Tuple<WorldObjectType, Type>> ();
			typeDick.Add (typeof (MobileWorldObject), new Tuple<WorldObjectType, Type> (WorldObjectType.Mobile, typeof (WorldObject)));
			typeDick.Add (typeof (SentryMob), new Tuple<WorldObjectType, Type> (WorldObjectType.SentryMob, typeof (MobileWorldObject)));
			typeDick.Add (typeof (Unit), new Tuple<WorldObjectType, Type> (WorldObjectType.Unit, typeof (MobileWorldObject)));
			typeDick.Add (typeof (Building), new Tuple<WorldObjectType, Type> (WorldObjectType.Building, typeof (WorldObject)));
			typeDick.Add (typeof (Tower), new Tuple<WorldObjectType, Type> (WorldObjectType.Tower, typeof (Building)));
			typeDick.Add (typeof (MobTrainer), new Tuple<WorldObjectType, Type> (WorldObjectType.Mobtrainer, typeof (Building)));
			typeDick.Add (typeof (Sentry), new Tuple<WorldObjectType, Type> (WorldObjectType.Sentry, typeof (MobTrainer)));
			typeDick.Add (typeof (NonRelatedUnitTrainer), new Tuple<WorldObjectType, Type> (WorldObjectType.NonRelatedUnitTrainer, typeof (MobTrainer)));
			typeDick.Add (typeof (SpeciesUnitTrainer), new Tuple<WorldObjectType, Type> (WorldObjectType.SpeciesUnitTrainer, typeof (MobTrainer)));
			typeDick.Add (typeof (StrategicPoint), new Tuple<WorldObjectType, Type> (WorldObjectType.StrategicPoint, typeof (MobTrainer)));
			typeDick.Add (typeof (Resource), new Tuple<WorldObjectType, Type> (WorldObjectType.Resource, typeof (StrategicPoint)));
			typeDick.Add (typeof (Monument), new Tuple<WorldObjectType, Type> (WorldObjectType.Monument, typeof (StrategicPoint)));
			// Fill
			foreach (GameObject go in gameObjectsArray) 
			{
				gameObjectDick.Add(go.name, go);
				WorldObject wo = go.GetComponent<WorldObject>();
				if (wo)
				{	
					wo.SetStatsDick ();
					GameManager.baseStatsDick.Add (go.name, new Dictionary<StatsType, float[]> (wo.statsDick));
					List<WorldObjectType> woTypesList = new List<WorldObjectType> ();
					Type currType = wo.GetType ();
					while (!typeDick.ContainsKey (currType)) 
					{
						currType = currType.BaseType;
					}
					while (currType != typeof (WorldObject)) 
					{
						woTypesList.Add (typeDick[currType].thing1);
						currType = typeDick[currType].thing2;
					}
					woTypesList.Add (WorldObjectType.WorldObject);
					// DefenseType - Building is already in typeDick
					if (wo as Building == null && GameManager.baseDefenseTypeArraysDick.ContainsKey (wo.defenseType)) 
					{
						woTypesList.Add (wo.defenseType);
					}
					// AttackStyle
					if (wo.statsDick.ContainsKey (StatsType.RangedStats)) 
					{
						woTypesList.Add (WorldObjectType.Ranged);
					}
					else if (wo.attackArray.Length > 0) 
					{
						woTypesList.Add (WorldObjectType.Melee);
					}
					// Add to speciesWOTList
					if (wo.originalSpecies != Species.NonPlayer) 
					{
						foreach (WorldObjectType wot in woTypesList) 
						{
							speciesWOTDick[wo.originalSpecies][wot].Add (go.name);
						}
					}
					else 
					{
						foreach (Species species in GameManager.speciesArray) 
						{
							foreach (WorldObjectType wot in woTypesList) 
							{
								speciesWOTDick[species][wot].Add (go.name);
							}
						}
					}
				}
				SpriteParent[]spriteParentsArray = go.GetComponentsInChildren<SpriteParent>(true);
				if (spriteParentsArray.Length > 0) 
				{
					SpriteRenderer spriteRenderer = spriteParentsArray[0].GetComponentsInChildren<SpriteRenderer>(true)[0];
					GameManager.mainSpriteDick.Add (go.name, spriteRenderer.sprite);
				}
			}
//			WorldObjectAnalyzer.totalScoreDick = new Dictionary<string, float>();
//			WorldObjectAnalyzer.statsScoreDick = new Dictionary<string, Dictionary<string, float>> ();
//			WorldObjectAnalyzer.CalculateTotalScores(woList);
		} 
		else Destroy(this.gameObject);
	}

	public static void StartDestroyGameObject (GameObject deadGO) 
	{
		 GameManager.GetGameObjectList().StartCoroutine (DestroyGameObject (deadGO));
	}

	private static IEnumerator DestroyGameObject (GameObject deadGO) 
	{
		yield return new WaitForSeconds (GameManager.setGameObjectWaitTime);
		Destroy (deadGO);
	}
}
