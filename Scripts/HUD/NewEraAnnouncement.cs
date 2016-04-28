using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class NewEraAnnouncement : MonoBehaviour 
{
	public Image imagina;
	private Text textical;
	private float newEraAnnouncementTimer = 2f;

	private void Awake () 
	{
		imagina.gameObject.SetActive (true);
		textical = imagina.GetComponentInChildren<Text> ();
		imagina.gameObject.SetActive (false);
	}

	public IEnumerator NewEraBitches(Eras newEra) 
	{
		gameObject.SetActive (true); 
		textical.text = newEra.ToString () + " Era";
		for (float i = 0; i < newEraAnnouncementTimer; i += Time.deltaTime) 
		{
			yield return null; 
		}
		gameObject.SetActive (false); 
	}

	private void OnEnable() 
	{
		imagina.gameObject.SetActive (true);
	}

	private void OnDisable () 
	{
		textical.text = "";
		imagina.gameObject.SetActive (false);
	}
}
