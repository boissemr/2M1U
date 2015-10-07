using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour {

	// public parameters
	public float	walkSpeed,
					pickupReach;

	// private variables
	RaycastHit[]	pickups;
	GameObject		pickup;
	LayerMask		pickupable;

	// start
	void Start() {
		pickupable = 1 << 8;
	}

	// update
	void Update() {
		// detect all pickupable objects around the player
		pickups = Physics.SphereCastAll(transform.position, .8f, transform.forward, 1.2f, pickupable);

		// pick one (the closest) out of the pickupable objects
		if(pickups.Length > 0) {
			System.Array.Sort(pickups, SortByDistance);
			pickup = pickups[0].collider.gameObject;
			Debug.DrawLine(transform.position, pickup.transform.position);
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

		// rotate player if moving
		if (movement.magnitude > .05) {
			transform.LookAt (transform.position + movement);
		}
	}
	
	// sort two objects by distance from the player
	int SortByDistance(RaycastHit a, RaycastHit b) {
		float	aMagnitude = (transform.position - a.collider.transform.position).sqrMagnitude,
				bMagnitude = (transform.position - b.collider.transform.position).sqrMagnitude;
		return	aMagnitude.CompareTo(bMagnitude);
	}
}
