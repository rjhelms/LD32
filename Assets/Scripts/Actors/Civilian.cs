using UnityEngine;
using System.Collections;

public class Civilian : Actor
{

	public bool Hit = false;

	// Use this for initialization
	void Start ()
	{
		BaseStart ();

		if (StartVelocity.sqrMagnitude == 0) {
			MyDirection = (Direction)Random.Range (0, 4);
		} else {
			RigidBody.velocity = StartVelocity;
		}

		MyPrefab = MyController.CivilianPrefab;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (MyController.Running) {
			BaseMovement ();
		} else {
			RigidBody.velocity = Vector2.zero;
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!Hit) {
			ResolveNPCCollision (coll);
		}
	}
	
}