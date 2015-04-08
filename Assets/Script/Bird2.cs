using UnityEngine;
using System.Collections;

public class Bird2 : MonoBehaviour {

	public Transform score;

	float speed = 0;
	bool isDead = false;

	// Use this for initialization
	void Start () {
		speed = Random.Range (3f, 5f);

		GetComponent<Animator>().speed = Random.Range (1.5f, 3f);
	}
	
	// Update is called once per frame
	void Update () {
		float amtMove = speed * Time.smoothDeltaTime;

		if(!isDead){
			transform.Translate (Vector3.right * amtMove);
		}else{
			GetComponent<Animator>().enabled = false;
			transform.Translate(Vector3.down * amtMove, Space.World);
		}

		Vector3 view = Camera.main.WorldToScreenPoint (transform.position);

		if(view.x > Screen.width + 30 || view.y < -30){
			Destroy(gameObject);
		}
	}

	void DropBird(){
	}
}
