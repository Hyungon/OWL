using UnityEngine;
using System.Collections;

public class Owl : MonoBehaviour {

	public Transform branch;
	public Transform bird;
	public Transform gift;
	public AudioClip sndOver;
	Transform spPoint;
	Transform newBranch;

	int speedSide = 6;
	int speedJump = 14;
	int gravity = 24;

	Vector3 dir = Vector3.zero;
	float maxY = 0;

	float score = 0;
	int birdCnt = 0;
	int giftCnt = 0;
	int giftScore = 0;

	bool isDead = false;
	bool isMobile = false;

	void Start(){
		isMobile = Application.platform == RuntimePlatform.Android ||
			Application.platform == RuntimePlatform.IPhonePlayer;

//		Screen.showCursor = false;

		spPoint = GameObject.Find ("SpawnPoint").transform;

		newBranch = Instantiate (branch, spPoint.position, Quaternion.identity) as Transform;
	}

	void Update(){
		if (isDead)
			return;

		CheckBranch ();
		MoveOwl ();
		SetCamera ();

		MakeBird();

		score = maxY * 1000 - birdCnt * 1000 + giftScore;
	}

	void CheckBranch(){
		RaycastHit hit;

		if (Physics.Raycast (transform.position, Vector3.down, out hit, 0.45f)) {
			if (dir.y < 0 && hit.transform.tag == "BRANCH") {
				dir.y = speedJump;
				hit.transform.GetComponent<AudioSource>().Play();

				Vector3 pos = transform.position;
				pos.y -= 0.3f;

				GameObject particle = Resources.Load ("Particle/DustStorm") as GameObject;
				Instantiate(particle, pos, Quaternion.identity);
			}
		}
	}

	void MoveOwl(){
		Vector3 view = Camera.main.WorldToScreenPoint (transform.position);

		if (view.y < -30) {
			isDead = true;
			return;
		}

		dir.x = 0;

		if (isMobile) {
		}else {
			float key = Input.GetAxis ("Horizontal");

			if ((key < 0 && view.x > 30) || (key > 0 && view.x < Screen.width - 30)) {
				dir.x = key * speedSide;
			}
		}

		dir.y -= gravity * Time.deltaTime;
		transform.Translate (dir * Time.smoothDeltaTime);

		int n = (dir.y > 0) ? 2 : 1;
		transform.GetComponent<Renderer>().material.mainTexture = Resources.Load ("owl" + n) as Texture2D;

	}

	void SetCamera(){
		if (transform.position.y > maxY) {
			maxY = transform.position.y;

			Camera.main.transform.position = new Vector3 (0, maxY - 1.6f, -5);
		}

		float dist = spPoint.position.y - newBranch.position.y;

		if (dist >= 3) {
			Vector3 pos = spPoint.position;

			pos.x = Random.Range (-5f, 5f) * 0.5f;
			newBranch = Instantiate (branch, pos, Quaternion.identity) as Transform;

			int x = (Random.Range(0,2) == 0)?-1:1;
			int y = (Random.Range(0,2) == 0)?-1:1;

			newBranch.GetComponent<Renderer>().material.mainTextureScale = new Vector2(x,y);

			newBranch.localScale = new Vector3(Random.Range(1.2f,2), 0.7f, 1);
		}
	}

	void MakeBird(){
		if (Random.Range (0,1000)<970) return;

		Vector3 pos = new Vector3(0, 0 , 0.3f);
		pos.y = maxY + Random.Range (2f, 4f);

		if (Random.Range (0,100)<50){
			pos.x = -10;
			Instantiate(bird, pos, Quaternion.identity);
		}else{
			int n1 = GameObject.FindGameObjectsWithTag ("GIFT1").Length;
			int n2 = GameObject.FindGameObjectsWithTag ("GIFT2").Length;
			int n3 = GameObject.FindGameObjectsWithTag ("GIFT3").Length;

			if(n1 + n2 + n3 >= 5) return;

			pos.x = Random.Range (-4f, 4f);
			pos.y = maxY + Random.Range (3f, 4f);
			pos.z = 0.4f;

			Transform obj = Instantiate (gift, pos, Quaternion.identity) as Transform;

			int n = Random.Range (1, 4);
			SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
			render.sprite = Resources.Load<Sprite>("gift" + n);

			obj.tag = "GIFT" + n;
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.tag == "BIRD"){
			if(col.transform.eulerAngles.z != 0) return;

			birdCnt++;

			col.SendMessage ("DropBird", SendMessageOptions.DontRequireReceiver);

			GameObject particle = Resources.Load ("Particle/Fire") as GameObject;
			Instantiate(particle, col.transform.position, Quaternion.identity);			                                  
		}

		if(col.tag.Contains ("GIFT")){
			int n = int.Parse (col.tag.Substring (4, 1));
			giftScore = n * 500;
			giftCnt++;

			col.SendMessage ("GetGift", SendMessageOptions.DontRequireReceiver);

			GameObject particle = Resources.Load ("Particle/Spark") as GameObject;
			Instantiate(particle, col.transform.position, Quaternion.identity);
		}
	}

	void OnGUI(){
		float x1 = 20;
		float x2 = Screen.width / 2 - 50;
		float x3 = Screen.width - 100;

		int size = (isMobile) ? 36 : 20;

//		string s1 = string.Format("<color=navy><size={0:0}>Score:{1:#,0}</size></color>", size, score);
//		string s2 = string.Format ("<color=red><size={0:0}>Bird:{1:#,0}</size></color>",size, birdCnt);
//		string s3 = string.Format ("<color=#004000><size={0:0}>Gift:{1:#,0}</size></color>", size, giftCnt);

//		GUI.Label(new Rect(x1, 10, 200, 50), s1);
//		GUI.Label(new Rect(x2, 10, 200, 50), s2);
//		GUI.Label(new Rect(x3, 10, 200, 50), s3);

		string s1 = string.Format ("<size={0:0}>Score : {1:#,0}</size>",size, score);
		string s2 = string.Format ("<size={0:0}>Bird : {1:#,0}</size>", size, birdCnt);
		string s3 = string.Format ("<size={0:0}>Gift : {1:#,0}</size>", size, giftCnt);

		OutlineText (x1, 20, s1, "navy");
		OutlineText (x2, 20, s2, "red");
		OutlineText (x3, 20, s3, "#004000");

		if(!isDead) return;

		if(Camera.main.GetComponent<AudioSource>().clip != sndOver){
			Camera.main.GetComponent<AudioSource>().clip = sndOver;
			Camera.main.GetComponent<AudioSource>().loop = false;
			Camera.main.GetComponent<AudioSource>().Play();
		}

		Cursor.visible = true;

		float x = Screen.width/2;
		float y = Screen.height/2;

		if(GUI.Button (new Rect(x-80, y-50, 160, 50), "Play Again?")){
			Application.LoadLevel ("MainGame");
		}

		if(GUI.Button (new Rect(x-80, y+50, 160, 50), "Quit Game?")){
			Application.Quit ();
		}
	}

	void OutlineText(float x, float y, string text, string color){
		string str = string.Format ("<color=white>{0:a}</color>", text);

		GUI.Label(new Rect(x-2, y, 300, 50), str);
		GUI.Label(new Rect(x, y-2, 300, 50), str);
		GUI.Label(new Rect(x+2, y, 300, 50), str);
		GUI.Label(new Rect(x, y+2, 300, 50), str);

		str = string.Format ("<color={1:a}>{0:a}</color>", text, color);
		GUI.Label (new Rect(x, y, 300, 50), str);
	}

}
