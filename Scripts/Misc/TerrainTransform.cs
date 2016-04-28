using UnityEngine;
using System.Collections;

public class TerrainTransform : MonoBehaviour 
{
	private void Awake () 
	{
		SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer sR in spriteRenderers) 
		{
			if (sR.transform.localEulerAngles.x == 90f) 
			{
				sR.transform.localEulerAngles = new Vector3 (HUD.xCameraRotation, sR.transform.localEulerAngles.y, sR.transform.localEulerAngles.z);
			}
			else 
			{
				sR.transform.localEulerAngles = new Vector3 (-HUD.xCameraRotation, sR.transform.localEulerAngles.y, sR.transform.localEulerAngles.z);
			}
		}
	}
}
