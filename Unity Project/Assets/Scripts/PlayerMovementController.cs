using UnityEngine;
using System.Collections;

public class PlayerMovementController : MovementController {

	public float gravity;
	public float vVelocity;

	private MoveDirection prevMoveDirection;

	private float newRadius;
	private bool grounded;
	private float myVelocity;

	private EntityController entityController;
	private SoundPlayer soundPlayer;

	public PlayerMovementController ()
	{
		facingRight = false;
		grounded = true;
		myVelocity = 0;
	}

	// Use this for initialization
	void Start () {
		newRadius = radius;
		entityController = GetComponent<EntityController> ();
		soundPlayer = GameObject.Find ("World").GetComponent<SoundPlayer> ();
	}
	
	// Update is called once per frame
	void Update () {
		prevMoveDirection = moveDirection;
		moveDirection = MoveDirection.NONE;
		
		if (!moving) return;
		
		if (Input.GetButton ("Horizontal")) {
			// Cache the horizontal input.
			float h = Input.GetAxis ("Horizontal");

			if (h > 0)
				moveDirection = MoveDirection.RIGHT;
			else if (h < 0)
				moveDirection = MoveDirection.LEFT;

		
			// If the input is moving the player right and the player is facing left...
			if (moveDirection == MoveDirection.RIGHT && !facingRight) Flip ();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (moveDirection == MoveDirection.LEFT && facingRight) Flip ();

			currentAngle = (currentAngle + movementSpeed * (int)moveDirection * Time.deltaTime) % (2 * Mathf.PI);
			transform.rotation = Quaternion.Euler (0, 0, -currentAngle * Mathf.Rad2Deg);
		}

		if (prevMoveDirection == MoveDirection.NONE && moveDirection != MoveDirection.NONE) {
			entityController.SetWalking(true);
		} else if (prevMoveDirection != MoveDirection.NONE && moveDirection == MoveDirection.NONE) {
			entityController.SetWalking(false);
		}

		// If the player should jump...
		if (Input.GetButtonDown ("Jump") && grounded) {
			myVelocity = vVelocity;
			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			grounded = false;
			entityController.SetJumping(true);
			soundPlayer.playJumpSound();
		}
		
		if (!grounded) {
			myVelocity -= gravity;
			newRadius += myVelocity;
			if (newRadius <= radius) {
				grounded = true;
				newRadius = radius;
				entityController.SetJumping(false);
			}
		}
		
		transform.position = new Vector3 (newRadius * Mathf.Sin (currentAngle), newRadius * Mathf.Cos (currentAngle));
	}
}
		