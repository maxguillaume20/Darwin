using UnityEngine;
using RTS;

// this script is attached to the main camera in the speciesselection scene

public class SpeciesSelection : MonoBehaviour 
{
	private string selectedMap = "1v1Map";
	private Species selectedSpecies = Species.Sheep;

	public void SelectBunny()
	{
		selectedSpecies = Species.Bunnies;
		LoadLevel ();
	}
	public void SelectDeer()
	{
		selectedSpecies = Species.Deer;
		LoadLevel ();
	}
	public void SelectSheep()
	{
		selectedSpecies = Species.Sheep;
		LoadLevel ();
	}

	private void LoadLevel () 
	{
		PlayerManager.AddSpecies (selectedSpecies);
		GameManager.Initiate ();
		Application.LoadLevel (selectedMap);
	}
}
