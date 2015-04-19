using UnityEngine;
using System.Collections;

public class MegaphoneSource : Projectile
{

	public GameObject[] MegaphoneComponents;

	// Use this for initialization
	void Start ()
	{
		startPosition = transform.position;

		// instantiate child projectiles
		GameObject newProjectileObject;
		Projectile newProjectile;

		// Megaphone_N
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [0], startPosition, 
		                                                          Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (0, 1).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// Megaphone_NE
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [1], startPosition, 
		                                               Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (1, 1).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// Megaphone_E
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [2], startPosition, 
		                                               Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (1, 0).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// Megaphone_SE
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [3], startPosition, 
		                                               Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (1, -1).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// Megaphone_S
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [4], startPosition, 
		                                               Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (0, -1).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// Megaphone_SW
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [5], startPosition, 
		                                               Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (-1, -1).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// Megaphone_W
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [6], startPosition, 
		                                               Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (-1, 0).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// Megaphone_NW
		newProjectileObject = (GameObject)Instantiate (MegaphoneComponents [7], startPosition, 
		                                               Quaternion.identity);
		newProjectile = newProjectileObject.GetComponent<Projectile> ();
		newProjectile.StartVelocity = new Vector2 (-1, 1).normalized * newProjectile.FireVelocity;
		newProjectile.Source = this.Source;
		newProjectile.transform.parent = this.transform.parent;

		// finally, destroy self
		Destroy (this.gameObject);
	}
}
