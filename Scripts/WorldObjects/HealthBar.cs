using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour 
{
	public GameObject background;
	public GameObject healthbar;
	private float maxScale = 60f;
	private float expScale = 1.14f;
	private float halfMaxScaleHealth = 1000f;
	public SpriteRenderer background_SPrenderer;
	public SpriteRenderer healthbar_SPrenderer;
	private WorldObject worldObject;

	public void SetWorldObject (WorldObject wo)
	{
		worldObject = wo;
		ResetBar ();
	}

	public void ResetBar () 
	{
		float bXScale = maxScale * Mathf.Pow (worldObject.healthArray [1], expScale) / (Mathf.Pow (worldObject.healthArray [1], expScale) + Mathf.Pow (halfMaxScaleHealth, expScale));
		float hXScale = maxScale * Mathf.Pow (worldObject.healthArray [0], expScale) / (Mathf.Pow (worldObject.healthArray [0], expScale) + Mathf.Pow (halfMaxScaleHealth, expScale));
		background.gameObject.transform.localScale = new Vector3 (bXScale, background.gameObject.transform.localScale.y, background.gameObject.transform.localScale.z);
		healthbar.transform.localScale =  new Vector3 (hXScale, background.gameObject.transform.localScale.y, background.gameObject.transform.localScale.z);
		healthbar.transform.Translate (new Vector3 (background_SPrenderer.bounds.min.x - healthbar_SPrenderer.bounds.min.x, 0f, 0f));
	}

	public void ChangeHP (float currHitpoints)
	{
		float hXScale = 0f;
		if (worldObject.healthArray[0] > 0) 
		{
			hXScale = maxScale * Mathf.Pow (worldObject.healthArray [0], expScale) / (Mathf.Pow (worldObject.healthArray [0], expScale) + Mathf.Pow (halfMaxScaleHealth, expScale));
		}
		healthbar.transform.localScale =  new Vector3 (hXScale, healthbar.gameObject.transform.localScale.y,healthbar.gameObject.transform.localScale.z);
		healthbar.transform.Translate (new Vector3 (background_SPrenderer.bounds.min.x - healthbar_SPrenderer.bounds.min.x, 0f, 0f));
		if (!gameObject.activeSelf) gameObject.SetActive(true);
	}
}
