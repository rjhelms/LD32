using UnityEngine;
using System.Collections;

public class Capitalist : Actor
{
	public float CommieLookDistance = 64f;
	public GameObject MyProjectile;
	public float FireRate;
	public int RandomFireChance = 1;

	private float nextFire;
	
	// Use this for initialization
	void Start ()
	{
		BaseStart ();

		MyPrefab = MyController.CapitalistPrefab;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (MyController.Running) {
			BaseMovement ();
			CheckForCommie ();
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

			bool randomFire = (Random.Range (0, 100) < RandomFireChance);

			if (seeCommie || randomFire) {
				FireProjectile (MyProjectile);
				nextFire = Time.time + FireRate;
				if (mySprite.isVisible) {
					MyController.SFXSource.PlayOneShot (MyWeaponSound);
				}
			}
		}
	}
}