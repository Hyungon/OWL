using UnityEngine;
using System.Collections;

public class Gift : MonoBehaviour {

	public Transform score;

	// Update is called once per frame
	void Update () {
		Vector3 view = Camera.main.WorldToScreenPoint(transform.position);

		if(view.y < -30)
			Destroy(gameObject);
	}

	void GetGift(){

		GetComponent<AudioSource>().Play();

		int n = int.Parse (transform.tag.Substring (4,1));

		Transform obj = Instantiate (score) as Transform;
		obj.GetComponent<GUIText>().text = string.Format ("{0:+#,0}",n*500);
		obj.GetComponent<GUIText>().color = new Vector4(0, 0.3f, 0, 1);

		Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
		obj.position = pos;

		Destroy (gameObject);
	}
}
