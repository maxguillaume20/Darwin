using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using RTS;

public class StatsInfoText : MonoBehaviour 
{
	public Text text;
	public int indexArray;
	public float amount;
	public StatsType statsType;
	private string units;

	public void Iniaite () 
	{
		text = GetComponent<Text> ();
		if (name.Contains ("Increase")) 
		{
			indexArray = 1;
		}
		else 
		{
			indexArray = 0;
		}
		if (name.Contains("Health")) 
		{
			statsType = StatsType.Health;
			units = " hp";
		}
		else if (name.Contains("Attack")) 
		{
			statsType = StatsType.Attack;
			units = " dps";
		}
		else if (name.Contains("Ranged")) 
		{
			statsType = StatsType.RangedStats;
			units = " m";
		}
		else if (name.Contains("Movement")) 
		{
			statsType = StatsType.MobileStats;
			units = " m/s";
		}
		else if (name.Contains("Defense")) 
		{
			statsType = StatsType.Defense;
			units = " %";
			string defenseIndex = name.Substring(name.Length - 1);
			switch (defenseIndex) 
			{
			case "2":
				indexArray = 2 + indexArray;
				break;
			case "3":
				indexArray = 4 + indexArray;
				break;
			case "4":
				indexArray = 6 + indexArray;
				break;
			case "5":
				indexArray = 8 + indexArray;
				break;
			}
		}
	}

	public void SetStat (float newStat) 
	{
		amount = newStat;
		if (statsType != StatsType.Defense) 
		{
			text.text = amount.ToString("0.#") + units;
		}
		else 
		{
			text.text = (newStat * 100f).ToString("0.#") + units;
		}
	}

	public void SetIncrease (float newIncrease) 
	{
		if (newIncrease > 0f) 
		{
			text.text = "(+ " + newIncrease.ToString("0.#") +  " " + units + ")";
		}
		else 
		{
			text.text = "";
		}
	}
}
