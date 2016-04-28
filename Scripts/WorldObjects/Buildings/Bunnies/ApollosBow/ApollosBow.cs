using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RTS;

public class ApollosBow : HeroUnit
{
//	private NavMeshObstacle navMeshObs;
//	private float favorCostTimerLength = 2;
//	private float favorCostTimer;
//	public int favorCost;
//	public float fireReloadTime;
//	public float spinTime;
//	public float spinReloadTime;
//	private float spinAttackRadius = 1;
//	private float spinSpeed = 20f;
//	private bool spinning;
//	public float range;
//	public float velocity;
//	private float jumpBackVelocity = 25f;
//	public float[] fireAttackArray;
//	public float[] spinAttackArray;
//	private float lastDegree;
//	private bool flip;
//	private FakeArrow fakeArrow;
//	private Bow bow;
//	public float deathCD;
//	public float jumpBackCD;
//	public float spinCD;
//
//	protected override void Awake ()
//	{
//		base.Awake ();
//		navMeshObs = GetComponent<NavMeshObstacle> ();
//		fakeArrow = GetComponentInChildren<FakeArrow> ();
//		bow = GetComponentInChildren<Bow> ();
//		methodArray[0] = delegate {Fire();};
//		methodArray[1] = delegate {StartCoroutine(Spin ());};
//		methodArray[2] = delegate {StartCoroutine(JumpBack());};
//	}
//
//	private void Update ()
//	{
//		favorCostTimer += Time.deltaTime;
//		if (favorCostTimer >= favorCostTimerLength) 
//		{
//			GameManager.HumanPlayer.ChangeResource(ResourceType.Unique, -favorCost);
//			if (GameManager.HumanPlayer.GetResource(ResourceType.Unique) <= 0) 
//			{
//				GameManager.HumanPlayer.ChangeResource(ResourceType.Unique, (int) (0 - GameManager.HumanPlayer.GetResource(ResourceType.Unique)));
//				Die();
//			}
//			favorCostTimer = 0f;
//		}
//		if (selected && Input.touchCount == 1 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) && !spinning && GameManager.FingerInBounds(Input.GetTouch(0)) && !GameManager.HumanPlayer.userInput.cameramoved && GameManager.HumanPlayer.userInput.touchTime > 0.15f)
//		{
//			Rotate();
//		}
//	}
//
//	private void Rotate() 
//	{
//		Vector3 relativePos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.GetTouch (0).position.x, Input.GetTouch (0).position.y, 30f)) - transform.position;
//		bow.transform.rotation = Quaternion.LookRotation (relativePos);
//	}
//
//	private void Fire() 
//	{
//		if (fakeArrow.gameObject.activeSelf)
//		{
//			GameObject gameobject = (GameObject) Instantiate (GameManager.GetGameObject("ApollosArrow"), transform.position, bow.transform.rotation);
//			Projectile projectile = gameobject.GetComponent<Projectile>();
//			projectile.shooter = this as WorldObject;
//			projectile.range = range;
//			projectile.velocity = velocity;
//			GameManager.Hud.StartButtonCD(GameManager.uahMiniPanel.uahButtons[0].button, fireReloadTime, GameManager.uahMiniPanel.uahButtons[0].texican, "");
//			StartCoroutine (ReloadArrow ());
//		}
//	}
//
//	private IEnumerator Spin() 
//	{
//		spinning = true;
//		GameManager.Hud.StartButtonCD(GameManager.uahMiniPanel.uahButtons [1].button, spinCD, GameManager.uahMiniPanel.uahButtons[1].texican, "");
//		float time = 0f;
//		while (time < spinTime) 
//		{
//			for (float attackTimer = 0f; attackTimer < spinReloadTime; attackTimer += Time.deltaTime) 
//			{
//				time += Time.deltaTime;
//				bow.transform.Rotate(Vector3.up * spinSpeed);
//				yield return null;
//			}
//			Collider[] targets = Physics.OverlapSphere(transform.position, mainCollider.radius * transform.localScale.magnitude + spinAttackRadius);
//			foreach (Collider target in targets) 
//			{
//				WorldObject targetWO = target.GetComponent<WorldObject>();
//				if (targetWO && !targetWO.IsOwnedBy(player.species)) 
//				{
////					targetWO.Damage(spinAttackArray, this as WorldObject);
//				}
//			}
//		}
//		spinning = false;
//	}
//
//	private IEnumerator JumpBack() 
//	{
//		GameManager.Hud.StartButtonCD(GameManager.uahMiniPanel.uahButtons [2].button, jumpBackCD, GameManager.uahMiniPanel.uahButtons[2].texican, "");
//		RaycastHit hit = new RaycastHit();
//		float distance = range * 0.75f;
//		if (!spinning && Physics.Raycast (transform.position, -bow.transform.forward, out hit, distance, LayerMask.GetMask (new string[] {"NavMesh"}))) 
//		{
//			distance = (hit.point - transform.position).magnitude - mainCollider.radius * transform.localScale.magnitude;
//		}	
//		navMeshObs.enabled = false;
//		while (distance > 0) 
//		{
//			float positionChange = Time.deltaTime * jumpBackVelocity;
//			distance -= positionChange;
//			transform.position += (positionChange * -bow.transform.forward);
//			yield return null;
//		}
//		navMeshObs.enabled = true;
//	}
//
//	private IEnumerator ReloadArrow() 
//	{
//		fakeArrow.gameObject.SetActive (false);
//		for (float time = 0f; time < fireReloadTime; time += Time.deltaTime) yield return null;
//		fakeArrow.gameObject.SetActive (true);
//	}
//
//	public override void Die ()
//	{
//		base.Die ();
//		GameManager.Hud.StartButtonCD(GameManager.uahPanel.uahPanButts [1].button, deathCD, GameManager.uahPanel.uahPanButts [1].texticals[0], "Apollo's Bow");
//	}
//
//	public override void SelectTap (Player controller)
//	{
//		base.SelectTap (controller);
//		GameManager.HumanPlayer.userInput.touchTime = 0f;
//	}
}
