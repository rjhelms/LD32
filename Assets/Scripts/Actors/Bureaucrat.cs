using UnityEngine;
using System.Collections;

public class Bureaucrat : Actor
{

	// Use this for initialization
	void Start ()
	{
		BaseStart ();
		MyDirection = (Direction)Random.Range (0, 4);
	}
	
	// Update is called once per frame
	void Update ()
	{
		BaseMovement ();
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		ResolveNPCCollision (coll);
	}
}
