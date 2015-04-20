using UnityEngine;
using System;
using System.Collections;

public class Projectile : MonoBehaviour
{

	public float FireVelocity;
	public LayerMask TargetMask;
	public Rigidbody2D RigidBody;
	public Vector2 StartVelocity;
	public int MaxDistance = 4096;
	public WeaponType Type;
	public Actor Source;
	public GameController MyController;

	protected Vector2 startPosition;

	// Use this for initialization
	void Start ()
	{
		RigidBody = this.GetComponent<Rigidbody2D> ();
		RigidBody.velocity = StartVelocity;
		startPosition = transform.position;
		MyController = Source.MyController;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		int distance = Mathf.FloorToInt (((Vector2)transform.position - startPosition).sqrMagnitude);
		if (distance > MaxDistance) {
			Destroy (this.gameObject);
		}
	
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		bool validHit = true;

		if (coll.gameObject.layer == LayerMask.NameToLayer ("Projectile") 
			|| coll.gameObject.layer == LayerMask.NameToLayer ("Powerup")) {
			validHit = false;
		} else if (Source == null || Source.gameObject == null || coll.gameObject == null) {
			validHit = false;
		} else if (coll.gameObject == Source.gameObject) {
			validHit = false;
		}

		if (validHit) {
			if (coll.gameObject != null) {
				try {
					if ((TargetMask.value & 1 << coll.gameObject.layer) > 0 && MyController.Running) {
						Debug.Log ("Target hit: " + coll.gameObject.name);

						Civilian hitCivilian = null;
						Capitalist hitCapitalist = null;
						Commie hitCommie = null;
						Bureaucrat hitBureaucrat = null;
						PlayerController hitPlayer = null;

						switch (Type) {
						case WeaponType.LEAFLET:
							hitCivilian = coll.GetComponent<Civilian> ();
							break;
						case WeaponType.MONEY:
							hitCapitalist = coll.GetComponent<Capitalist> ();
							hitBureaucrat = coll.GetComponent<Bureaucrat> ();
							break;
						case WeaponType.MEGAPHONE:
							hitCivilian = coll.GetComponent<Civilian> ();
							hitBureaucrat = coll.GetComponent<Bureaucrat> ();
							break;
						case WeaponType.ENEMY_MONEY:
							hitCommie = coll.GetComponent<Commie> ();
							hitPlayer = coll.GetComponent<PlayerController> ();
							break;
						case WeaponType.ENEMY_MEGAPHONE:
							hitCommie = coll.GetComponent<Commie> ();
							hitPlayer = coll.GetComponent<PlayerController> ();
							break;
						}

						if (hitBureaucrat != null && Type == WeaponType.MEGAPHONE) {
					
							hitBureaucrat.BecomeEnraged ();
					
						} else if (hitBureaucrat != null && Type == WeaponType.MONEY) {

							if (!hitBureaucrat.Enraged) {
								hitBureaucrat.Hit = true;
								hitBureaucrat.BecomeCommie ();
								MyController.Score += 200;
								MyController.HitPoints += 1;
							}

						} else if (hitCapitalist != null && Type == WeaponType.MONEY) {
					
							hitCapitalist.Hit = true;
							hitCapitalist.BecomeCommie ();
							MyController.Score += 200;
							MyController.HitPoints += 1;
					
						} else if (hitCivilian != null && (Type == WeaponType.LEAFLET || Type == WeaponType.MEGAPHONE)) {

							hitCivilian.Hit = true;
							hitCivilian.BecomeCommie ();
							MyController.Score += 100;

						} else if (hitCommie != null 
							&& (Type == WeaponType.ENEMY_MONEY || Type == WeaponType.ENEMY_MEGAPHONE)) {

							hitCommie.Hit = true;
							hitCommie.BecomeCivilian ();
							MyController.Score -= 100;
							MyController.HitPoints -= 2;

						} else if (hitPlayer != null && Type == WeaponType.ENEMY_MONEY) {

							MyController.Ammo [1]++;
							MyController.SFXSource.PlayOneShot (MyController.PowerUpSound);

						} else if (hitPlayer != null && Type == WeaponType.ENEMY_MEGAPHONE) {
				
							MyController.HitPoints -= 1;
							MyController.SFXSource.PlayOneShot (MyController.PlayerHitSound);
				
						}
					}
				} catch (NullReferenceException e) {
					Debug.Log ("Got a null collision: " + e.ToString ());
				}
			}

			Destroy (this.gameObject);
			return;
		}
	}
}

