using UnityEngine;
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

	protected Vector2 startPosition;

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
		bool validHit = true;

		if (coll.gameObject.layer == LayerMask.NameToLayer ("Projectile")) {
			validHit = false;
		} else if (coll.gameObject == Source.gameObject) {
			validHit = false;
		}

		if (validHit) {
			if ((TargetMask.value & 1 << coll.gameObject.layer) > 0) {
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
					break;
				case WeaponType.MEGAPHONE:
					hitCivilian = coll.GetComponent<Civilian> ();
					hitBureaucrat = coll.GetComponent<Bureaucrat> ();
					break;
				case WeaponType.ENEMY_MONEY:
					hitCommie = coll.GetComponent<Commie> ();
					hitPlayer = coll.GetComponent<PlayerController> ();
					break;
				}

				if (hitCivilian != null && (Type == WeaponType.LEAFLET || Type == WeaponType.MEGAPHONE)) {
					hitCivilian.Hit = true;
					hitCivilian.BecomeCommie ();
					Source.MyController.Score += 100;
				} else if (hitCapitalist != null && Type == WeaponType.MONEY) {
					hitCapitalist.Hit = true;
					hitCapitalist.BecomeCommie ();
					Source.MyController.Score += 200;
				} else if (hitCommie != null && Type == WeaponType.ENEMY_MONEY) {
					hitCommie.Hit = true;
					hitCommie.BecomeCivilian ();
					Source.MyController.Score -= 50;
				} else if (hitPlayer != null && Type == WeaponType.ENEMY_MONEY) {
					Source.MyController.Ammo [1]++;
				} else if (hitBureaucrat != null && Type == WeaponType.MEGAPHONE) {
					hitBureaucrat.BecomeEnraged ();
				}
			} else {
				Debug.Log ("Non-target hit: " + coll.gameObject.name);
			}

			Destroy (this.gameObject);
			return;
		}
	}
}

