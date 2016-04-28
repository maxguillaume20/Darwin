using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using RTS;

public delegate bool BoolMethod();

public class HUD : MonoBehaviour 
{	
	private Canvas thisCanvas;
	public static float xCameraRotation;
	public MainButton[] mainButtons;
	public Text EraText;
	public RepairButton repairButton;
	public InfoText Infotext;
	public Text populationText;
	public Button CloseButton;
	public bool popGraphOpen;
	public PauseMenu pausemenu;
	public MainPanel mainpanel;
	public Dictionary<ResourceType, Text> ResourceTexts = new Dictionary<ResourceType, Text>();
	public Dictionary<PanelButtonType, GameObject> PanelDict = new Dictionary<PanelButtonType, GameObject> ();
	public Sprite[] popSprites;
	public static Dictionary<Species, Sprite> speciesPopSpriteDick { get; set; }
	public Sprite[] resSprites;
	public static Dictionary<Species, Dictionary<ResourceType, Sprite>> speciesResourceSpriteDick { get; set; } 
	public static ResourceDisplayer resDisplayer;

	void Awake()
	{
		GameManager.Hud = this;
		thisCanvas = GetComponent<Canvas> ();
		mainButtons = GetComponentsInChildren<MainButton> ();
		Infotext = GetComponentInChildren<InfoText> ();
		EraText.text = GameManager.HumanPlayer.Era.ToString ();
		xCameraRotation = Camera.main.transform.eulerAngles.x;
		speciesPopSpriteDick = new Dictionary<Species, Sprite> ();
		speciesPopSpriteDick.Add (Species.Bunnies, popSprites [0]);
		speciesPopSpriteDick.Add (Species.Deer, popSprites [1]);
		speciesPopSpriteDick.Add (Species.Sheep, popSprites [2]);
		speciesResourceSpriteDick = new Dictionary<Species, Dictionary<ResourceType, Sprite>>();
		for (int i = 0; i < GameManager.speciesArray.Length - 1; i ++) 
		{
			speciesResourceSpriteDick.Add(GameManager.speciesArray[i], new Dictionary<ResourceType, Sprite>());
			speciesResourceSpriteDick[GameManager.speciesArray[i]].Add(ResourceType.Gold, resSprites[0]);
			speciesResourceSpriteDick[GameManager.speciesArray[i]].Add(ResourceType.Wood, resSprites[1]);
			switch (GameManager.speciesArray[i]) 
			{
			case Species.Bunnies:
				speciesResourceSpriteDick[GameManager.speciesArray[i]].Add(ResourceType.Unique, resSprites[2]);
				break;
			case Species.Deer:
				speciesResourceSpriteDick[GameManager.speciesArray[i]].Add(ResourceType.Unique, resSprites[3]);
				break;
			case Species.Sheep:
				speciesResourceSpriteDick[GameManager.speciesArray[i]].Add(ResourceType.Unique, resSprites[4]);
				break;
			}
		}
		UniqueResSprite uniqueResSprite = GetComponentInChildren<UniqueResSprite> ();
		uniqueResSprite.GetComponent<Image>().sprite = speciesResourceSpriteDick[GameManager.HumanPlayer.species][ResourceType.Unique];
		PopImage popImage = GetComponentInChildren<PopImage> ();
		switch (GameManager.HumanPlayer.species)
		{
		case Species.Bunnies:
			popImage.GetComponent<Image>().sprite = popSprites[0];
			break;
		case Species.Deer:
			popImage.GetComponent<Image>().sprite = popSprites[1];
			break;
		case Species.Sheep:
			popImage.GetComponent<Image>().sprite = popSprites[2];
			break;
		}	
		resDisplayer = GetComponentInChildren<ResourceDisplayer> ();
	}

	public void SetActive (bool isActive)
	{
		thisCanvas.enabled = isActive;
	}
	
	void Start()
	{ 
		ResourceText[] resourceTexts = GetComponentsInChildren<ResourceText> ();
		foreach (ResourceText resText in resourceTexts) 
		{	
			if (resText.gameObject.name == "GoldText")
			{
				ResourceTexts.Add(ResourceType.Gold, resText.GetComponent<Text>());
				ResourceTexts[ResourceType.Gold].text = GameManager.HumanPlayer.startgold.ToString();
			}
			else if (resText.gameObject.name == "WoodText")
			{
				ResourceTexts.Add(ResourceType.Wood, resText.GetComponent<Text>());
				ResourceTexts[ResourceType.Wood].text = GameManager.HumanPlayer.startwood.ToString();
			}
			else if (resText.gameObject.name == "UniqueText")
			{
				ResourceTexts.Add(ResourceType.Unique, resText.GetComponent<Text>());
				ResourceTexts[ResourceType.Unique].text = GameManager.HumanPlayer.startunique.ToString();
			}
		}
		populationText.text = Pop_Dynamics_Model.modelStatsDick[GameManager.HumanPlayer.species][StatsType.Population].ToString();
		Camera.main.transform.position = new Vector3 (GameManager.HumanPlayer.transform.position.x, GameManager.cameraHeight, GameManager.HumanPlayer.transform.position.z - (GameManager.cameraHeight / Mathf.Tan (Camera.main.transform.eulerAngles.x * Mathf.Deg2Rad)));
		Camera.main.orthographicSize = GameManager.cameraInitialSize;
		PanelButton[] panelbuttons = GetComponentsInChildren<PanelButton> ();
		foreach (PanelButton pb in panelbuttons) 
		{
			PanelDict.Add(pb.GetButtonID(), pb.gameObject);
			pb.gameObject.SetActive(false);	
		}
		mainpanel.gameObject.SetActive (false);
	}

