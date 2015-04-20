using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{

	public WeaponType Type;
	public float FlashRate = 0.5f;

	private float nextFlash;
	private SpriteRenderer mySprite;

	// Use this for initialization
	void Start ()
	{
		mySprite = GetComponent<SpriteRenderer> ();
		nextFlash = Time.time + FlashRate;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > nextFlash) {
			mySprite.enabled = !mySprite.enabled;
			nextFlash = Time.time + FlashRate;
		}
	}
}
