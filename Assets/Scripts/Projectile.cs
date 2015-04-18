﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

	public float FireVelocity;
	public LayerMask TargetMask;
	public Rigidbody2D RigidBody;
	public Vector2 StartVelocity;
	public int MaxDistance = 4096;
	public WeaponType Type;

	private Vector2 startPosition;

	// Use this for initialization
	void Start ()
	{
		RigidBody = this.GetComponent<Rigidbody2D> ();
		RigidBody.velocity = StartVelocity;
		startPosition = transform.position;

	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		int distance = Mathf.FloorToInt (((Vector2)transform.position - startPosition).sqrMagnitude);
		if (distance > MaxDistance) {
			Debug.Log ("Max distance reached.");
			Destroy (this.gameObject);
		}
	
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		bool validHit = false;

		if (coll.gameObject.layer == LayerMask.NameToLayer ("Projectile")) {
			validHit = false;
		} else if ((Type == WeaponType.MONEY || Type == WeaponType.LEAFLET) 
			&& coll.gameObject.layer != LayerMask.NameToLayer ("Player")) {
			validHit = true;
		} else if (Type == WeaponType.ENEMY_MONEY && coll.GetComponent<Commie> () != null) {
			validHit = true;
		}

		if (validHit) {
			if ((TargetMask.value & 1 << coll.gameObject.layer) > 0) {
				Debug.Log ("Target hit: " + coll.gameObject.name);
				Civilian hitCivilian = coll.GetComponent < Civilian> ();
				Capitalist hitCapitalist = coll.GetComponent<Capitalist> ();
				Commie hitCommie = coll.GetComponent<Commie> ();
				if (hitCivilian != null && Type == WeaponType.LEAFLET) {
					hitCivilian.Hit = true;
					hitCivilian.BecomeCommie ();
				} else if (hitCapitalist != null && Type == WeaponType.MONEY) {
					hitCapitalist.Hit = true;
					hitCapitalist.BecomeCommie ();
				} else if (hitCommie != null && Type == WeaponType.ENEMY_MONEY) {
					hitCommie.Hit = true;
					hitCommie.BecomeCivilian ();
				}
			} else {
				Debug.Log ("Non-target hit: " + coll.gameObject.name);
			}

			Destroy (this.gameObject);
			return;
		}
	}
}

