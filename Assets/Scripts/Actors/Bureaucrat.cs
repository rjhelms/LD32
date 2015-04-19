using UnityEngine;
using System.Collections;

public class Bureaucrat : Actor
{
	public bool Enraged;
	public float EnragedFlashRate = 1f;
	public float EnragedDuration = 5f;

	private float nextFlashTime;
	private float calmTime;
	private SpriteRenderer mySprite;

	// Use this for initialization
	void Start ()
	{
		Enraged = false;
		BaseStart ();
		MyDirection = (Direction)Random.Range (0, 4);
		mySprite = this.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if (Enraged) {
			EnragedUpdate ();
		} else {
			BaseMovement ();
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		ResolveNPCCollision (coll);
	}
	
	void EnragedUpdate ()
	{
		if (Time.time > calmTime) {
			Enraged = false;
			mySprite.enabled = true;
		} else if (Time.time > nextFlashTime) {
			mySprite.enabled = !mySprite.enabled;
			nextFlashTime = Time.time + EnragedFlashRate;
		}
		BaseMovement ();
	}

	public void BecomeEnraged ()
	{
		Enraged = true;
		nextFlashTime = Time.time + EnragedFlashRate;
		calmTime = Time.time + EnragedDuration;
		Debug.Log ("grrr...");
	}
}