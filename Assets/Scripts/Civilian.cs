using UnityEngine;
using System.Collections;

public class Civilian : Actor
{
	public Vector2 StartVelocity;
	public bool Hit = false;

	// Use this for initialization
	void Start ()
	{
		BaseStart ();
		animator = this.GetComponent<Animator> ();
		RigidBody = this.GetComponent<Rigidbody2D> ();
		if (StartVelocity.sqrMagnitude == 0) {
			MyDirection = (Direction)Random.Range (0, 4);
		} else {
			RigidBody.velocity = StartVelocity;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (RigidBody.velocity.sqrMagnitude < (MoveSpeed * MoveSpeed)) {
			MyDirection = GetOppositeDirection (MyDirection);
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

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!Hit) {
			ResolveNPCCollision (coll);
		}
	}
	
}