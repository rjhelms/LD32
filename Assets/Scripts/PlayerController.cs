using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Actor
{
	public GameObject[] Projectiles;
	public int[] Ammo = {100, -1, -1};
	public Text[] AmmoText;
	public int CurrentWeapon = 0;
	public float FireRate;

	private float nextFire;

	// Use this for initialization
	void Start ()
	{

		animator = this.GetComponent<Animator> ();
		RigidBody = this.GetComponent<Rigidbody2D> ();
		nextFire = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Moving = false;
		Vector2 moveVector = new Vector2 ();

		ProccesInput (ref moveVector);

		// move player
		if (!Moving) {
			moveVector *= 0;
		} else {
			moveVector = moveVector.normalized * MoveSpeed;
		}

		UpdateAnimation (moveVector.magnitude * AnimationSpeedFactor);

		RigidBody.velocity = moveVector;

		for (int i = 0; i < AmmoText.Length; i++) {
			AmmoText [i].text = (Ammo [i]).ToString ();
		}

	}

	void ProccesInput (ref Vector2 moveVector)
	{
		if (Input.GetKey (KeyCode.W)) {
			MyDirection = Direction.NORTH;
			moveVector += new Vector2 (0, 1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.D)) {
			MyDirection = Direction.EAST;
			moveVector += new Vector2 (1, 0);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.S)) {
			MyDirection = Direction.SOUTH;
			moveVector += new Vector2 (0, -1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.A)) {
			MyDirection = Direction.WEST;
			moveVector += new Vector2 (-1, 0);
			Moving = true;
		}

		if (Input.GetKey (KeyCode.Space)) {
			if (Time.time > nextFire && Ammo [CurrentWeapon] > 0) {
				FireProjectile (Projectiles [CurrentWeapon]);
				Ammo [CurrentWeapon]--;
				nextFire = Time.time + FireRate;
			}
		}

		if (Input.GetKey (KeyCode.Alpha1) && Ammo [0] > 0) {
			CurrentWeapon = 0;
		}

		if (Input.GetKey (KeyCode.Alpha2) && Ammo [1] > 0) {
			CurrentWeapon = 1;
		}
	}
}
