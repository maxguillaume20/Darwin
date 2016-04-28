using UnityEngine;
using System.Collections;
using RTS;

public class MainMenu : MonoBehaviour {

	public void NewGame ()
	{
		Application.LoadLevel ("SpeciesSelection");
		Time.timeScale = 1.0f;
	}
}
