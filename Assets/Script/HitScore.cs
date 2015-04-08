using UnityEngine;
using System.Collections;

public class HitScore : MonoBehaviour {

	int fontSize = 16;
	float speed = 0.05f;

	void Start(){
		if(Application.platform == RuntimePlatform.Android ||
		   Application.platform == RuntimePlatform.IPhonePlayer){
			fontSize = 30;
		}

		transform.GetComponent<GUIText>().fontSize = fontSize;
		StartCoroutine ("Fadeout");
	}
	
	// Update is called once per frame
	void Update () {
		float amtMove = speed * Time.deltaTime;

		transform.Translate (Vector3.up * amtMove);
	}

	IEnumerator Fadeout(){
		yield return new WaitForSeconds(0.5f);

		for(float a = 1; a >= 0; a -= 0.02f){
			transform.GetComponent<GUIText>().material.color = new Vector4(1,1,1,a);

			yield return new WaitForFixedUpdate();
		}

		Destroy(gameObject);
	}
}
