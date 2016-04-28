using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace RTS 
{

	public static class GameManager {

		private static bool created = false;
		public static int MinZoom { get { return 10; } }
		public static int MaxZoom { get { return 300; } }
		public static float cameraHeight { get { return 30f; } }
		public static float cameraInitialSize { get { return 20f; } }
		public static int LeftCamBound { get { return -50; } }
		public static int RightCamBound { get { return 430; } }
		public static int TopCamBound { get { return 230; } }
		public static int BottomCamBound{ get { return -50; } }
		public static bool GameOver { get; set; }
		public static bool MenuOpen { get; set; }
		public static float mainButtonWidth { get { return 0.09f; } }
		public static float mainButtonHeight { get { return 0.16f; } }
		public static float panelWidth { get { return 0.25f; } }
		public static float doubleTapButtonTime { get { return 1.5f; } }
		public static float setGameObjectWaitTime = 3f;
		public static WaitForSeconds resGatheringTime { get; set; }
		public static WaitForSeconds woWaitTime { get; set; }
		public static Dictionary<Species, Player> playersDick { get; set; }
		public static LayerMask woLayerMask { get; set; }
		public static Dictionary<SpecType, int> specToArrayIndexDick { get; set; }
		public static Dictionary<int, ResourceType> resourceTypeArraytoDick { get; set; }
		public static WorldObjectType[] worldObjectTypeArray { get; set; }
		public static Species[] speciesArray { get; set; }
		public static Dictionary<Eras, int> eraOrderDick { get; set; }
		public static Dictionary<int, Eras> orderEraDick { get; set; }
		public static Dictionary<string, Dictionary<StatsType, float[]>> baseStatsDick { get ; set; }
		public static Dictionary<string, Sprite> mainSpriteDick { get; set; }
		public static Dictionary<string, Dictionary<Eras, Dictionary<StatsType, List<float[]>>>> newEraUpgradesDick { get ; set; }
		public static AttackType[] attackTypeArray { get ; set; }
		public static Dictionary<AttackType, int> attackTypeDickToArray { get ; set; }
		public static Dictionary<WorldObjectType, float[]> baseDefenseTypeArraysDick { get; set; }
		public static Dictionary<string, List<AnimationEventMethod>> animationEventsArray { get; set; }
//		public static Pop_Graph popGraph { get; set;}
		public static CheckButton checkButton { get; set; }
		public static CancelButton cancelButton { get; set; }
		public static HeroSelectButton heroSelectButton { get; set; }
		public static HUD Hud { get; set; }
		public static EvolutionPanel evoPanel { get; set; }
//		public static UAHPanel uahPanel { get; set; }
		public static Player HumanPlayer { get; set; }
//		public static UAHminiPanel uahMiniPanel { get; set; }
//		public static Disasters disasters { get; set;}
		public static float gravityAngle = 45f;
		public static BattleController battleController { get; set; }
		public static bool inBattleGround { get; set; }
		public static bool inPuzzleScene { get; set; }
		private static GameObjectList gameObjectList;
		
		public static void Initiate() 
		{
			if (!created)
			{
				created = true;
				gravityAngle *= Mathf.Deg2Rad;
				float gravityMag = Physics.gravity.magnitude;
				Physics.gravity = new Vector3 (0f, -Mathf.Cos (gravityAngle) * gravityMag, -Mathf.Sin (gravityAngle) * gravityMag);
				// NavMesh.avoidancePredictionTime has the potential to affect the performance a lot
				NavMesh.avoidancePredictionTime = 4f;
				playersDick = new Dictionary<Species, Player>();
				resGatheringTime = new WaitForSeconds (5);
				woWaitTime = new WaitForSeconds (1f);
				mainSpriteDick = new Dictionary<string, Sprite> ();
				woLayerMask = LayerMask.GetMask(new string[] {"Bunnies", "Deer", "Sheep", "Wolves", "NonPlayer"});
				specToArrayIndexDick = new Dictionary<SpecType, int>{{SpecType.Nature, 0}, {SpecType.Sun, 1}, {SpecType.Stick, 2}, {SpecType.Rock, 3}, {SpecType.Wheel, 4},{SpecType.Fire, 5}};
				resourceTypeArraytoDick = new Dictionary<int, ResourceType> {{0, ResourceType.Gold}, {1,ResourceType.Wood}, {2, ResourceType.Unique}};
				worldObjectTypeArray = new WorldObjectType[] {WorldObjectType.WorldObject, WorldObjectType.Building, WorldObjectType.Tower, WorldObjectType.Mobtrainer, WorldObjectType.Sentry, WorldObjectType.NonRelatedUnitTrainer, WorldObjectType.SpeciesUnitTrainer, WorldObjectType.StrategicPoint, WorldObjectType.Resource, WorldObjectType.Monument, WorldObjectType.Mobile, WorldObjectType.SentryMob, WorldObjectType.Unit, WorldObjectType.Melee, WorldObjectType.Ranged, WorldObjectType.LightUnit, WorldObjectType.HeavyUnit, WorldObjectType.Siege};
				speciesArray = new Species[] {Species.Bunnies, Species.Deer, Species.Sheep, Species.Wolves};
				eraOrderDick = new Dictionary<Eras, int> {{Eras.StoneAge, 1}, {Eras.Classical, 2}, {Eras.Renaissance, 3}, {Eras.Industrial, 4}, {Eras.Information, 5}};
				orderEraDick = new Dictionary<int, Eras> {{1, Eras.StoneAge}, {2, Eras.Classical}, {3, Eras.Renaissance}, {4, Eras.Industrial}, {5, Eras.Information}};
				attackTypeArray = new AttackType[] {AttackType.Blunt, AttackType.Pierce, AttackType.Crush, AttackType.Incendiary, AttackType.Psychological};
				attackTypeDickToArray = new Dictionary<AttackType, int> {{AttackType.Blunt, 0}, {AttackType.Pierce, 1}, {AttackType.Crush, 2}, {AttackType.Incendiary, 3}, {AttackType.Psychological, 4}};
				baseStatsDick = new Dictionary<string, Dictionary<StatsType, float[]>>();
				newEraUpgradesDick = new Dictionary<string, Dictionary<Eras, Dictionary<StatsType, List<float[]>>>> ();
				baseDefenseTypeArraysDick = new Dictionary<WorldObjectType, float[]> 
				{
					{WorldObjectType.LightUnit, new float[] {0.1f, -0.4f, 0f, -0.5f, -0.2f}},
					{WorldObjectType.HeavyUnit, new float[] {0.6f, 0.1f, 0.3f, -0.5f, -1f}},
					{WorldObjectType.Siege, new float[] {-0.1f, 0.99f, 0f, -0.5f, 1f}},
					{WorldObjectType.Building, new float[] {0.4f, 0.99f, -0.8f, -0.5f, 1f}}
				};
				animationEventsArray = new Dictionary<string, List<AnimationEventMethod>> ();
			}
		}

		// fucking pointers...
		public static float[] DuplicateArray (float[] statsArray) 
		{
			if (statsArray != null) 
			{
				float[] duplicateArray = new float[statsArray.Length];
				for (int i = 0; i < statsArray.Length; i ++) 
				{
					duplicateArray[i] = statsArray[i];
				}
				return duplicateArray;
			}
			return new float[0];
		}


		public static bool FingerInBounds (Vector2 touchPosition)
		{
//			On Top Left Corner Buttons
			if (touchPosition.x < Screen.width * /* 2 * */ mainButtonWidth && touchPosition.y > Screen.height * (1 - mainButtonHeight)) 
			{
				return false;
			}

//			On Top Right Corner Button
			else if (touchPosition.x > Screen.width * (1 - mainButtonWidth) && touchPosition.y > Screen.height * (1 - mainButtonHeight))
			{
				return false;
			}

//			On Bottom Right Corner
			else if (touchPosition.x > Screen.width * (1 - mainButtonWidth) && touchPosition.y < Screen.height * mainButtonHeight) 
			{
				return false;			
			}
//			On Bottom LeftCorner
			else if (touchPosition.x < Screen.width * mainButtonWidth && touchPosition.y < Screen.height * mainButtonHeight)
			{
				return false;
			}
//			On Open uahMiniPanel
//			else if (uahMiniPanel.gameObject.activeSelf & touch.position.x > Screen.width * uahMiniPanel.recTransform.anchorMin.x && touch.position.x < Screen.width * uahMiniPanel.recTransform.anchorMax.x && touch.position.y > Screen.height * uahMiniPanel.recTransform.anchorMin.y && touch.position.y < Screen.height * uahMiniPanel.recTransform.anchorMax.y) 
//			{
//				return false;
//			}
			return true;
		}

		public static void SetGameObjectList(GameObjectList objectList) 
		{
			gameObjectList = objectList;
		}

		public static GameObjectList GetGameObjectList () 
		{
			return gameObjectList;
		}
		
		public static GameObject GetGameObject(string name) 
		{
			return gameObjectList.gameObjectDick [name];
		}

		public static GameObject[] GetWorldObjectArray() 
		{
			return gameObjectList.gameObjectsArray;
		}

		public static List<string> GetSpeciesWOTList(Species species, WorldObjectType wot) 
		{
			return gameObjectList.speciesWOTDick [species] [wot];
		}

		public static List<string> GetSpeciesAttackTypeList(Species species, AttackType attackType) 
		{
			return gameObjectList.speciesAttackTypesDick [species] [attackType];
		}
	}
}

public delegate void AnimationEventMethod ();
