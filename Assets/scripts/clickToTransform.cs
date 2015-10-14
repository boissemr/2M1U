using UnityEngine;
using System.Collections;

public class clickToTransform : MonoBehaviour {
	
	[Tooltip("the object that will replace this one when it is clicked on")]			public GameObject	target;
	[Tooltip("whether starting position will be relative to the companion object")]		public bool			positionRelative;
	[Tooltip("whether starting rotation will be inherited from the companion object")]	public bool			inheritRotation;
	[Tooltip("starting position")]														public Vector3		startingPosition;
	[Tooltip("starting rotation")]														public Quaternion	startingRotation;

	void Start() {

		// set position and rotation according to whether or not they are set to be relative
		transform.position = startingPosition + (positionRelative ? transform.position : Vector3.zero);
		if(!inheritRotation) transform.rotation = startingRotation;
	}
	
	void OnMouseDown() {

		// create the target in this object's place
		Instantiate(target, this.transform.position, this.transform.rotation);
		Object.Destroy(this.gameObject);
	}
}