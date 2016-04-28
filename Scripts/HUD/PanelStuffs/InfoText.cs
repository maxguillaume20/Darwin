using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class InfoText : MonoBehaviour 
{
	public Text nameText;
	public Text healthText;
	public Text popText;
	public Image popSprite;
	private bool setPopInfo;
	private string Name;
	private string additionalshit;
	private WorldObject wo;
	private StrategicPoint stratPoint;
	private Caravan selectedCaravan;

	private void OnEnable ()
	{
		nameText.text = "";
		healthText.text = "";
		nameText.gameObject.SetActive (true);
		healthText.gameObject.SetActive (true);
		if (GameManager.HumanPlayer.userInput.GetSelectedObjects().Count != 0) 
		{
			wo = GameManager.HumanPlayer.userInput.GetSelectedObjects()[0];	
			nameText.text = wo.GetNameText();
			healthText.text = wo.GetHealthText();
			stratPoint = wo as StrategicPoint;
			selectedCaravan = wo as Caravan;
			if (stratPoint && stratPoint.occupied || selectedCaravan) 
			{
				SetPopInfoActive (true);
				popSprite.sprite = HUD.speciesPopSpriteDick[wo.GetSpecies()];
			}
		}
	}

	private void Update() 
	{
		if (wo && healthText.text != wo.GetHealthText())
		{
			healthText.text = wo.GetHealthText();
		}
		if (setPopInfo) 
		{
			if (selectedCaravan &&  popText.text != ((selectedCaravan.currPopCount)).ToString()) 
			{
				popText.text = ((selectedCaravan.currPopCount)).ToString();
			}
			else if (stratPoint && stratPoint.occupied && popText.text != ((int)(stratPoint.population)).ToString()) 
			{
				popText.text = ((int)(stratPoint.population)).ToString();
			}
		}
		if (stratPoint == null && selectedCaravan == null && (setPopInfo || popText.gameObject.activeSelf || popSprite.gameObject.activeSelf)) 
		{
			setPopInfo = false;
			popText.gameObject.SetActive (false);
			popSprite.gameObject.SetActive (false);
		}
	}

	public void SetPopInfoActive (bool enabled) 
	{
		setPopInfo = enabled;
		popText.gameObject.SetActive (enabled);
		popSprite.gameObject.SetActive (enabled);
	}

	private void OnDisable() 
	{
		stratPoint = null;
		wo = null;
		nameText.gameObject.SetActive (false);
		healthText.gameObject.SetActive (false);
		popSprite.gameObject.SetActive(false);
		popText.gameObject.SetActive(false);
	}
}
