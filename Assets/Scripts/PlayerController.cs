using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Actor
{
	public GameObject[] Projectiles;
	public Text[] AmmoText;
	public Text ScoreText;
	public int CurrentWeapon = 0;
	public float FireRate;
	public Camera MyCamera;

	private float nextFire;

	// Use this for initialization
	void Start ()
	{
		BaseStart ();
		nextFire = Time.time;
		MyCamera = GameObject.FindObjectOfType<Camera> ();
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
			AmmoText [i].text = (MyController.Ammo [i]).ToString ();
		}

		ScoreText.text = MyController.Score.ToString ();
		MyCamera.transform.position = new Vector3 (Mathf.Floor (transform.position.x), 
		                                           Mathf.Floor (transform.position.y) - 16, -10);
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
			if (Time.time > nextFire && MyController.Ammo [CurrentWeapon] > 0) {
				FireProjectile (Projectiles [CurrentWeapon]);
				MyController.Ammo [CurrentWeapon]--;
				nextFire = Time.time + FireRate;
			}
		}

		if (Input.GetKey (KeyCode.R)) {
			Application.LoadLevel ("scene1");
		}

		if (Input.GetKey (KeyCode.Alpha1) && MyController.Ammo [0] > 0) {
			CurrentWeapon = 0;
		}

		if (Input.GetKey (KeyCode.Alpha2) && MyController.Ammo [1] > 0) {
			CurrentWeapon = 1;
		}
	}
}
