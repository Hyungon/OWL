using UnityEngine;
using System.Collections;

public class Bird1 : MonoBehaviour {

	public Transform score;
	
	int imgCnt = 6;
	int imgNum = 0;
	int imgPerSec = 0;
	float imgDelay = 0;

	float speed = 0;
	bool isDead = false;

	// Use this for initialization
	void Start () {
		imgPerSec = Random.Range (10, 19);
		imgDelay = 1f / imgPerSec;

		speed = Random.Range (3f, 5f);	
	}
	
	// Update is called once per frame
	void Update () {
		float amtMove = speed * Time.smoothDeltaTime;

		if (!isDead) {
			AnimationBird ();
			transform.Translate (Vector3.right * amtMove);
		} else {
			transform.Translate (Vector3.down * amtMove, Space.World);
		}

		Vector3 view = Camera.main.WorldToScreenPoint (transform.position);
		if (view.x > Screen.width + 30 || view.y < -30) {
			Destroy (gameObject);
		}
	}

	void AnimationBird(){
		imgDelay -= Time.deltaTime;
		if (imgDelay > 0)
			return;

		imgNum = (int)Mathf.Repeat (++imgNum, imgCnt);
		float ofs = 1f / imgCnt * imgNum;

		transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2 (ofs, 0);

		imgDelay = 1f / imgPerSec;
	}

	void DropBird(){

		GetComponent<AudioSource>().Play();

		Transform obj = Instantiate (score) as Transform;
		obj.GetComponent<GUIText>().text = "-1,000";
		obj.GetComponent<GUIText>().color = Color.red;

		Vector3  pos = Camera.main.WorldToViewportPoint(transform.position);
		obj.position = pos;

		transform.eulerAngles = new Vector3(0, 0, 180);
		isDead = true;

	}
}
