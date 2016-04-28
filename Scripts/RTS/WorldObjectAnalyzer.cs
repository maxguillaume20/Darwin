using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RTS 
{
	public class WorldObjectAnalyzer  
	{
//		public static Dictionary<string, float> totalScoreDick { get; set; }
//		public static Dictionary<string, Dictionary<string, float>> statsScoreDick { get; set; }                  
//		// Base Defenses
//		public static float[] buildingDefArray { get { return new float[] {1.5f, 4f, 0.5f, 0.75f, Mathf.Infinity}; } }
//		public static float[] lightDefArray { get { return new float[] {1f, 0.5f, 1.5f, 0.75f, 1f}; } }
//		public static float[] heavyDefArray { get { return new float[] {2f, 0.75f, 1.5f, 1f, 0.33f}; } }
//
//		// Subjective Variables 
//		private static float movementCoefficient = 0.5f;
//		private static float buildingPsychDef = 1f;																												
//
//		public static void CalculateTotalScores(List<WorldObject> woList) 
//		{
//			// Initializing Shit
//			List<string> woNameList = new List<string> ();
////			Dictionary<string, Dictionary<StatsType, float>> woStoredStatsDick = new Dictionary<string, Dictionary<StatsType, float>> ();
//			string[] scoreStatsArray = new string[] {
//								"AttackWO",
//								"AttackB", 
//								"AttackLU", 
//								"AttackHU",
//								"Defense"
//						};
//			Dictionary<string, float> healthDick = new Dictionary<string, float> ();
//			Dictionary<string, float> damageDick = new Dictionary<string, float> ();
//			Dictionary<string, float> attackSpeedDick = new Dictionary<string, float> ();
//			Dictionary<string, float> rangeDick = new Dictionary<string, float> ();
//			Dictionary<string, float> movementVelocityDick = new Dictionary<string, float> ();
//			Dictionary<string, float> projectileVelocityDick = new Dictionary<string, float> ();
//			Dictionary<string, List<AttackType>> woAttackTypeDick = new Dictionary<string, List<AttackType>> ();
//			Dictionary<AttackType, float[]> woAttackTypeSumCountDick = new Dictionary<AttackType, float[]> ();
//			Dictionary<AttackType, float> defenseTypeSumDick = new Dictionary<AttackType, float> ();
//			Dictionary<string, Dictionary<AttackType, float[]>> attackDefenseWoTypeDick = new Dictionary<string, Dictionary<AttackType, float[]>> ();
//			foreach (AttackType attackType in GameManager.attackTypeArray) 
//			{
//				woAttackTypeSumCountDick.Add(attackType, new float[2]);
//				defenseTypeSumDick.Add(attackType, 0f);
//			}
//			int woAttackCount = 0;
//			int rangeCount = 0;
//			int unitCount = 0;
//			foreach (WorldObject wo in woList) 
//			{
//				woNameList.Add(wo.name);
//				totalScoreDick.Add (wo.name, 0f);
//				statsScoreDick.Add (wo.name, new Dictionary<string, float>());
//				foreach (string statsType in scoreStatsArray) 
//				{
//					statsScoreDick[wo.name].Add(statsType, 0f);
//				}
//				healthDick.Add(wo.name, 0f);
//				damageDick.Add(wo.name, 0f);
//				attackSpeedDick.Add(wo.name, 0f);
//				rangeDick.Add(wo.name, 1f);
//				movementVelocityDick.Add(wo.name, 0f);
//				projectileVelocityDick.Add(wo.name, Mathf.Infinity);
//				woAttackTypeDick.Add (wo.name, new List<AttackType>());
//				attackDefenseWoTypeDick.Add (wo.name, new Dictionary<AttackType, float[]>());
//				foreach(AttackType attackType in GameManager.attackTypeArray) 
//				{
//					attackDefenseWoTypeDick[wo.name].Add(attackType, new float[2]);
//				}
//				// Starts Crunching
//				
//				// Health
//				healthDick[wo.name] = wo.healthArray[1];
//	
//				// Attack 
//				if (wo.attackArray.Length > 0)
//				{
//					woAttackCount ++;
//					damageDick[wo.name] = wo.attackArray[0];
//					attackSpeedDick[wo.name] = wo.attackArray[1];
//					foreach (AttackType attackType in wo.attackTypeList) 
//					{
//						woAttackTypeDick[wo.name].Add(attackType);
//						woAttackTypeSumCountDick[attackType][0] += damageDick[wo.name];
//						woAttackTypeSumCountDick[attackType][1] += 1f / (float) wo.attackTypeList.Count;
//					}
//				}
//
//				// Defense (Buildings)
//				if (wo as Building) 
//				{
//					for (int i = 0; i < wo.defenseArray.Length - 1; i ++) 
//					{
//						attackDefenseWoTypeDick[wo.name][GameManager.attackTypeArray[i]][1] = wo.defenseArray[GameManager.attackTypeDickToArray[GameManager.attackTypeArray[i]]];
//						defenseTypeSumDick[GameManager.attackTypeArray[i]] += attackDefenseWoTypeDick[wo.name][GameManager.attackTypeArray[i]][1];
//					}
//					attackDefenseWoTypeDick[wo.name][AttackType.Psychological][1] = buildingPsychDef;
//					defenseTypeSumDick[AttackType.Psychological] += buildingPsychDef;
//
//					// Range (Towers)
//					Tower tower = wo as Tower;
//					if (tower) 
//					{
//						rangeCount ++;
//						rangeDick[wo.name] = tower.towerStatsArray[0];
//						projectileVelocityDick[wo.name] = tower.towerStatsArray[1];
//					}
//				}
//
//				else if (wo as Unit)
//				{
//					foreach (AttackType attackType in GameManager.attackTypeArray) 
//					{
//						attackDefenseWoTypeDick[wo.name][attackType][1] = wo.defenseArray[GameManager.attackTypeDickToArray[attackType]];
//						defenseTypeSumDick[attackType] += attackDefenseWoTypeDick[wo.name][attackType][1];
//					}		
//
//					// Range (Units)
//					RangedUnit rangedUnit = wo as RangedUnit;
//					if (rangedUnit) 
//					{
//						rangeCount ++;
//						rangeDick[wo.name] = rangedUnit.towerStatsArray[0];
//						projectileVelocityDick[wo.name] = rangedUnit.towerStatsArray[1];
//					}
//
//					// Movement  
//					Unit unit = wo as Unit;
//					unitCount ++;
//					movementVelocityDick[wo.name] = unit.unitStatsArray[0];
//				}
//			}
//
//			// Attack and Defense Averages for each Type
//			Dictionary<AttackType, float[]> woAttackDefenseTypeMeanDick = new Dictionary<AttackType, float[]>();
//			foreach (AttackType attackType in GameManager.attackTypeArray) 
//			{
//				woAttackDefenseTypeMeanDick.Add(attackType, new float[2]);
//				woAttackDefenseTypeMeanDick[attackType][0] = woAttackTypeSumCountDick[attackType][0] * woAttackTypeSumCountDick[attackType][1] / woAttackCount;
//				woAttackDefenseTypeMeanDick[attackType][1] = defenseTypeSumDick[attackType] / (float)woList.Count;
//			}
//
//			// Averages for Movement, Range, and ProjectileVelocity
//			float woAttackSpeedAverage = GetAverage (attackSpeedDick, woAttackCount);
//			float movementTotalAverage = GetAverage(movementVelocityDick, woNameList.Count);
//			float unitMovementAverage = GetAverage (movementVelocityDick, unitCount);
//			float rangeWeightedAverage = GetAverage (rangeDick, rangeCount);
//			float projectileWeightedVelocity = GetAverage (projectileVelocityDick, rangeCount);
//
//			// Calculate Scores
//			foreach (string woName in woNameList) 
//			{
//
//				// Attack 
//				float[] woDefArray = new float[5];
//				foreach (AttackType attackType in GameManager.attackTypeArray) 
//				{
//					woDefArray[GameManager.attackTypeDickToArray[attackType]] = woAttackDefenseTypeMeanDick[attackType][1];
//				}
//				SetAverageAttack(woName, "AttackWO", woDefArray, woAttackTypeDick[woName], damageDick[woName], attackSpeedDick[woName]);
//				SetAverageAttack(woName, "AttackB", buildingDefArray, woAttackTypeDick[woName], damageDick[woName], attackSpeedDick[woName]);
//				SetAverageAttack(woName, "AttackLU", lightDefArray, woAttackTypeDick[woName], damageDick[woName], attackSpeedDick[woName]);
//				SetAverageAttack(woName, "AttackHU", heavyDefArray, woAttackTypeDick[woName], damageDick[woName], attackSpeedDick[woName]);
//				// Defense
//				float averageAttack = 0f;
//				foreach (AttackType attackType in GameManager.attackTypeArray) 
//				{
//					averageAttack += woAttackDefenseTypeMeanDick[attackType][0] / attackDefenseWoTypeDick[woName][attackType][1];
//				}
//				statsScoreDick[woName]["Defense"] = healthDick[woName] * (1 + movementCoefficient * movementVelocityDick[woName] * movementVelocityDick[woName] / projectileWeightedVelocity) - averageAttack;
////				Debug.Log (woName + "     Defense: " + (statsScoreDick[woName]["Defense"]).ToString("#.00") + "     Attack vs Average WorldObject: " + (statsScoreDick[woName]["AttackWO"]).ToString("#.00") + "      vs Building: " + (statsScoreDick[woName]["AttackB"]).ToString("#.00")  + "     vs Light Unit: " + (statsScoreDick[woName]["AttackLU"]).ToString("#.00")  + "     vs Heavy Unit: " + (statsScoreDick[woName]["AttackHU"]).ToString("#.00"));
//			}
//		}
//
//		private static float GetAverage(Dictionary<string, float> woDick, int count) 
//		{
//			float average = 0f;
//			foreach (string woName in woDick.Keys) 
//			{
//				average += woDick[woName];
//			}
//			return average / (float)count;
//		}
//
//		private static void SetAverageAttack(string woName, string attackAgainst, float[] defenseArray, List<AttackType> woAttackTypeList, float damage, float attackSpeed) 
//		{
//			float attackDamage = 0f;
//			foreach (AttackType attackType in woAttackTypeList) 
//			{
//				attackDamage += damage / defenseArray[GameManager.attackTypeDickToArray[attackType]];
//			}
//			if (woAttackTypeList.Count > 0) 
//			{
//				statsScoreDick[woName][attackAgainst] = attackDamage * attackSpeed / (float)woAttackTypeList.Count;
//			}
//			else statsScoreDick[woName][attackAgainst] = 0f;
//		}
	}
}
