using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class InfoSpecButton1 : InfoSpecButton
{
	private float[] cathArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] medArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] telArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] cultsArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] cyberArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] hillArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] mechaArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] iedsArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] personArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] aiArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] smartArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] plasticArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};

	protected override void Awake ()
	{
		base.Awake ();
		namesArray = new string[] {"Catholicism", "Meditation Crystals", "Televangelism", "Cults", "Cyber Terrorism", "Hillbillies", "Mecha Bambi", "IEDs", "Personalized Medicine", "Robotics", "Space Program", "Plastic Utensils"};
		methodArray = new UnityEngine.Events.UnityAction[] {delegate {Catholicism();}, delegate {MeditationCrystals();}, delegate {Televangelism();},
			delegate {Cults();}, delegate {CyberTerrorism();}, delegate {Hillbillies();}, delegate {MechaBambi();}, delegate {IEDs();}, 
			delegate {PersonalizedMedicine();}, delegate {Robotics();}, delegate {SpaceProgram();}, delegate {PlasticUtensils();}};
		costVariablesList.Add (cathArray);
		costVariablesList.Add (medArray);
		costVariablesList.Add (telArray);
		costVariablesList.Add (cultsArray);
		costVariablesList.Add (cyberArray);
		costVariablesList.Add (hillArray);
		costVariablesList.Add (mechaArray);
		costVariablesList.Add (iedsArray);
		costVariablesList.Add (personArray);
		costVariablesList.Add (aiArray);
		costVariablesList.Add (smartArray);
		costVariablesList.Add (plasticArray);
		messageArray = new string[] {"", "", "","","","","", "", "","","",""};
	}

	public void Catholicism() 
	{

	}
	
	public void MeditationCrystals() 
	{

	}
	
	public void Televangelism() 
	{

	}
	
	public void Cults() 
	{

	}
	
	public void CyberTerrorism() 
	{

	}
	
	public void Hillbillies() 
	{

	}

	public void MechaBambi() 
	{

	}

	public void IEDs() 
	{

	}

	public void PersonalizedMedicine() 
	{

	}

	public void Robotics() 
	{

	}

	public void SpaceProgram() 
	{

	}

	public void PlasticUtensils() 
	{

	}
}
