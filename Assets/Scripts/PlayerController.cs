using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Actor
{
	public GameObject[] Projectiles;
	public int CurrentWeapon = 0;
	public float FireRate;
	public Camera MyCamera;
	public bool Dead = false;
	public float ReviveTime = 3f;
	public float DeadFlashSpeed = 0.333f;

	private float nextFlash;
	private float nextRevive;

	private float nextFire;


	// Use this for initialization
	void Start ()
	{
		BaseStart ();
		nextFire = Time.time;
		MyCamera = GameObject.FindObjectOfType<Camera> ();
	}

	void Update ()
	{

	}
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (MyController.Running) {
			if (Dead) {
				if (Time.time > nextRevive) {
					Dead = false;
					mySprite.enabled = true;
				} else if (Time.time > nextFlash) {
					mySprite.enabled = !mySprite.enabled;
					nextFlash = Time.time + DeadFlashSpeed;
				}
			}

			Hit = false;
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
			CentreCamera ();

		} else {
			RigidBody.velocity = Vector2.zero;
		}
	}

	void ProccesInput (ref Vector2 moveVector)
	{
		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.Keypad8) 
			|| Input.GetKey (KeyCode.UpArrow)) {
			MyDirection = Direction.NORTH;
			moveVector += new Vector2 (0, 1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.Keypad6) || Input.GetKey (KeyCode.RightArrow)) {
			MyDirection = Direction.EAST;
			moveVector += new Vector2 (1, 0);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.Keypad2) || Input.GetKey (KeyCode.DownArrow)) {
			MyDirection = Direction.SOUTH;
			moveVector += new Vector2 (0, -1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.Keypad4) 
			|| Input.GetKey (KeyCode.LeftArrow)) {
			MyDirection = Direction.WEST;
			moveVector += new Vector2 (-1, 0);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.Keypad7) || Input.GetKey (KeyCode.Home)) {
			MyDirection = Direction.WEST;
			moveVector += new Vector2 (-1, 1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.Keypad9) || Input.GetKey (KeyCode.PageUp)) {
			MyDirection = Direction.EAST;
			moveVector += new Vector2 (1, 1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.Keypad3) || Input.GetKey (KeyCode.PageDown)) {
			MyDirection = Direction.EAST;
			moveVector += new Vector2 (1, -1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.Keypad1) || Input.GetKey (KeyCode.End)) {
			MyDirection = Direction.WEST;
			moveVector += new Vector2 (-1, -1);
			Moving = true;
		}

		if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl)) {
			if (Time.time > nextFire && MyController.Ammo [CurrentWeapon] > 0) {
				FireProjectile (Projectiles [CurrentWeapon]);
				MyController.Ammo [CurrentWeapon]--;
				nextFire = Time.time + FireRate;
				MyController.SFXSource.PlayOneShot (MyController.PlayerWeaponSounds [CurrentWeapon]);
			}
		}



		if (Input.GetKey (KeyCode.Alpha1) && MyController.Ammo [0] > 0) {
			CurrentWeapon = 0;
			MyController.WeaponSelectorImage.rectTransform.localPosition = MyController.WeaponSelectorPositions [0];
		}

		if (Input.GetKey (KeyCode.Alpha2) && MyController.Ammo [1] > 0) {
			CurrentWeapon = 1;
			MyController.WeaponSelectorImage.rectTransform.localPosition = MyController.WeaponSelectorPositions [1];
		}

		if (Input.GetKey (KeyCode.Alpha3) && MyController.Ammo [2] > 0) {
			CurrentWeapon = 2;
			MyController.WeaponSelectorImage.rectTransform.localPosition = MyController.WeaponSelectorPositions [2];
		}
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Powerup")) {
			switch (coll.GetComponent<Powerup> ().Type) {
			case WeaponType.LEAFLET:
				MyController.Ammo [0] += 5;
				break;
			case WeaponType.MONEY:
				MyController.Ammo [1] += 10;
				break;
			case WeaponType.MEGAPHONE:
				MyController.Ammo [2] += 5;
				break;
			case WeaponType.MEDKIT:
				MyController.HitPoints += 4;
				break;
			}
			MyController.PowerupCount++;
			MyController.Score += 100;

			MyController.SFXSource.PlayOneShot (MyController.PowerUpSound);
			Destroy (coll.gameObject);
		}
	}

	public void CentreCamera ()
	{
		MyCamera.transform.position = new Vector3 (Mathf.Floor (transform.position.x), 
		                                               Mathf.Floor (transform.position.y) - 16, -10);
	}

	public void Die ()
	{
		Dead = true;
		nextRevive = Time.time + ReviveTime;
		nextFlash = Time.time + DeadFlashSpeed;
		mySprite.enabled = false;
	}
}