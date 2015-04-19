using UnityEngine;
using System.Collections;

public class Commie : Actor
{

	public GameObject MyProjectile;
	public int FireChance = 1;
	public float FireRate;

	private float nextFire;

	
	// Use this for initialization
	void Start ()
	{
		BaseStart ();
		MyPrefab = MyController.CommiePrefab;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (MyController.Running) {
			BaseMovement ();

			if (Time.time > nextFire) {
				int willFire = Random.Range (0, 100);
				if (willFire < FireChance) {
					FireProjectile (MyProjectile);
					nextFire = Time.time + FireRate;
					if (mySprite.isVisible) {
						MyController.SFXSource.PlayOneShot (MyWeaponSound);
					}
				}
			}

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
