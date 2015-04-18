using UnityEngine;
using System.Collections;

public class Civilian : Actor
{

	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		rigidBody = this.GetComponent<Rigidbody2D> ();
		Direction = (Direction)Random.Range (0, 4);
		Debug.Log ((Direction)3);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector2 moveVector = new Vector2 ();

		switch (Direction) {
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
	
		rigidBody.velocity = moveVector;
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		Debug.Log ("Civilian in collision, direction: " + Direction);
		Direction newDirection = Direction;
		bool hitWall = true;

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

		Direction = newDirection;
		Debug.Log ("Going " + Direction);
	}
}