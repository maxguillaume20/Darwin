using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RTS;

public delegate void goodMethodName ();

public class TempBuilding : MonoBehaviour 
{
	private Renderer thisRenderer;
	private static Color activeSpriteColor { get; set; }
	private static Color inactiveSpriteColor { get; set; }
	private Color stratLineColor;
	private Player player;
	private float touchTimer;
	private float cameraMoveBoundary = 0.15f;
	private float cameraMoveSpeed = 0.5f;
	public bool isPlaceable;
	private SpriteRenderer spriteRenderer;
	private BuildingSlot currBuildingSlot;
	private float buildingRadius;
	private float searchTimer;

	void Awake()
	{
		activeSpriteColor = new Color (1f, 1f, 1f);
		inactiveSpriteColor = new Color (0f, 0f, 0f, 0.6f);
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteRenderer.material.color = Color.black;
		player = transform.root.GetComponent<Player> ();
		thisRenderer = GetComponent<Renderer> ();
		transform.localEulerAngles = new Vector3 (HUD.xCameraRotation, 0f, 0f);
	}

	public void EnableTempBuilding(string buildingname)
	{
		gameObject.SetActive (true);
		Building newBuilding = GameManager.GetGameObject(buildingname).GetComponent<Building>();
		spriteRenderer.sprite = GameManager.mainSpriteDick[newBuilding.name];
		buildingRadius = newBuilding.GetComponent<SphereCollider> ().radius;
		transform.localScale = newBuilding.GetComponentsInChildren<SpriteParent> (true) [0].transform.localScale;
		name = buildingname;
		currBuildingSlot = null;
		GameManager.HumanPlayer.userInput.enabled = false;
	}

	void Update() 
	{
		if (!GameManager.HumanPlayer.userInput.usingMouseInput) 
		{
			if (Input.touchCount > 0 && touchTimer > 0.1f && GameManager.FingerInBounds(Input.GetTouch(0).position)) 
			{
				// might have to change the y position 
				transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (Input.GetTouch(0).position.x, Input.GetTouch (0).position.y + 100, Camera.main.transform.position.y));
				transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);
				touchTimer = 0f;
			}
			MoveCamera();
			touchTimer += Time.deltaTime;
		}
		else 
		{
			GameManager.HumanPlayer.userInput.mouseInput.MoveCamera ();
			if (Input.GetMouseButton(0) && GameManager.FingerInBounds (Input.mousePosition)) 
			{
				Vector3 inputPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				float camRotAdjustment = Mathf.Tan (Mathf.Deg2Rad * (90f - Camera.main.transform.rotation.eulerAngles.x)) * inputPosition.y;
				transform.position = new Vector3 (inputPosition.x, 0f, inputPosition.z + camRotAdjustment);
			}


		}
		SetPlaceability ();
		searchTimer += Time.deltaTime;
	}

	void OnDisable() 
	{
		searchTimer = 0f;
		GameManager.HumanPlayer.userInput.enabled = true;
	}

	private void MoveCamera()
	{
		if (Input.touchCount > 0 && GameManager.FingerInBounds(Input.GetTouch(0).position)) 
		{
			Vector2 touchPos = Input.GetTouch(0).position;
			if (touchPos.x < Screen.width * cameraMoveBoundary || touchPos.x >= Screen.width * (1 - cameraMoveBoundary) || touchPos.y < Screen.height * cameraMoveBoundary || touchPos.y >= Screen.height * (1 - cameraMoveBoundary)) 
			{
				Vector2 direction = (touchPos - new Vector2 (Screen.width / 2f, Screen.height / 2f)).normalized;
				Camera.main.transform.Translate(direction.x * cameraMoveSpeed, direction.y * cameraMoveSpeed, 0f); 
			}
		}
	}

	public void PlaceBuilding()
	{
		player.AddBuilding(name, transform.position);
		Building newBuilding = player.buildings.currentBuildings [player.buildings.currentBuildings.Count - 1];
		newBuilding.buildingSlot = SetBuildingSlot ();
		currBuildingSlot.currBuilding = newBuilding;
		SpeciesUnitTrainer newSpeciesUnitTrainer = newBuilding.GetComponent<SpeciesUnitTrainer>();
		if (newSpeciesUnitTrainer) 
		{
			newSpeciesUnitTrainer.SetMainStratPoint (currBuildingSlot.stratPoint);
		}
		gameObject.SetActive (false);
	}

	private void SetPlaceability () 
	{
		isPlaceable = false;
		Vector3 origin = new Vector3 (transform.position.x, 100f, transform.position.z);
		RaycastHit hit;
		if (Physics.Raycast (origin, Vector3.down, out hit, 200f, LayerMask.GetMask (new string[] {"BuildingArea"}))) 
		{
			BuildingSlot bS = hit.collider.GetComponent<BuildingSlot> ();
			if (bS.species == player.species && !bS.isOccupied) 
			{
				transform.position = bS.transform.position;
				currBuildingSlot = bS;
				Collider[] colls = Physics.OverlapSphere (transform.position, buildingRadius, GameManager.woLayerMask);
				if (colls.Length == 0) 
				{
					isPlaceable = true;
				}
				else if (searchTimer > 1f)
				{
					foreach (Collider coll in colls) 
					{
						MobileWorldObject wO = coll.GetComponent<MobileWorldObject>();
						if (wO && wO.GetSpecies () == player.species && !wO.isMoving && !wO.attacking) 
						{
							Vector3 buildingPos = new Vector3 (transform.position.x, 0f, transform.position.z);
							Vector3 woPos = new Vector3 (wO.transform.position.x, 0f, wO.transform.position.z);
							if (woPos == buildingPos) 
							{
								woPos += new Vector3 (Random.Range (-1f, 1f), 0f, Random.Range (-1f, 1f));
							}
							Vector3 moveVector = woPos - buildingPos;
							float distance = buildingRadius + (wO.mainCollider as SphereCollider).radius - moveVector.magnitude + 1f;
							Vector3 destination = wO.transform.position + moveVector.normalized * distance;
							wO.SetNavAgentDestination (destination, distance + moveVector.magnitude);
						}
					}
					searchTimer = 0f;
				}
			}
		}
		else 
		{
			currBuildingSlot = null;
		}
		if (isPlaceable) 
		{
			thisRenderer.material.color = activeSpriteColor;
		}
		else thisRenderer.material.color = inactiveSpriteColor;
	}

	private BuildingSlot SetBuildingSlot () 
	{
		currBuildingSlot.SetOccupation (true);
		return currBuildingSlot;
	}
}
