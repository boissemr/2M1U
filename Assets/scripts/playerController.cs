using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	// public parameters
	public float	walkSpeed,
					pickupReach;
	public Vector3	carryingPosition;

	// private variables
	RaycastHit[]	pickups;
	GameObject		pickup,
					carrying;
	LayerMask		pickupable;

	// start
	void Start() {
		pickupable = 1 << 8;
	}

	// update
	void Update() {
		// detect all pickupable objects around the player
		pickups = Physics.SphereCastAll(transform.position, pickupReach*.66f, transform.forward, pickupReach, pickupable);

		// pick one (the closest) out of the pickupable objects
		if(pickups.Length > 0) {
			System.Array.Sort(pickups, SortByDistance);
			pickup = pickups[0].collider.gameObject;
			Debug.DrawLine(transform.position, pickup.transform.position);
		}

		// pick up or drop stuff
		if(Input.GetButtonDown("Fire1")) {
			if(carrying == null && pickups.Length > 0) {
				// pick it up
				carrying = pickup;
				transform.LookAt(new Vector3(carrying.transform.position.x, transform.position.y, carrying.transform.position.z));
				carrying.transform.position = transform.position + (Quaternion.Euler(0, transform.eulerAngles.y, 0) * carryingPosition);
			} else {
				// drop it
				carrying = null;
			}
		}
	}

	// fixedupdate (for physics stuff)
	void FixedUpdate() {
		// get player inputs
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		// move player around
		Vector3 movement = new Vector3(h*walkSpeed, 0, v*walkSpeed);
		transform.position += movement;

		// carrying and player rotation
		if(carrying != null) {
			// if we are carrying something, we move it and look at it
			carrying.transform.position += movement;
			transform.LookAt(new Vector3(carrying.transform.position.x, transform.position.y, carrying.transform.position.z));
		} else if(movement.magnitude > .05) {
			// if we're not carrying anything, we look ahead
			transform.LookAt(transform.position + movement);
		}
	}
	
	// sort two objects by distance from the player
	int SortByDistance(RaycastHit a, RaycastHit b) {
		float	aMagnitude = (transform.position - a.collider.transform.position).sqrMagnitude,
				bMagnitude = (transform.position - b.collider.transform.position).sqrMagnitude;
		return	aMagnitude.CompareTo(bMagnitude);
	}
}
