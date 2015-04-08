using UnityEngine;
using System.Collections;

public class Branch : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		Vector3 view = Camera.main.WorldToScreenPoint (transform.position);

		if (view.y < -30) {
			Destroy (gameObject);
		}
	}
}