	public void OpenMainPanel()
	{
		mainpanel.gameObject.SetActive (true);
		// if InfoText is not on
		if (!mainpanel.MPthings [0].activeSelf) 
		{
			mainpanel.MPthings[0].SetActive(true);	
		}
		foreach (MainButton mb in mainButtons) 
		{
			if (mb.GetComponent<RectTransform>().anchorMax.x < 0.5f) mb.gameObject.SetActive(false);
		}
	}

	public void ClosePanel()
	{
		List<WorldObject> selectObjects = GameManager.HumanPlayer.userInput.SelectedObjects;
		if (selectObjects.Count > 0 && selectObjects[0] as Building) 
		{
			selectObjects[0].Deselect ();
			GameManager.HumanPlayer.userInput.ClearSelectedObjects ();
		}
		else 
		{
			ActuallyClosePanel ();
		}
	}

	public void ActuallyClosePanel () 
	{
		foreach (GameObject poop in PanelDict.Values) 
		{
			if (poop.activeSelf) poop.SetActive (false);
		}
		mainpanel.gameObject.SetActive (false);
		foreach (MainButton mb in mainButtons) 
		{
			if (mb as EvolutionButton == null && mb as CapitalButt == null) mb.gameObject.SetActive(true);
		}
		GameManager.HumanPlayer.userInput.touchTime = 0f;
	}

	public void OpenPanelButton(PanelButtonType butt)
	{
		PanelDict [butt].SetActive (true);
	}

	public void ClosePanelButton(PanelButtonType butt)
	{
		PanelDict [butt].SetActive (false);
	}

	// SetPanelInactive is used by the buildings menu
	public void SetPanelInactive()
	{
		mainpanel.gameObject.SetActive (false);
	}

	public MainPanel GetMainPanel()
	{
		return mainpanel;
	}
	public void OpenPauseMenu()
	{
		pausemenu.gameObject.SetActive (true);
	}

	public static bool EnoughResources (float[] costArray, Text[] resTexts) 
	{ 
		for(int i = 0; i < costArray.Length; i ++) 
		{
			if (costArray[i] > GameManager.HumanPlayer.GetResource(GameManager.resourceTypeArraytoDick[i])) 
			{
				StartChangeTextColorToRed(resTexts);
				return false;
			}
		}
		for (int i = 0; i < costArray.Length; i ++) 
		{
			GameManager.HumanPlayer.ChangeResource (GameManager.resourceTypeArraytoDick[i], -costArray[i]);
		}
		return true;
	}

	public static void StartChangeTextColorToRed (Text[] resTexts) 
	{
		GameManager.Hud.StartCoroutine (ChangeTextColorToRed (resTexts));
	}

	private static IEnumerator ChangeTextColorToRed(Text[] resTexts) 
	{
		foreach (Text resText in resTexts) resText.color = Color.red;
		for (float timer = 0.0f; timer < 1f && resTexts[0].gameObject.activeSelf && resTexts[resTexts.Length - 1].gameObject.activeSelf; timer += Time.deltaTime) yield return null;
		foreach (Text resText in resTexts) resText.color = Color.black;
	}

	public void StartButtonCD (Button button, float coolDown, Text texican, string originalText) 
	{
		StartCoroutine(ButtonCoolDown(button, coolDown, texican, originalText));
	}

	private IEnumerator ButtonCoolDown(Button button, float coolDown, Text texican, string originalText) 
	{
		button.interactable = false;
		for (float time = 0f; time < coolDown; time += Time.deltaTime) 
		{
			int cd = (int)(coolDown - time);
			texican.text =  cd.ToString();
			yield return null;
		}
		texican.text = originalText;
		button.interactable = true;
	}

	public void StartFlashButton(Image buttonImage, BoolMethod boolMethod) 
	{
		StartCoroutine (FlashButton (buttonImage, boolMethod));
	}

	private IEnumerator FlashButton(Image buttonImage, BoolMethod boolMethod) 
	{
		for (float time = 0; boolMethod(); time += Time.deltaTime * 5) 
		{
			float transparency = (Mathf.Cos(time) + 2f) / 3f;
			buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, transparency);
			yield return null;
		}
		buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1f);
	}
}

