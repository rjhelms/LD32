using UnityEngine;
using System.Collections;

public class Commie : Actor
{
	public Vector2 StartVelocity;

	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		RigidBody = this.GetComponent<Rigidbody2D> ();
		RigidBody.velocity = StartVelocity;
	}
	
	// Update is called once per frame
	void Update ()
	{
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
		ResolveNPCCollision (coll);
	}

}
