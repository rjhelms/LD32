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
				Civilian hitCivilian = coll.GetComponent < Civilian> ();
				Capitalist hitCapitalist = coll.GetComponent<Capitalist> ();
				Commie hitCommie = coll.GetComponent<Commie> ();
				PlayerController hitPlayer = coll.GetComponent<PlayerController> ();
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
				}
			} else {
				Debug.Log ("Non-target hit: " + coll.gameObject.name);
			}

			Destroy (this.gameObject);
			return;
		}
	}
}

