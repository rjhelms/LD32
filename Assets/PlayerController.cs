using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public int WalkNHash = Animator.StringToHash ("WalkN");
	public int WalkEHash = Animator.StringToHash ("WalkE");
	public int WalkSHash = Animator.StringToHash ("WalkS");
	public int WalkWHash = Animator.StringToHash ("WalkW");

	public float MoveSpeed = 50f;
	public float AnimationSpeedFactor = 0.25f;

	public Direction Direction;
	public bool Moving;

	private Animator animator;
	private Rigidbody2D rigidBody;
	// Use this for initialization
	void Start ()
	{
		animator = this.GetComponent<Animator> ();
		rigidBody = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Moving = false;
		Vector2 moveVector = new Vector2 ();

		// get input
		if (Input.GetKey (KeyCode.W)) {
			Direction = Direction.NORTH;
			moveVector += new Vector2 (0, 1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.D)) {
			Direction = Direction.EAST;
			moveVector += new Vector2 (1, 0);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.S)) {
			Direction = Direction.SOUTH;
			moveVector += new Vector2 (0, -1);
			Moving = true;
		}
		if (Input.GetKey (KeyCode.A)) {
			Direction = Direction.WEST;
			moveVector += new Vector2 (-1, 0);
			Moving = true;
		}

		// update sprite
		switch (Direction) {
		case Direction.NORTH:
			animator.Play (WalkNHash);
			break;
		case Direction.EAST:
			animator.Play (WalkEHash);
			break;
		case Direction.SOUTH:
			animator.Play (WalkSHash);
			break;
		case Direction.WEST:
			animator.Play (WalkWHash);
			break;
		}

		// move player
		if (!Moving) {
			moveVector *= 0;
		} else {
			moveVector = moveVector.normalized * MoveSpeed;
		}

		animator.speed = moveVector.magnitude * AnimationSpeedFactor;
		rigidBody.velocity = moveVector;
	}
}