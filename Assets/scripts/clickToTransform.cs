using UnityEngine;
using System.Collections;

public class clickToTransform : MonoBehaviour {

	public GameObject target;

	void OnMouseDown() {
		Instantiate(target, this.transform.position, this.transform.rotation);
		Object.Destroy(this.gameObject);
	}
}