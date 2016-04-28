using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class PopGraph : MonoBehaviour 
{
	private ParticleSystem thisParticleSystem;
	private RectTransform psRectTransform;
	public RectTransform canvasRectTrasnform;
	public Image seasonsImage;
	private int day;
	private float dayLength = 1f;
	private float oneYear = 456.25f;
	private float nineMonths = 0.6f;
	private Vector2 startPos;
	private Vector2 endPos;
	private static float particleSize = 1f;
	private Vector3 particleVelocity;
	public static float veggieScalingFactor, animalScalingFactor;
	public static float veggieBase, animalBase;
	private float veggieScreenBase = 0.05f;
	private float animalScreenBase = 0.35f;
	private float veggieScreenSpace = 0.25f;
	private float animalScreenSpace = 0.4f;
	private float veggieGraphMax = 500f;
	private float animalGraphMax = 100f;
	public Image animalAxis;
	public Image veggieAxis;
	private float axisHalfWidth = 0.005f;

	void Start() 
	{
		seasonsImage.rectTransform.sizeDelta = new Vector2 (Screen.width * 8f / 5f, Screen.height * 0.2f);
		veggieScalingFactor = Screen.height * veggieScreenSpace / veggieGraphMax * canvasRectTrasnform.localScale.y;
		veggieBase = -Screen.height * (0.5f - veggieScreenBase) * canvasRectTrasnform.localScale.y;
		veggieAxis.rectTransform.anchorMax = new Vector2 (nineMonths, veggieScreenBase + axisHalfWidth);
		veggieAxis.rectTransform.anchorMin = new Vector2 (0f, veggieScreenBase - axisHalfWidth);
		animalScalingFactor = Screen.height * animalScreenSpace / animalGraphMax * canvasRectTrasnform.localScale.y;
		animalBase = -Screen.height * (0.5f - animalScreenBase) * canvasRectTrasnform.localScale.y;
		animalAxis.rectTransform.anchorMax = new Vector2 (nineMonths, animalScreenBase + axisHalfWidth);
		animalAxis.rectTransform.anchorMin = new Vector2 (0f, animalScreenBase - axisHalfWidth);
		startPos = new Vector2 (-Screen.width * 0.5f, 0f);
		endPos = new Vector2 (-Screen.width * 1.3f, 0f);
		Vector3 distance = Camera.main.ScreenToWorldPoint (new Vector2 (Screen.width, 0f)) - Camera.main.ScreenToWorldPoint(Vector2.zero);
		particleVelocity = new Vector3 (-distance.x / (oneYear * dayLength), 0f, 0f);
		seasonsImage.rectTransform.anchoredPosition = startPos;
		thisParticleSystem = GetComponent<ParticleSystem> ();
		psRectTransform = thisParticleSystem.GetComponent<RectTransform> ();
		psRectTransform.anchoredPosition = endPos;
		StartCoroutine (GraphPopulations ());
	}

	private IEnumerator GraphPopulations() 
	{
		StartCoroutine (MoveParticleSystem ());
		while (true) 
		{
			for (float time = 0f; time < dayLength; time += Time.smoothDeltaTime) 
			{
				Pop_Dynamics_Model.day += Time.smoothDeltaTime / dayLength;
				yield return null;
			}
			Pop_Dynamics_Model.SetPreviousPopDick();
			for (int i = 0; i < Pop_Dynamics_Model.speciesList.Count; i ++) 
			{
				thisParticleSystem.Emit(new Vector3(0f, Pop_Dynamics_Model.Equations(i), 0f), particleVelocity, particleSize, Mathf.Infinity, Pop_Dynamics_Model.speciesColorDick[Pop_Dynamics_Model.speciesList[i]]);
			}
		}
	}

	private IEnumerator MoveParticleSystem() 
	{
		while (Pop_Dynamics_Model.day < oneYear * nineMonths)
		{
			psRectTransform.anchoredPosition = Vector2.Lerp(startPos, new Vector2(Screen.width * 0.5f, 0f), Pop_Dynamics_Model.day / oneYear);
			yield return null;
		}
		StartCoroutine (MoveCalendar ());
	}

	private IEnumerator MoveCalendar() 
	{
		while (true) 
		{
			float elapsedTime = 0f;
			while (elapsedTime < oneYear * dayLength) 
			{
				seasonsImage.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime / (oneYear * dayLength));
				elapsedTime += Time.smoothDeltaTime / 0.8f;
				yield return null;
			}
		}
	}


}
