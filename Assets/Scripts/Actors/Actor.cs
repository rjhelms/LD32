using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{

	public string WalkN = "WalkN";
	public string WalkE = "WalkE";
	public string WalkS = "WalkS";
	public string WalkW = "WalkW";
	public float MoveSpeed = 50f;
	public float AnimationSpeedFactor = 0.03f;
	public Direction MyDirection;
	public bool Moving;
	public LayerMask CollideLayers;
	public Rigidbody2D RigidBody;
	public GameObject CommiePrefab;
	public GameObject CivilianPrefab;
	public GameController MyController;
	public int RandomTurnChance = 2;

	protected Animator animator;

	// Use this for initialization
	void Start ()
	{
	
	}

	protected void BaseStart ()
	{
		animator = this.GetComponent<Animator> ();
		RigidBody = this.GetComponent<Rigidbody2D> ();

		MyController = GameObject.FindObjectOfType<GameController> ();
	}

	protected void UpdateAnimation (float speed)
	{
		// update sprite
		switch (MyDirection) {
		case Direction.NORTH:
			animator.Play (WalkN);
			break;
		case Direction.EAST:
			animator.Play (WalkE);
			break;
		case Direction.SOUTH:
			animator.Play (WalkS);
			break;
		case Direction.WEST:
			animator.Play (WalkW);
			break;
		}

		animator.speed = speed;
	}

	protected void ResolveNPCCollision (Collision2D coll)
	{
		Direction newDirection = MyDirection;
		bool hitWall = true;
		
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player") 
			|| coll.gameObject.layer == LayerMask.NameToLayer ("NPC")) {
			newDirection = GetOppositeDirection (MyDirection);
		} else {
			int depth = 0;

			while (hitWall) {
				newDirection = (Direction)Random.Range (0, 4);
				Vector2 rayVector = new Vector2 ();
				
				switch (newDirection) {
				case Direction.NORTH:
					rayVector = new Vector2 (0, 1);
					break;
				case Direction.EAST:
					rayVector = new Vector2 (1, 0);
					break;
				case Direction.SOUTH:
					rayVector = new Vector2 (0, -1);
					break;
				case Direction.WEST:
					rayVector = new Vector2 (-1, 0);
					break;
				}
				
				RaycastHit2D[] hit = Physics2D.RaycastAll (transform.position, rayVector, 16f, CollideLayers);
				hitWall = false;
				foreach (RaycastHit2D item in hit) {
					if (item.collider.gameObject != this.gameObject) {
						hitWall = true;
						depth++;
					}
				}

				if (depth > 20) {
					Debug.LogError ("Couldn't resolve collision");
					hitWall = false;
				}
			}
		}

		MyDirection = newDirection;
	}

	protected Direction GetOppositeDirection (Direction oldDirection)
	{	
		Direction newDirection = oldDirection;
		switch (oldDirection) {
		case Direction.NORTH:
			newDirection = Direction.SOUTH;
			break;
		case Direction.EAST:
			newDirection = Direction.WEST;
			break;
		case Direction.SOUTH:
			newDirection = Direction.NORTH;
			break;
		case Direction.WEST:
			newDirection = Direction.EAST;
			break;
		default:
			throw(new System.ArgumentOutOfRangeException ());
		}

		return newDirection;
	}
	
	protected void FireProjectile (GameObject projectile)
	{
		Vector2 projectileVelocity = new Vector2 ();
		
		GameObject newProjectileObject = (GameObject)Instantiate (projectile, RigidBody.position, 
		                                                          Quaternion.identity);
		Projectile newProjectile = newProjectileObject.GetComponent<Projectile> ();
		
		if (RigidBody.velocity.magnitude > 0) {
			projectileVelocity = RigidBody.velocity.normalized;
		} else {
			switch (MyDirection) {
			case Direction.NORTH:
				projectileVelocity = Vector2.up;
				break;
			case Direction.EAST:
				projectileVelocity = Vector2.right;
				break;
			case Direction.SOUTH:
				projectileVelocity = -Vector2.up;
				break;
			case Direction.WEST:
				projectileVelocity = -Vector2.right;
				break;
			}
		}

		projectileVelocity *= newProjectile.FireVelocity;
		newProjectile.transform.parent = MyController.ProjectileContainer;
		newProjectile.StartVelocity = projectileVelocity;
		newProjectile.Source = this;
	}

	protected void BaseMovement ()
	{
		if (RigidBody.velocity.sqrMagnitude < (MoveSpeed * MoveSpeed)) {
			MyDirection = GetOppositeDirection (MyDirection);
		}

		int turnChance = Random.Range (0, 100);
		if (turnChance < RandomTurnChance) {
			Debug.Log ("Random turn");
			MyDirection = (Direction)Random.Range (0, 4);
		}

		Vector2 moveVector = new Vector2 ();
		
		switch (MyDirection) {
		case Direction.NORTH:
			moveVector += new Vector2 (0, 1);
			break;
		case Direction.EAST:
			moveVector += new Vector2 (1, 0);
			break;
		case Direction.SOUTH:
			moveVector += new Vector2 (0, -1);
			break;
		case Direction.WEST:
			moveVector += new Vector2 (-1, 0);
			break;
		}
		
		moveVector = moveVector.normalized * MoveSpeed;
		
		UpdateAnimation (moveVector.magnitude * AnimationSpeedFactor);
		
		RigidBody.velocity = moveVector;
	}

	public void BecomeCommie ()
	{
		GameObject newCommieObject = (GameObject)Instantiate (CommiePrefab, transform.position, Quaternion.identity);
		Commie newCommie = newCommieObject.GetComponent<Commie> ();
		newCommie.transform.parent = MyController.CommieContainer;
		newCommie.MyDirection = this.MyDirection;
		newCommie.StartVelocity = this.RigidBody.velocity;
		Destroy (this.gameObject);
	}
	
	public void BecomeCivilian ()
	{
		GameObject newCivilianObject = (GameObject)Instantiate (CivilianPrefab, transform.position, Quaternion.identity);
		Civilian newCivilian = newCivilianObject.GetComponent<Civilian> ();
		newCivilian.transform.parent = MyController.CivilianContainer;
		newCivilian.MyDirection = this.MyDirection;
		newCivilian.StartVelocity = this.RigidBody.velocity;
		Destroy (this.gameObject);
	}
}