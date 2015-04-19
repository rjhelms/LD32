using UnityEngine;
using System.Collections;

public class Civilian : Actor
{

	// Use this for initialization
	void Start ()
	{
		BaseStart ();

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