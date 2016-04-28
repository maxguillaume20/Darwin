using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class LabButton : PanelButton
{
	private Text mainText;
	public Text costText;
	public Image costSprite;
	private Lab lab;
	private int labCost;
	public ExperimentPanel experimentPanel;

	protected override void Awake ()
	{
		base.Awake ();
		buttonID = RTS.PanelButtonType.LabButton; 
		mainText = GetComponentInChildren<Text> ();
		mainText.text = "Experiment";
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		if (wo) 
		{
			lab = wo as Lab;
			costText.gameObject.SetActive (true);
			costSprite.gameObject.SetActive (true);
			labCost = (int)lab.uniqueStatsArray[2];
			costText.text = labCost.ToString ();
		}
	}

	private void Update () 
	{
		if (!lab.ableToExperiment)  
		{
			if (!TrainingProgressBar.isActive) 
			{
				TrainingProgressBar.OpenBar (buttonID);
			}
			TrainingProgressBar.ChangeProgress (lab.GetCoolDownProgress ());
		}
		else if (TrainingProgressBar.isActive) 
		{
			TrainingProgressBar.CloseBar ();
		}
		if (!lab.ableToExperiment && button.interactable) 
		{
			button.interactable = false;
			button.image.color = button.colors.disabledColor;
		}
		else if (lab.ableToExperiment && !button.interactable)
		{
			button.interactable = true;
			button.image.color = button.colors.normalColor;
		}
		if (labCost != (int)lab.uniqueStatsArray[2]) 
		{
			labCost = (int)lab.uniqueStatsArray[2];
			costText.text = labCost.ToString ();
		}
	}

	protected override void OnDisable () 
	{
		costSprite.gameObject.SetActive (false);
		costText.gameObject.SetActive (false);
		TrainingProgressBar.CloseBar ();
		selected = false;
		button.image.color = button.colors.normalColor;
		button.interactable = true;
	}

	public void StartExperiment() 
	{
		if (!selected) 
		{
			selected = true;
			button.image.color = button.colors.highlightedColor;
			StartCoroutine(SelectedCountDown());
		}
		else if (EnoughResources("shitassshitfuck", new Text[] {costText}))
		{
			GameManager.HumanPlayer.ChangeResource(ResourceType.Gold, -lab.uniqueStatsArray[2]);
			experimentPanel.StartExperiment(lab);
		}
	}
	
	protected override bool EnoughResources(string name, Text[] texts) 
	{
		if (GameManager.HumanPlayer.GetResource(ResourceType.Gold) >= lab.uniqueStatsArray[2]) return true;
		StartCoroutine(ChangeTextColorToRed(texts));
		return false;
	}
	
	public void ExperimentOver () 
	{
		lab.StartExperimentCoolDown ();
	}
}
