using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class ResourceDisplayer : MonoBehaviour 
{
	private ResourceDisplay originalResDisplay;
	private Queue<ResourceDisplay> inactiveResDisplaysQueue = new Queue<ResourceDisplay> ();
	private List<ResourceDisplay> activeResDisplaysList = new List<ResourceDisplay> ();
	private float maxActiveResDisplayTime = 2f;
	private float startPointOffsetY = 4f;
	private float endPointYPos = 3f;
	

	private void Awake () 
	{

		originalResDisplay = GetComponentInChildren<ResourceDisplay> ();
		originalResDisplay.gameObject.SetActive (false);
		inactiveResDisplaysQueue.Enqueue (originalResDisplay);
	}

	public void ActivateResourceDisplay (Vector3 worldPoint, Species species, ResourceType resType, int amount) 
	{
		// get newActiveResDisplay
		ResourceDisplay newActiveResDisplay;
		if (inactiveResDisplaysQueue.Count > 0)
		{
			newActiveResDisplay = inactiveResDisplaysQueue.Dequeue();
		}
		else 
		{
			newActiveResDisplay = (ResourceDisplay) Instantiate(originalResDisplay);
			newActiveResDisplay.transform.parent = this.transform;
		}
		// activate newActiveResDisplay
		worldPoint.Set (worldPoint.x, worldPoint.y, worldPoint.z + startPointOffsetY);
		newActiveResDisplay.Activate (worldPoint, species, resType, amount);
		activeResDisplaysList.Add (newActiveResDisplay);
		// start ActiveResDisplayCoroutine if it's not already going
		if (activeResDisplaysList.Count == 1) 
		{
			StartCoroutine (ActiveResDisplayCoroutine());
		}
	}

	private IEnumerator ActiveResDisplayCoroutine () 
	{
		while (activeResDisplaysList.Count > 0) 
		{
			// handle active resDisplays
			List<ResourceDisplay> deactivateList = new List<ResourceDisplay>();
			foreach (ResourceDisplay resDisplay in activeResDisplaysList) 
			{
				// add resDisplay to deactivateList if it's been active longer than maxActiveResDisplayTime
				resDisplay.activeTimer += Time.deltaTime;
				if (resDisplay.activeTimer > maxActiveResDisplayTime) 
				{
					deactivateList.Add (resDisplay);
				}
				// else move and resize resDisplay
				else 
				{
					Vector3 startPoint = Camera.main.WorldToScreenPoint (resDisplay.worldPoint);
					startPoint.Set (startPoint.x, startPoint.y, 0f);
					Vector3 endWorldPoint = new Vector3 (resDisplay.worldPoint.x, resDisplay.worldPoint.y, resDisplay.worldPoint.z + endPointYPos);
					Vector3 endPoint = Camera.main.WorldToScreenPoint (endWorldPoint);
					endPoint.Set (endPoint.x, endPoint.y, 0f);
					resDisplay.transform.position = Vector3.Lerp (startPoint, endPoint, resDisplay.activeTimer / maxActiveResDisplayTime);
				}
			}
			// deactivate resDisplays and add them to inactiveResDisplayQueue
			foreach (ResourceDisplay resDisplay in deactivateList) 
			{
				resDisplay.activeTimer = 0f;
				activeResDisplaysList.Remove (resDisplay);
				resDisplay.gameObject.SetActive (false);
				inactiveResDisplaysQueue.Enqueue (resDisplay);
			}
			yield return null;
		}
	}
}
