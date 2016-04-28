using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RTS
{
	public class Pop_Dynamics_Model : MonoBehaviour
	{
		public static Species[] allSpeciesArray { get; set; }
		public static List<Species> speciesList { get; set; }
		public static Dictionary<Species, Dictionary<StatsType, float>> modelStatsDick { get; set; }
		private static Dictionary<Species, Dictionary<StatsType, float>> baseModelStatsDick { get; set;}
		public static Dictionary<Species, Dictionary<Species, float>> eatRatesDick { get; set; }
		private static Dictionary<Species, Dictionary<Species, float>> baseEatRatesDick { get; set; }
		private static Dictionary<Species, List<Species>> predationDick { get; set; }
		private static Dictionary<Species, List<Species>> interferenceDick { get; set; }
		public static Dictionary<Species, float> seasonalGrowthRates { get; set; }
		public static Dictionary<Species, float> prevPopulationsDick { get; set; }
		public static Dictionary<Species, Color> speciesColorDick { get; set; }  
		private static float seasonal_year;
		private static float seasonalDay;
		public static float day;
		// time_step can also be thought of as the overall robustness of the system
		private static float time_step = 0.01f;

		private void Awake() 
		{	

			// Initiate modelStatsDick
			//
			//	[0] = bunnies, [1] = deer, [2] = sheep, [3] = wolves, [4] = grass, [5] = carrots
			float[] populationsArray = new float[] {50f, 30f, 40f, 10, 499.9f, 99.9f};
			float[] birthRatesArray = new float[] {0.14f, 0.05f, 0.065f, 0.04f, 9, 14};
			//	[0] = bunnies, [1] = deer, [2] = sheep, [3] = wolves, [4] = grass carrying capacity, [5] = carrots carrying capacity
			float[] deathArray = new float[] {0.08f, 0.015f, 0.04f, 0.01f, 500f, 100f};
			//	[0] = bunnies, [1] = deer, [2] = sheep, [3] = wolves [4] = Grass, [5] = Carrots
			float[] protectionRatesArray = new float[] {25f, 25f, 25f, 0f, 40f, 15f};
			float[] nutrientsArray = new float[] {0.5f, 1.2f, 1.25f, 0f, 1f, 1.1f};
			//	[0] = bunnies, [1] = deer, [2] = sheep
			float[] interferenceArray = new float[] {0f, 0f, 0f, 0f, 0f, 0f};
			float Eat_BG = 0.8f, Eat_BC = 1.3f, Eat_DG = 4.5f, Eat_DC = 0.5f, Eat_SG = 6f, Eat_WB = 0.5f, Eat_WD = 0.58f, Eat_WS = 0.9f;
			// normalizes one year/period (2*Pi) to 365 days
			seasonal_year = 365f / (2f * Mathf.PI);
			modelStatsDick = new Dictionary<Species, Dictionary<StatsType, float>> ();
			prevPopulationsDick = new Dictionary<Species, float> ();
			eatRatesDick = new Dictionary<Species, Dictionary<Species, float>>();
			// the rest of speciesList is made by PlayerManager
			speciesList.Add (Species.Wolves);
			speciesList.Add (Species.Grass);
			speciesList.Add (Species.Carrots);
			allSpeciesArray = new Species[] {Species.Bunnies, Species.Deer, Species.Sheep, Species.Wolves, Species.Grass, Species.Carrots}; 
			for (int i = 0; i < allSpeciesArray.Length; i ++) 
			{
				modelStatsDick.Add(allSpeciesArray[i], new Dictionary<StatsType, float>());
				modelStatsDick[allSpeciesArray[i]].Add(StatsType.Population, populationsArray[i]);
				prevPopulationsDick.Add(allSpeciesArray[i], populationsArray[i]);
				modelStatsDick[allSpeciesArray[i]].Add(StatsType.BirthRates, birthRatesArray[i]);
				modelStatsDick[allSpeciesArray[i]].Add(StatsType.DeathRates, deathArray[i]);
				modelStatsDick[allSpeciesArray[i]].Add(StatsType.ProtectionRates, protectionRatesArray[i]);
				modelStatsDick[allSpeciesArray[i]].Add(StatsType.Nutrients, nutrientsArray[i]);
				modelStatsDick[allSpeciesArray[i]].Add(StatsType.Interference, interferenceArray[i]);
				eatRatesDick.Add(allSpeciesArray[i], new Dictionary<Species, float>());
			}
			baseModelStatsDick = new Dictionary<Species, Dictionary<StatsType, float>>(modelStatsDick);
			eatRatesDick [Species.Bunnies].Add (Species.Grass, Eat_BG);
			eatRatesDick [Species.Bunnies].Add (Species.Carrots, Eat_BC);
			eatRatesDick [Species.Deer].Add (Species.Grass, Eat_DG);
			eatRatesDick [Species.Deer].Add (Species.Carrots, Eat_DC);
			eatRatesDick [Species.Sheep].Add (Species.Grass, Eat_SG);
			eatRatesDick [Species.Wolves].Add (Species.Bunnies, Eat_WB);
			eatRatesDick [Species.Wolves].Add (Species.Deer, Eat_WD);
			eatRatesDick [Species.Wolves].Add (Species.Sheep, Eat_WS);
			foreach (Species eSpecies in eatRatesDick.Keys) 
			{
				Species[] preyArray = eatRatesDick[eSpecies].Keys.ToArray();
				for (int i = 0; i < preyArray.Length; i ++) 
				{
					if (!speciesList.Contains (preyArray[i])) 
					{
						eatRatesDick[eSpecies].Remove (preyArray[i]);
					}
				}
			}
			baseEatRatesDick = eatRatesDick;
			predationDick = new Dictionary<Species, List<Species>>();
			predationDick.Add (Species.Bunnies, new List<Species> {Species.Wolves});
			predationDick.Add (Species.Deer, new List<Species> {Species.Wolves});
			predationDick.Add (Species.Sheep, new List<Species> {Species.Wolves});
			predationDick.Add (Species.Wolves, new List<Species> {});
			predationDick.Add (Species.Grass, new List<Species> {Species.Bunnies, Species.Deer, Species.Sheep});
			predationDick.Add (Species.Carrots, new List<Species> {Species.Bunnies, Species.Deer});
			foreach (Species pSpecies in predationDick.Keys) 
			{
				Species[] predatorArray = predationDick[pSpecies].ToArray();
				for (int i = 0; i < predatorArray.Length; i ++) 
				{
					if (!speciesList.Contains (predatorArray[i])) 
					{
						predationDick[pSpecies].Remove (predatorArray[i]);
					}
				}
			}
			interferenceDick = new Dictionary<Species, List<Species>>();
			interferenceDick.Add (Species.Bunnies, new List<Species> {Species.Deer, Species.Sheep});
			interferenceDick.Add (Species.Deer, new List<Species> {Species.Bunnies, Species.Sheep});
			interferenceDick.Add (Species.Sheep, new List<Species> {Species.Bunnies, Species.Deer});
			interferenceDick.Add (Species.Wolves, new List<Species> {});
			foreach (Species iSpecies in interferenceDick.Keys) 
			{
				Species[] interArray = interferenceDick[iSpecies].ToArray();
				for (int i = 0; i < interArray.Length; i ++) 
				{
					if (!speciesList.Contains (interArray[i])) 
					{
						interferenceDick[iSpecies].Remove (interArray[i]);
					}
				}
			}
			seasonalGrowthRates = new Dictionary<Species, float> ();
			seasonalGrowthRates.Add (Species.Grass, birthRatesArray [4]);
			seasonalGrowthRates.Add (Species.Carrots, birthRatesArray [5]);
			speciesColorDick = new Dictionary<Species, Color> ();
			Color[] colorArray = new Color[] {
								Color.white,
								Color.red,
								Color.blue,
								Color.black,
								Color.green,
								new Color (1f, 0.5f, 0f)
						};
			for (int i = 0; i < allSpeciesArray.Length; i ++) 
			{
				speciesColorDick.Add (allSpeciesArray[i], colorArray[i]);
			}
		}

		public static void SetPreviousPopDick() 
		{
			foreach (Species species in modelStatsDick.Keys)
			{
				prevPopulationsDick[species] = modelStatsDick[species][StatsType.Population];
			}
		}

		public static float Equations(int i) 
		{
			int intOldPop = (int)modelStatsDick [speciesList [i]] [StatsType.Population];
			if (speciesList[i] != Species.Grass && speciesList[i] != Species.Carrots) 
			{
				if ((int)modelStatsDick[speciesList[i]][StatsType.Population] > 0) 
				{
					AnimalEquation(speciesList[i]);
				}
				int difference = (int)modelStatsDick[speciesList[i]][StatsType.Population] - intOldPop;
				if (difference != 0) GameManager.playersDick[speciesList[i]].ChangePopulation(difference);
				return modelStatsDick[speciesList[i]][StatsType.Population] * PopGraph.animalScalingFactor + PopGraph.animalBase;
			}
			VeggieEquation (speciesList [i]);
			return modelStatsDick[speciesList[i]][StatsType.Population] * PopGraph.veggieScalingFactor + PopGraph.veggieBase;
		}

		private static void AnimalEquation(Species species) 
		{
			modelStatsDick [species] [StatsType.Population] += prevPopulationsDick [species] * time_step * (modelStatsDick[species][StatsType.BirthRates] * Consumption(species) - modelStatsDick[species][StatsType.DeathRates] - Protection (species) * (Predation(species) + Interference(species)));
		}

		private static void VeggieEquation(Species species) 
		{
			seasonalGrowthRates[species] = modelStatsDick[species][StatsType.BirthRates] * Mathf.Cos(day / seasonal_year) + 2 * modelStatsDick[species][StatsType.BirthRates] / 3; 
			// the deathRates of veggies are their respective carrying capacities
			modelStatsDick [species] [StatsType.Population] += prevPopulationsDick [species] * time_step * (seasonalGrowthRates[species] * (1 - prevPopulationsDick[species] / modelStatsDick[species][StatsType.DeathRates]) - Protection(species) * Predation(species));
			// The way the equations are currently written, if a veggie reaches it's carrying capacity, it's growth rate will
			// be zero, thus the seasonalGrowthRate won't have any affect if there isn't sufficient predation on the veggie 
			// (i.e. low grazer populations). This if statement "fixes" that.
			if (modelStatsDick[species][StatsType.Population] >= modelStatsDick[species][StatsType.DeathRates] - 0.1f)
			{
				modelStatsDick[species][StatsType.Population] = modelStatsDick[species][StatsType.DeathRates] - 0.1f;
			}
		}

		private static float Consumption(Species predator) 
		{
			float consumption = 0f;
			foreach (Species prey in eatRatesDick[predator].Keys) 
			{
				consumption += eatRatesDick[predator][prey] * modelStatsDick[prey][StatsType.Nutrients] * prevPopulationsDick[prey] * Protection (prey);
			}
			return consumption;
		}

		private static float Predation(Species prey) 
		{
			float predation = 0f;
			foreach (Species predator in predationDick[prey]) 
			{
				predation += eatRatesDick[predator][prey] * prevPopulationsDick[predator];
			}
			return predation;
		}

		private static float Interference(Species niceGuySpecies) 
		{
			float interference = 0f;
			foreach (Species assholeSpecies in interferenceDick[niceGuySpecies]) 
			{
				interference += prevPopulationsDick[assholeSpecies] * modelStatsDick[assholeSpecies][StatsType.Interference];
			}
			return interference;
		}

		private static float Protection (Species prey) 
		{
			return prevPopulationsDick [prey] / (Mathf.Pow (modelStatsDick[prey][StatsType.ProtectionRates], 2) + Mathf.Pow (prevPopulationsDick [prey], 2));
		}

		public static float GetOriginalStat(Species species, StatsType statsType) 
		{
			return baseModelStatsDick[species][statsType];
		}

		public static float GetOriginalEatRate(Species predator, Species prey) 
		{
			return baseEatRatesDick [predator] [prey];
		}

		public static void ChangeModelStats (Species changingSpecies, StatsType statsType, float change) 
		{
			modelStatsDick [changingSpecies] [statsType] += change;
		}

		public static void ChangeEatRates (Species predator, Species prey, float change) 
		{
			eatRatesDick [predator] [prey] += change;
		}
	}
}
//	private static float seasonal_year = 58.09155423f;
//	private static float seasonalDay;
//	public static float day;
//	public static float day_length;
//	private static float veggie_scaling_factor = 6.5f, animal_scaling_factor = 1.25f;
//	public static int veggie_base = -40, animal_base = 35;
//	// time_step can also be thought of as the overall robustness of the system
//	private static float time_step = 0.01f;
//	
//	private void Awake() 
//	{
//		//	[0] = bunnies, [1] = deer, [2] = sheep, [3] = wolves, [4] = grass, [5] = carrots
//		float[] populationsArray = new float[] {50f, 30f, 40f, 10, 499.9f, 99.9f};
//		float[] birthRatesArray = new float[] {0.14f, 0.05f, 0.065f, 0.04f, 9, 14};
//		//	[0] = bunnies, [1] = deer, [2] = sheep, [3] = wolves, [4] = grass carrying capacity, [5] = carrots carrying capacity
//		float[] deathArray = new float[] {0.08f, 0.015f, 0.04f, 0.01f, 500f, 100f};
//		//	[0] = bunnies, [1] = deer, [2] = sheep, [3] = Grass, [4] = Carrots
//		float[] protectionRatesArray = new float[] {25f, 25f, 25f, 40f, 15f};
//		float[] nutrientsArray = new float[] {0.5f, 1.25f, 1.25f, 1f, 1f};
//		//	[0] = bunnies, [1] = deer, [2] = sheep
//		float[] interferenceArray = new float[] {0f, 0f, 0f};
//		float Eat_BG = 0.8f, Eat_BC = 1.3f, Eat_DG = 4.5f, Eat_SG = 6f, Eat_WB = 0.5f, Eat_WD = 0.58f, Eat_WS = 0.9f;
//	}