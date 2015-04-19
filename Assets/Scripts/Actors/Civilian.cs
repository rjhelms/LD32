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
		BaseMovement ();
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!Hit) {
			ResolveNPCCollision (coll);
		}
	}
	
}