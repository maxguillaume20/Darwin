using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class RenSpecButton1 : SpecializationButton
{
	private float[] reformationArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] missionArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] espionageArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] blacksmithArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] medicineArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] astronomyArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};

	protected override void Awake ()
	{
		base.Awake ();
		era = Eras.Renaissance;
		namesArray = new string[] {"Reformation", "Missionaries", "Espionage", "BlackSmith", "Medicine", "Astronomy"};
		methodArray = new UnityEngine.Events.UnityAction[] {delegate {Reformation();}, delegate {Missionaries();}, delegate {Espionage();},
			delegate {BlackSmith();}, delegate {Medicine();}, delegate {Astronomy();}};
		costVariablesList.Add (reformationArray);
		costVariablesList.Add (missionArray);
		costVariablesList.Add (espionageArray);
		costVariablesList.Add (blacksmithArray);
		costVariablesList.Add (medicineArray);
		costVariablesList.Add (astronomyArray);
		messageArray = new string[] {"", "", "","","",""};
	}

	public void Reformation() 
	{

	}

	public void Missionaries() 
	{

	}

	public void Espionage() 
	{

	}

	public void BlackSmith() 
	{

	}

	public void Medicine() 
	{

	}

	public void Astronomy() 
	{

	}
}
