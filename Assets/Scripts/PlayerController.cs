using UnityEngine;
using System.Collections;

public class PlayerController : Actor
{

	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		rigidBody = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Moving = false;
		Vector2 moveVector = new Vector2 ();

		// get input
		if (Input.GetKey (KeyCode.W)) {
			Direction = Direction.NORTH;
			moveVector += new Vector2 (0, 1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.D)) {
			Direction = Direction.EAST;
			moveVector += new Vector2 (1, 0);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.S)) {
			Direction = Direction.SOUTH;
			moveVector += new Vector2 (0, -1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.A)) {
			Direction = Direction.WEST;
			moveVector += new Vector2 (-1, 0);
			Moving = true;
		}

		// move player
		if (!Moving) {
			moveVector *= 0;
		} else {
			moveVector = moveVector.normalized * MoveSpeed;
		}

		UpdateAnimation (moveVector.magnitude * AnimationSpeedFactor);

		rigidBody.velocity = moveVector;
	}
}