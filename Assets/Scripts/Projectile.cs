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
		MyController = Source.MyController;
		RigidBody = this.GetComponent<Rigidbody2D> ();
		StartVelocity *= (1 + (MyController.CurrentLevel * MyController.SpeedUpFactor));
		RigidBody.velocity = StartVelocity;
		startPosition = transform.position;
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
		if (coll.gameObject.layer == LayerMask.NameToLayer ("Projectile") 
			|| coll.gameObject.layer == LayerMask.NameToLayer ("Powerup")
			|| Source == null || Source.gameObject == null || coll.gameObject == null 
			|| coll.gameObject == Source.gameObject) {
			return;
		}

		if (coll.gameObject != null) {
			try {
				if ((TargetMask.value & 1 << coll.gameObject.layer) > 0 && MyController.Running) {
					Debug.Log ("Target hit: " + coll.gameObject.name);

					switch (Type) {
					case WeaponType.LEAFLET:
						LeafletHit (coll);
						break;
					case WeaponType.MONEY:
						MoneyHit (coll);
						break;
					case WeaponType.MEGAPHONE:
						MegaphoneHit (coll);
						break;
					case WeaponType.ENEMY_MONEY:
						EnemyMoneyHit (coll);
						break;
					case WeaponType.ENEMY_MEGAPHONE:
						EnemyMegaphoneHit (coll);
						break;
					}

				}
			} catch (NullReferenceException e) {
				Debug.Log ("Got a null collision: " + e.ToString ());
			}
		}

		Destroy (this.gameObject);
		return;
	}

	void LeafletHit (Collider2D coll)
	{
		Civilian hitCivilian = coll.GetComponent<Civilian> ();
		if (hitCivilian != null) {
			hitCivilian.Hit = true;
			hitCivilian.BecomeCommie ();
			MyController.Score += 100;
			return;
		}
	}

	void MoneyHit (Collider2D coll)
	{
		Capitalist hitCapitalist = coll.GetComponent<Capitalist> ();
		if (hitCapitalist != null) {
			hitCapitalist.Hit = true;
			hitCapitalist.BecomeCommie ();
			MyController.Score += 200;
			MyController.HitPoints += 1;
			return;
		}

		Bureaucrat hitBureaucrat = coll.GetComponent<Bureaucrat> ();

		if (hitBureaucrat != null) {
			
			if (!hitBureaucrat.Enraged) {
				hitBureaucrat.Hit = true;
				hitBureaucrat.BecomeCommie ();
				MyController.Score += 200;
				MyController.Ammo [2] += 2;
				MyController.HitPoints += 1;
				return;
			}
		}
	}

	void MegaphoneHit (Collider2D coll)
	{
		Civilian hitCivilian = coll.GetComponent<Civilian> ();

		if (hitCivilian != null) {
			hitCivilian.Hit = true;
			hitCivilian.BecomeCommie ();
			MyController.Score += 100;
			return;
		}

		Bureaucrat hitBureaucrat = coll.GetComponent<Bureaucrat> ();

		if (hitBureaucrat != null) {
			hitBureaucrat.BecomeEnraged ();
			return;
		}

	}

	void EnemyMoneyHit (Collider2D coll)
	{
		Commie hitCommie = coll.GetComponent<Commie> ();

		if (hitCommie != null) {
			hitCommie.Hit = true;
			hitCommie.BecomeCivilian ();
			MyController.Score -= 100;
			MyController.HitPoints -= 2;
			return;
		}

		PlayerController hitPlayer = coll.GetComponent<PlayerController> ();

		if (hitPlayer != null && !hitPlayer.Dead) {
			MyController.Ammo [1]++;
			MyController.SFXSource.PlayOneShot (MyController.PowerUpSound);
			return;
		}
	}

	void EnemyMegaphoneHit (Collider2D coll)
	{
		Commie hitCommie = coll.GetComponent<Commie> ();

		if (hitCommie != null) {
			hitCommie.Hit = true;
			hitCommie.BecomeCivilian ();
			MyController.Score -= 100;
			MyController.HitPoints -= 2;
			return;
		}

		PlayerController hitPlayer = coll.GetComponent<PlayerController> ();

		if (hitPlayer != null && !hitPlayer.Dead) {
			MyController.HitPoints -= 1;
			MyController.SFXSource.PlayOneShot (MyController.PlayerHitSound);
			return;
		}
	}

}

