﻿using UnityEngine;
using System.Collections;

public class Capitalist : Actor
{
	public Vector2 StartVelocity;
	public bool Hit = false;
	public float CommieLookDistance = 64f;
	public GameObject MyProjectile;
	public float FireRate;

	private float nextFire;

	// Use this for initialization
	void Start ()
	{
		BaseStart ();
		animator = this.GetComponent<Animator> ();
		RigidBody = this.GetComponent<Rigidbody2D> ();
		MyDirection = (Direction)Random.Range (0, 4);
		nextFire = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		BaseMovement ();
		CheckForCommie ();
	}
	
	void OnCollisionEnter2D (Collision2D coll)
	{
		if (!Hit) {
			ResolveNPCCollision (coll);
		}
	}

	void CheckForCommie ()
	{
		if (Time.time > nextFire) {
			RaycastHit2D[] hit = Physics2D.RaycastAll (transform.position, RigidBody.velocity.normalized, CommieLookDistance, 
		                                           CollideLayers);
			bool seeCommie = false;
			foreach (RaycastHit2D item in hit) {
				if (item.collider.gameObject != this.gameObject) {
					Commie hitCommie = item.collider.GetComponent<Commie> ();
					if (hitCommie != null) {
						Debug.Log ("Spotted commie");
						seeCommie = true;
					}
				}
			}

			if (seeCommie) {
				FireProjectile (MyProjectile);
				nextFire = Time.time + FireRate;
			}
		}
	}
}