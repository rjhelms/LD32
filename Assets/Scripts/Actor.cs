using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour
{

	public string WalkN = "WalkN";
	public string WalkE = "WalkE";
	public string WalkS = "WalkS";
	public string WalkW = "WalkW";
	
	public float MoveSpeed = 50f;
	public float AnimationSpeedFactor = 0.03f;
	
	public Direction Direction;
	public bool Moving;
	
	protected Animator animator;
	protected Rigidbody2D rigidBody;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	protected void UpdateAnimation (float speed)
	{
		// update sprite
		switch (Direction) {
		case Direction.NORTH:
			animator.Play (WalkN);
			break;
		case Direction.EAST:
			animator.Play (WalkE);
			break;
		case Direction.SOUTH:
			animator.Play (WalkS);
			break;
		case Direction.WEST:
			animator.Play (WalkW);
			break;
		}

		animator.speed = speed;
	}
}