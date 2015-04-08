using UnityEngine;
using System.Collections;

public class SKY : MonoBehaviour {

	float Speed = 0.03f;
	
	// Update is called once per frame
	void Update () {
		float ofs = Speed * Time.time;

		transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (ofs, 0);
	
	}
}
