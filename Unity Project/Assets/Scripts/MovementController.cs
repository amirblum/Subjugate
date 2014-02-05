using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

	public enum MoveDirection {LEFT = -1, NONE = 0, RIGHT = 1};

	public float movementSpeed;
	public float currentAngle;
	public float radius;
	public MoveDirection moveDirection;
	protected bool moving;

	public MovementController() {
		moving = true;
	}

	
	[HideInInspector]
	public bool facingRight;			// For determining which way the player is currently facing.

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!moving) return;
		currentAngle = (currentAngle + movementSpeed * (int)moveDirection * Time.deltaTime) % (2 * Mathf.PI);
		transform.rotation = Quaternion.Euler (0, 0, -currentAngle * Mathf.Rad2Deg);
		transform.position = new Vector3 (radius * Mathf.Sin (currentAngle), radius * Mathf.Cos (currentAngle));
	}
	
	public void SetMoving(bool isMoving){
		moving = isMoving;
	}	

	public void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
