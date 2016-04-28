using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public class PopulationPanel : MonoBehaviour 
{
	public bool showingStats;
	private Species humanSpecies;
	public List<Image> legendImages;
	public List<Text> legendTexts;
	public Text[] statsTexts;
	public List<Image> statsImages;
	public List<Text> statsValuesList;
	private StatsType[] statsOrderArray = new StatsType[] {StatsType.BirthRates, StatsType.DeathRates, StatsType.ProtectionRates, StatsType.Nutrients, StatsType.Interference};
//	private Species[] veggieArray = new Species[] {Species.Grass, Species.Carrots};
//	private StatsType[] veggieStatsArray = new StatsType[] {StatsType.BirthRates, StatsType.Nutrients};
	private Dictionary<StatsType, Text> speciesValuesTextDick = new Dictionary<StatsType, Text> ();
	private Dictionary<StatsType, float> speciesValuesFloatDick = new Dictionary<StatsType, float> ();
	private Dictionary<Species, Text> consumptionTextDick = new Dictionary<Species, Text> ();
	private Dictionary<Species, float> consumptionFloatDick = new Dictionary<Species, float> ();
	private int humanPopulation;
	private float wolfPredation;
//	private Dictionary<Species, Dictionary<StatsType, Text>> veggieValuesTextDick = new Dictionary<Species, Dictionary<StatsType, Text>> ();
//	private Dictionary<Species, Dictionary<StatsType, float>> veggieValuesFloatDick = new Dictionary<Species, Dictionary<StatsType, float>> ();
	public Button switchButton;

	private void Start() 
	{
		humanSpecies = GameManager.HumanPlayer.species;
		statsTexts [0].text = humanSpecies.ToString ();
		statsImages [0].sprite = HUD.speciesPopSpriteDick [humanSpecies];
		// Adjust Species Legend
		Dictionary<Species, Tuple<Image, Text>> legendDick = new Dictionary<Species, Tuple<Image, Text>> ();
		for (int i = 0; i < Pop_Dynamics_Model.allSpeciesArray.Length; i ++) 
		{
			legendImages[i].color = Pop_Dynamics_Model.speciesColorDick[Pop_Dynamics_Model.allSpeciesArray[i]];
			legendTexts[i].text = Pop_Dynamics_Model.allSpeciesArray[i].ToString ();
			legendDick.Add (Pop_Dynamics_Model.allSpeciesArray[i], new Tuple<Image, Text> (legendImages[i], legendTexts[i]));
		}
		foreach (Species species in legendDick.Keys) 
		{
			if (!Pop_Dynamics_Model.speciesList.Contains (species)) 
			{
				legendDick[species].thing1.gameObject.SetActive (false);
				legendImages.Remove (legendDick[species].thing1);
				legendDick[species].thing2.gameObject.SetActive (false);
				legendTexts.Remove (legendDick[species].thing2);
				legendDick[Species.Wolves].thing1.rectTransform.anchorMax = legendDick[species].thing1.rectTransform.anchorMax;
				legendDick[Species.Wolves].thing1.rectTransform.anchorMin = legendDick[species].thing1.rectTransform.anchorMin;
				legendDick[Species.Wolves].thing2.rectTransform.anchorMax = legendDick[species].thing2.rectTransform.anchorMax;
				legendDick[Species.Wolves].thing2.rectTransform.anchorMin = legendDick[species].thing2.rectTransform.anchorMin;
			}
		}
		// Consumption
		consumptionTextDick.Add (Species.Grass, statsValuesList[statsValuesList.Count - 2]);
		consumptionFloatDick.Add (Species.Grass, Pop_Dynamics_Model.eatRatesDick [humanSpecies] [Species.Grass]);
		consumptionTextDick [Species.Grass].text = consumptionFloatDick [Species.Grass].ToString ("0.###");
		if (humanSpecies == Species.Sheep) 
		{
			// carrot consumption shit is at the end of both statsImages and statsValuesList
			statsImages.RemoveAt(statsImages.Count - 1);
			statsImages[statsImages.Count - 1].rectTransform.anchorMin = new Vector2 (0.2f, statsImages[statsImages.Count - 1].rectTransform.anchorMin.y);
			statsImages[statsImages.Count - 1].rectTransform.anchorMax = new Vector2 (0.3f, statsImages[statsImages.Count - 1].rectTransform.anchorMax.y);
			statsValuesList.RemoveAt(statsValuesList.Count - 1);
			statsValuesList[statsValuesList.Count - 1].rectTransform.anchorMin = new Vector2 (0.4f, statsValuesList[statsValuesList.Count - 1].rectTransform.anchorMin.y);
			statsValuesList[statsValuesList.Count - 1].rectTransform.anchorMax = new Vector2 (0.6f, statsValuesList[statsValuesList.Count - 1].rectTransform.anchorMax.y);
		}
		else 
		{
			consumptionTextDick.Add (Species.Carrots, statsValuesList[statsValuesList.Count - 1]);
			consumptionFloatDick.Add (Species.Carrots, Pop_Dynamics_Model.eatRatesDick [humanSpecies] [Species.Carrots]);
			consumptionTextDick [Species.Carrots].text = consumptionFloatDick [Species.Carrots].ToString ("0.###");
		}
		// Stats in Pop_Dynmaics_Model.modelStatsDick
		humanPopulation = (int)Pop_Dynamics_Model.modelStatsDick [humanSpecies] [StatsType.Population];
		speciesValuesTextDick.Add (StatsType.Population, statsValuesList[0]);
		speciesValuesTextDick[StatsType.Population].text = humanPopulation.ToString();
		for (int i = 0; i < statsOrderArray.Length; i ++) 
		{
			speciesValuesFloatDick.Add (statsOrderArray[i], Pop_Dynamics_Model.modelStatsDick [humanSpecies][statsOrderArray[i]]);
			speciesValuesTextDick.Add (statsOrderArray[i], statsValuesList[i + 1]);
			speciesValuesTextDick[statsOrderArray[i]].text = speciesValuesFloatDick[statsOrderArray[i]].ToString("0.###");
		}
		// Predation
		wolfPredation = Pop_Dynamics_Model.eatRatesDick [Species.Wolves] [humanSpecies];
		speciesValuesTextDick.Add (StatsType.EatRates, statsValuesList [6]);
		speciesValuesTextDick [StatsType.EatRates].text = wolfPredation.ToString ("0.###");
	}

	public void ShowStats() 
	{
		foreach (Image image in legendImages) image.gameObject.SetActive(false);
		foreach (Text text in legendTexts) text.gameObject.SetActive(false);
		foreach (Text text in statsTexts) text.gameObject.SetActive(true);
		foreach (Image image in statsImages) image.gameObject.SetActive(true);
		foreach (Text text in statsValuesList) text.gameObject.SetActive(true);
		StartCheckStats ();
		switchButton.GetComponent<RectTransform>().Rotate(new Vector3 (0f, 0, 180f));
		switchButton.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.Off);
		switchButton.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
	}

	public void ShowLegend() 
	{
		foreach (Image image in legendImages) image.gameObject.SetActive(true);
		foreach (Text text in legendTexts) text.gameObject.SetActive(true);
		foreach (Text text in statsTexts) text.gameObject.SetActive(false);
		foreach (Image image in statsImages) image.gameObject.SetActive(false);
		foreach (Text text in statsValuesList) text.gameObject.SetActive(false);
		showingStats = false;
		switchButton.GetComponent<RectTransform>().Rotate(new Vector3 (0f, 0f, 180f));
		switchButton.onClick.SetPersistentListenerState (1, UnityEngine.Events.UnityEventCallState.Off);
		switchButton.onClick.SetPersistentListenerState (0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
	}

	public void StartCheckStats () 
	{
		showingStats = true;
		StartCoroutine (CheckStats ());
	}

	private IEnumerator CheckStats() 
	{
		Species[] preyArray = consumptionFloatDick.Keys.ToArray ();
		while (showingStats && PopulationsButton.popGraphOpen) 
		{
			if (humanPopulation != (int) Pop_Dynamics_Model.modelStatsDick[humanSpecies][StatsType.Population]) 
			{
				humanPopulation = (int) Pop_Dynamics_Model.modelStatsDick[humanSpecies][StatsType.Population];
				speciesValuesTextDick[StatsType.Population].text = humanPopulation.ToString();
			}
			foreach (StatsType statsType in statsOrderArray) 
			{
				if (speciesValuesFloatDick[statsType] != Pop_Dynamics_Model.modelStatsDick[humanSpecies][statsType]) 
				{
					speciesValuesFloatDick[statsType] = Pop_Dynamics_Model.modelStatsDick[humanSpecies][statsType];
					speciesValuesTextDick[statsType].text = speciesValuesFloatDick[statsType].ToString("0.###");
				}
			}
			foreach (Species prey in preyArray) 
			{
				if (consumptionFloatDick[prey] != Pop_Dynamics_Model.eatRatesDick[humanSpecies][prey]) 
				{
					consumptionFloatDick[prey] = Pop_Dynamics_Model.eatRatesDick[humanSpecies][prey];
					consumptionTextDick[prey].text = consumptionFloatDick[prey].ToString("0.###");
				}
			}
			if (wolfPredation != Pop_Dynamics_Model.eatRatesDick[Species.Wolves][humanSpecies]) 
			{
				wolfPredation = Pop_Dynamics_Model.eatRatesDick[Species.Wolves][humanSpecies];
				speciesValuesTextDick[StatsType.EatRates].text = wolfPredation.ToString("0.###");
			}
			yield return null;
		}
	}
}
