using UnityEngine;
using System.Collections.Generic;

namespace RTS 
{
	public static class PlayerManager  
	{
		private static List<Species> speciesList = new List<Species> ();
		private static int playerCount = 2;
		public static Species playerSpecies;

		public static void AddSpecies(Species selectedSpecies)
		{
			playerSpecies = selectedSpecies;
			speciesList.Add (selectedSpecies);
			while (speciesList.Count < playerCount) 
			{
				List<Species> tempSpeciesList = new List<Species> {Species.Bunnies, Species.Deer, Species.Sheep};
				while (tempSpeciesList.Count > 0 && speciesList.Count < playerCount) 
				{
					int randomIndex = Random.Range (0, tempSpeciesList.Count);
					if (tempSpeciesList[randomIndex] != selectedSpecies || tempSpeciesList.Count == 1) 
					{
						speciesList.Add (tempSpeciesList[randomIndex]);
						tempSpeciesList.RemoveAt (randomIndex);
					}
				}

			}
			Pop_Dynamics_Model.speciesList = new List<Species> (speciesList);
		}
		
		public static Species GetSpecies()
		{
			int listindex = Random.Range (0, speciesList.Count);
			Species newSpecies = speciesList [listindex];
			speciesList.RemoveAt (listindex);
			return newSpecies;
		}
	}
}