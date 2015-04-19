using UnityEngine;
using System.Collections;
using Pathfinding;

public class Bureaucrat : Actor
{

	public bool Enraged;
	public float EnragedFlashRate = 1f;
	public float EnragedDuration = 5f;
	public Path MyPath;
	public float NextWayPointDistance = 2f;
	public GameObject MyProjectile;
	public int FireChance = 1;
	public float FireRate;

	private float nextFlashTime;
	private float calmTime;
	private SpriteRenderer mySprite;
	private Seeker seeker;
	private int currentWayPoint;
	private bool waitingForPath;
	private float nextFire;

	// Use this for initialization
	void Start ()
	{
		BaseStart ();

		if (StartVelocity.sqrMagnitude == 0) {
			MyDirection = (Direction)Random.Range (0, 4);
		} else {
			RigidBody.velocity = StartVelocity;
		}

		mySprite = this.GetComponent<SpriteRenderer> ();
		seeker = this.GetComponent<Seeker> ();
		MyPrefab = MyController.BureaucratPrefab;

		Enraged = false;		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (MyController.Running) {

			if (Enraged) {
				EnragedUpdate ();
			} else {
				BaseMovement ();
			}

			if (Time.time > nextFire) {
				int willFire = Random.Range (0, 100);
				if (willFire < FireChance) {
					FireProjectile (MyProjectile);
					nextFire = Time.time + FireRate;
				}
			}

		} else {
			RigidBody.velocity = Vector2.zero;
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (Enraged) {
			ResolveEnragedCollision (coll);
		} else {
			ResolveNPCCollision (coll);
		}

	}

	void ResolveEnragedCollision (Collision2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			MyController.BureaucratHit ();
			BecomeCalm ();
		}
	}

	void EnragedUpdate ()
	{
		if (Time.time > calmTime) {
			BecomeCalm ();
			return;
		} else if (Time.time > nextFlashTime) {
			mySprite.enabled = !mySprite.enabled;
			nextFlashTime = Time.time + EnragedFlashRate;
		}

		bool donePath = false;

		if (MyPath != null 
			&& (Vector2.Distance (transform.position, MyPath.vectorPath [currentWayPoint]) < NextWayPointDistance)) {
			currentWayPoint++;

			if (currentWayPoint >= MyPath.vectorPath.Count) {
				donePath = true;
			} else {
				float angle = Mathf.Atan2 (MyPath.vectorPath [currentWayPoint].y - transform.position.y, 
				                           MyPath.vectorPath [currentWayPoint].x - transform.position.x) 
					* 180 / Mathf.PI;

				if (angle < 0) {
					angle += 360;
				}

				Debug.Log ("angle: " + angle);

				if (angle >= 315 || angle < 45) {
					MyDirection = Direction.EAST;
				} else if (angle >= 45 && angle < 135) {
					MyDirection = Direction.NORTH;
				} else if (angle >= 135 && angle < 225) {
					MyDirection = Direction.WEST;
				} else if (angle >= 225 && angle < 315) {
					MyDirection = Direction.SOUTH;
				} else {
					Debug.LogWarning ("Bureaucrat broke the universe.");
				}
			}


		}

		if (MyPath != null && !donePath) {
			Vector2 pathVector = (MyPath.vectorPath [currentWayPoint] - transform.position).normalized;
			pathVector *= MoveSpeed; 
			UpdateAnimation (pathVector.magnitude * AnimationSpeedFactor);
			RigidBody.velocity = pathVector;
		} else if (!waitingForPath) {
			Debug.Log ("Reached end of path, getting another");
			MyPath = null;
			waitingForPath = true;
			seeker.StartPath (transform.position, MyController.PlayerTransform.position, OnPathComplete);
		}
	}

	public void BecomeEnraged ()
	{
		if (!Enraged) {
			nextFlashTime = Time.time + EnragedFlashRate;
			FireChance *= 5;
		}

		Enraged = true;
		calmTime = Time.time + EnragedDuration;
		Debug.Log ("grrr...");

		if (!waitingForPath) {
			MyPath = null;
			waitingForPath = true;
			seeker.StartPath (transform.position, MyController.PlayerTransform.position, OnPathComplete);
		}
	}

	public void BecomeCalm ()
	{
		Enraged = false;
		mySprite.enabled = true;
		MyPath = null;
		FireChance /= 5;
	}
		
	public void OnPathComplete (Path p)
	{
		if (!p.error) {
			MyPath = p;
			currentWayPoint = 0;
			waitingForPath = false;
		}
	}
}