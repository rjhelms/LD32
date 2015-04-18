using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{

	public float FireVelocity;
	public LayerMask TargetMask;
	public Rigidbody2D RigidBody;
	public Vector2 StartVelocity;
	public int MaxDistance = 4096;

	private Vector2 startPosition;

	// Use this for initialization
	void Start ()
	{
		RigidBody = this.GetComponent<Rigidbody2D> ();
		RigidBody.velocity = StartVelocity;
		startPosition = transform.position;

	}
	
	// Update is called once per frame
	void Update ()
	{
		int distance = Mathf.FloorToInt (((Vector2)transform.position - startPosition).sqrMagnitude);
		if (distance > MaxDistance) {
			Debug.Log ("Max distance reached.");
			Destroy (this.gameObject);
		}
	
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.layer != LayerMask.NameToLayer ("Player") 
			&& coll.gameObject.layer != LayerMask.NameToLayer ("Projectile")) {
			if ((TargetMask.value & 1 << coll.gameObject.layer) > 0) {
				Debug.Log ("Target hit: " + coll.gameObject.name);
				Civilian hitCivilian = coll.GetComponent < Civilian> ();
				if (hitCivilian != null) {
					hitCivilian.Hit = true;
					hitCivilian.BecomeCommie ();
				}
			} else {
				Debug.Log ("Non-target hit: " + coll.gameObject.name);
			}

			Destroy (this.gameObject);
			return;
		}
	}
}

