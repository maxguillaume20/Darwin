using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class InfoSpecButton2 : InfoSpecButton
{
	private float[] athiesmArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] oneArray = new float[]{1f, 0f, 0f, 0f, 0f, 0f};
	private float[] christArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] funArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] ninjasArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] reinArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] armArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] wmdsArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] geneArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] robArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] spaceArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] chemArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};

	protected override void Awake ()
	{
		base.Awake ();
		namesArray = new string[] {"Athiesm", "One-ness", "Christian Dating Sites", "Fundamentalism", "Ninjas", "Reindeer Drones", "Armored Vehicles", "WMDs", "Genetic Engineering", "Artificial Intelligence", "Smartphones", "Chemical Weapons"};
		methodArray = new UnityEngine.Events.UnityAction[] {delegate {Athiesm();}, delegate {Oneness();}, delegate {ChristianDatingSites();},
			delegate {Fundamentalism();}, delegate {Ninjas();}, delegate {ReindeerDrones();}, delegate {ArmoredVehicles();}, delegate {WMDs();}, 
			delegate {GeneticEngineering();}, delegate {AI();}, delegate {Smartphones();}, delegate {ChemicalWeapons();}};
		costVariablesList.Add (athiesmArray);
		costVariablesList.Add (oneArray);
		costVariablesList.Add (christArray);
		costVariablesList.Add (funArray);
		costVariablesList.Add (ninjasArray);
		costVariablesList.Add (reinArray);
		costVariablesList.Add (armArray);
		costVariablesList.Add (wmdsArray);
		costVariablesList.Add (geneArray);
		costVariablesList.Add (robArray);
		costVariablesList.Add (spaceArray);
		costVariablesList.Add (chemArray);
		messageArray = new string[] {"", "", "","","","","", "", "","","",""};
	}

	public void Athiesm() 
	{

	}
	
	public void Oneness() 
	{

	}
	
	public void ChristianDatingSites() 
	{

	}
	
	public void Fundamentalism() 
	{

	}
	
	public void Ninjas() 
	{

	}
	
	public void ReindeerDrones() 
	{

	}

	public void ArmoredVehicles() 
	{

	}

	public void WMDs() 
	{

	}

	public void GeneticEngineering() 
	{

	}

	public void AI() 
	{

	}

	public void Smartphones() 
	{

	}

	public void ChemicalWeapons() 
	{

	}
}
