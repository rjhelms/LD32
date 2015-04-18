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

	public LayerMask WallLayers;
	public Rigidbody2D RigidBody;

	protected Animator animator;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		//RigidBody.position = new Vector2 (Mathf.Floor (RigidBody.position.x), Mathf.Floor (RigidBody.position.y));
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
		Debug.Log (gameObject.name + " in collision, direction: " + MyDirection);
		Direction newDirection = MyDirection;
		bool hitWall = true;
		
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Player") 
			|| coll.gameObject.layer == LayerMask.NameToLayer ("NPC")) {
			newDirection = GetOppositeDirection (MyDirection);
		} else {
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
				
				RaycastHit2D[] hit = Physics2D.RaycastAll (transform.position, rayVector, 16f, WallLayers);
				if (hit.Length == 1) {
					hitWall = false;
				} else {
					Debug.Log (hit [1].collider.name);
					Debug.Log ("Rejecting " + newDirection);
					hitWall = true;
				}
				
			}
		}
		MyDirection = newDirection;
		Debug.Log ("Going " + MyDirection);
	}

	private Direction GetOppositeDirection (Direction oldDirection)
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
}