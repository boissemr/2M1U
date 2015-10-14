using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	// public parameters
	public float	walkSpeed,
					carryingSpeed,
					pickupReach,
					jumpForce;
	public Vector3	carryingPosition;

	// private variables
	RaycastHit[]	pickups;
	GameObject		pickup,
					carrying;
	LayerMask		pickupable;
	CharacterController controller;
	Vector3			movement;

	// start
	void Start() {

		// initialize variables
		movement = Vector3.zero;
		controller = GetComponent<CharacterController>();

		// layer masks
		pickupable = 1 << 8;
	}

	// update
	void Update() {

		// detect all pickupable objects around the player
		//pickups = Physics.SphereCastAll(transform.position, pickupReach*.66f, transform.forward, pickupReach, pickupable);
		pickups = Physics.SphereCastAll(transform.position, pickupReach, transform.forward, 0, pickupable);

		// pick one (the closest) out of the pickupable objects
		if(pickups.Length > 0) {
			System.Array.Sort(pickups, SortByDistance);
			pickup = pickups[0].collider.gameObject;
			Debug.DrawLine(transform.position, pickup.transform.position);
		}

		// pick up or drop stuff
		if(Input.GetButtonDown("Grab")) {
			if(carrying == null && pickups.Length > 0) {
				// pick it up
				carrying = pickup;
				//transform.LookAt(new Vector3(carrying.transform.position.x, transform.position.y, carrying.transform.position.z));
				carrying.transform.position = transform.position + (Quaternion.Euler(0, transform.eulerAngles.y, 0) * carryingPosition);
			} else {
				// drop it
				carrying = null;
			}
		}
		if(carrying != null) {
			// drop it if it gets snagged
			if(Vector3.Distance(transform.position, carrying.transform.position) > pickupReach) {
				carrying = null;
			}
		}
	}

	// fixedupdate
	void FixedUpdate() {

		// character controller
		Quaternion r = transform.rotation;
		if(controller.isGrounded) {
			movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			movement = transform.TransformDirection(movement);
			movement *= walkSpeed;
			if(Input.GetButton("Jump")) movement.y = jumpForce;
		}
		movement.y -= 20 * Time.deltaTime;
		transform.rotation = Quaternion.identity;
		controller.Move(movement * Time.deltaTime);
		transform.rotation = r;

		// carrying and player rotation
		if(carrying != null) {
			// if we are carrying something, we move it and look at it
			carrying.transform.position += movement;
			//transform.LookAt(new Vector3(carrying.transform.position.x, transform.position.y, carrying.transform.position.z));
		} else if(movement.magnitude > .05) {
			// if we're not carrying anything, we look ahead
			//transform.LookAt(movement);
		}

	}
	
	// sort two objects by distance from the player
	int SortByDistance(RaycastHit a, RaycastHit b) {
		float	aMagnitude = (transform.position - a.collider.transform.position).sqrMagnitude,
				bMagnitude = (transform.position - b.collider.transform.position).sqrMagnitude;
		return	aMagnitude.CompareTo(bMagnitude);
	}

	// return true if on the ground
	bool Grounded() {
		return Physics.Raycast(transform.position, Vector3.down, 1.1f);
	}
}
