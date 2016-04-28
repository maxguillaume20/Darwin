using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class RenSpecButton2 : SpecializationButton
{
	private float[] enlightArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] inqArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] tanArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] canonArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] engArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] alcArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};

	protected override void Awake ()
	{
		base.Awake ();
		era = Eras.Renaissance;
		namesArray = new string[] {"Enlightenment", "Inquistion", "Tannery", "Canon Foundry", "Engineering", "Alchemy"};
		methodArray = new UnityEngine.Events.UnityAction[] {delegate {Enlightenment();}, delegate {Inquisition();}, delegate {Tannery();},
			delegate {CanonFoundry();}, delegate {Engineering();}, delegate {Alchemy();}};
		costVariablesList.Add (enlightArray);
		costVariablesList.Add (inqArray);
		costVariablesList.Add (tanArray);
		costVariablesList.Add (canonArray);
		costVariablesList.Add (engArray);
		costVariablesList.Add (alcArray);
		messageArray = new string[] {"", "", "","","",""};
	}
	
	public void Enlightenment() 
	{

	}
	
	public void Inquisition() 
	{

	}
	
	public void Tannery() 
	{

	}
	
	public void CanonFoundry() 
	{

	}
	
	public void Engineering() 
	{

	}
	
	public void Alchemy() 
	{

	}
}
