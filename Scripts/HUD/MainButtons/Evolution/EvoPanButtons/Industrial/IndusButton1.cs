using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class IndusButton1 : EvolutionPanelButton
{
	private float[] chasArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] grovedArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};
	private float[] cotArray = new float[] {1f, 0f, 0f, 0f, 0f, 0f};

	protected override void Awake ()
	{
		base.Awake ();
		namesArray = new string[] {"Chastity", "Groved Barrels", "Cotton Gin"};
		methodArray = new UnityEngine.Events.UnityAction[] { delegate {Chastity();}, delegate {GrovedBarrels();}, delegate {CottonGin();}};
		costVariablesList.Add (chasArray);
		costVariablesList.Add (grovedArray);
		costVariablesList.Add (cotArray);
		messageArray = new string[] {"", "", ""};
	}
	
	public void Chastity() 
	{
	
	}
	
	public void GrovedBarrels () 
	{

	}
	
	public void CottonGin() 
	{

	}
}
