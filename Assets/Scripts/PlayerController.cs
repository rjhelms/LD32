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
	public float fireRate;

	private float nextFire;

	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		RigidBody = this.GetComponent<Rigidbody2D> ();
		nextFire = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
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

		AmmoText [0].text = (Ammo [0]).ToString ();

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
				FireProjectile ();
				Ammo [CurrentWeapon]--;
				nextFire = Time.time + fireRate;
			}
		}
	}

	void FireProjectile ()
	{
		Vector2 projectileVelocity = new Vector2 ();

		GameObject newProjectileObject = (GameObject)Instantiate (Projectiles [CurrentWeapon], RigidBody.position, Quaternion.identity);
		Projectile newProjectile = newProjectileObject.GetComponent<Projectile> ();

		if (RigidBody.velocity.magnitude > 0) {
			projectileVelocity = RigidBody.velocity.normalized;
		} else {
			switch (MyDirection) {
			case Direction.NORTH:
				projectileVelocity = Vector2.up;
				break;
			case Direction.EAST:
				projectileVelocity = Vector2.right;
				break;
			case Direction.SOUTH:
				projectileVelocity = -Vector2.up;
				break;
			case Direction.WEST:
				projectileVelocity = -Vector2.right;
				break;
			}
		}

		projectileVelocity *= newProjectile.FireVelocity;
		Debug.Log (projectileVelocity);
		newProjectile.StartVelocity = projectileVelocity;
	}
}
