using UnityEngine;
using System.Collections;
using RTS;

public class PauseMenu : MonoBehaviour {
	
	public GameObject[] hudcomponents;
	public GameObject popgraph;
	private bool[] hudstate;

	void Awake(){
		hudstate = new bool[hudcomponents.Length];
	}
	void OnEnable(){
		Time.timeScale = 0.0f; 
		popgraph.SetActive (false);
		GameManager.HumanPlayer.GetComponent< UserInput >().enabled = false;
		RTS.GameManager.MenuOpen = true;
		for(int i = 0; i < hudcomponents.Length; i++){
			if (hudcomponents[i].activeSelf){
				hudstate[i] = true;
				hudcomponents[i].SetActive(false);
			}
		}
	}

	public void Continue() {
		Time.timeScale = 1.0f;
//		popgraph.GetComponent<Pop_Graph> ().justunpaused = true;
		popgraph.SetActive (true);
		gameObject.SetActive (false); 
		GameManager.HumanPlayer.GetComponent< UserInput >().enabled = true;
		RTS.GameManager.MenuOpen = false;
		for(int i = 0; i < hudcomponents.Length; i++){
			if (hudstate[i]){
				hudstate[i] = false;
				hudcomponents[i].SetActive(true);
			}
		}
	}

	public void Quit(){
		Application.Quit ();
	}
}
