using UnityEngine;
using System.Collections;
//using RTS;

namespace RTS 
{
	public class SelectBox : MonoBehaviour
	{
		public static bool isActive;
		private static GameObject thisGameObject;
		private static float lineYAdjustment;
		private static float lineZAdjustment;
		private static LineRenderer[] linesArray { get; set; }
		
		private void Awake () 
		{
			lineYAdjustment = Mathf.Cos (Mathf.Deg2Rad * ((90f - Camera.main.transform.rotation.eulerAngles.x))) * (GameManager.cameraHeight / 3f);
			lineZAdjustment = Mathf.Sin (Mathf.Deg2Rad * (90f - Camera.main.transform.rotation.eulerAngles.x)) * (GameManager.cameraHeight / 3f);
			transform.rotation = Quaternion.identity;
			thisGameObject = gameObject;
			linesArray = new LineRenderer[4];
			for (int i = 0; i < linesArray.Length; i ++) 
			{
				linesArray[i] = ((GameObject) Instantiate (GameManager.GetGameObject("StraightLine"), transform.position, Quaternion.identity)).GetComponent<LineRenderer>();
				linesArray[i].transform.SetParent (this.transform);
				linesArray[i].SetColors (Color.white, Color.white);
			}  
			gameObject.SetActive (false);
		}
		
		public static void Enable () 
		{
			isActive = true;
			thisGameObject.SetActive (true);
		}

		public static void Disable () 
		{
			isActive = false;
			thisGameObject.SetActive (false);
		}
		
		public static void AdjustSize (Vector3 anchorMin, Vector3 anchorMax) 
		{
			linesArray [0].SetPosition (0, new Vector3 (anchorMin.x, anchorMin.y - lineYAdjustment, anchorMin.z + lineZAdjustment));
			linesArray [0].SetPosition (1, new Vector3 (anchorMin.x, anchorMax.y - lineYAdjustment, anchorMax.z + lineZAdjustment));
			linesArray [1].SetPosition (0, new Vector3 (anchorMin.x, anchorMax.y - lineYAdjustment, anchorMax.z + lineZAdjustment));
			linesArray [1].SetPosition (1, new Vector3 (anchorMax.x, anchorMax.y - lineYAdjustment, anchorMax.z + lineZAdjustment));
			linesArray [2].SetPosition (0, new Vector3 (anchorMax.x, anchorMax.y - lineYAdjustment, anchorMax.z + lineZAdjustment));
			linesArray [2].SetPosition (1, new Vector3 (anchorMax.x, anchorMin.y - lineYAdjustment, anchorMin.z + lineZAdjustment));
			linesArray [3].SetPosition (0, new Vector3 (anchorMax.x, anchorMin.y - lineYAdjustment, anchorMin.z + lineZAdjustment));
			linesArray [3].SetPosition (1, new Vector3 (anchorMin.x, anchorMin.y - lineYAdjustment, anchorMin.z + lineZAdjustment));
		}
	}

}
