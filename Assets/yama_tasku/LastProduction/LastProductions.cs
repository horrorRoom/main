using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LastProductions : MonoBehaviour {
	private GameObject player;
	public GameObject kami;
	public GameObject girl;
	private GameObject camera;
	public ParticleSystem finishEffect;
	public ParticleSystem bsp; //blackSphereParticle
	private GameObject fade; 
	public Image whiteOut;
	public GameObject Door;
	private Vector3 startAngle;
	private Vector3 endAngle;
	public GameObject DoorFront;

	private float mainTimer = 0.0f;
	private float addTimer = 0.0f;
	float lastTimer = 0f, blackOutTimer = 0f;
	public float walkSp = 0.5f, dashSp = 1.0f;	//プレイヤーの速度(落とすときの値)

	private float distance;
	private float[] addDistance= {20,13,5,3};	//ノイズを加算するプレイヤーと少女の距離
	public float add_N_Value = 0.5f; 			//加算するノイズの値
	public int add_EmissionRate = 10;			//加算するEmissionRateの値
	public float add_LimitTime = 10.0f; 		//加算するまでの時間
	public float finishLimitTime = 60.0f;		//ゲーム終了までのタイムリミット

	bool blackOutFlag = false, openDoorFlag = false;

	int count = 0;
	BossMove mbossMove;
	int result;
	public float rotateSpeed = 0.1f;
	float rotateTimeF=0f,rotateTimeS=0f;
	bool start=false, first = false, fadeFlag = false;
	public float moveDuration;													//移動にかかる時間

	void Start () {
		NoiseManager.Instance.Init ();
		player = GameObject.FindGameObjectWithTag ("Player");
		//girl = GameObject.Find ("Girl_movei");
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		//bsp = GameObject.Find ("blackSphere").GetComponent<ParticleSystem>();
		//tp = GameObject.Find ("TyphoonParticle").GetComponent<ParticleSystem>();
		fade = GameObject.FindGameObjectWithTag("Fade");
		mbossMove = kami.GetComponent<BossMove> ();

		startAngle = Door.transform.eulerAngles;
		endAngle = new Vector3 (startAngle.x, startAngle.y + 100f, startAngle.z);
	}

	void Update () {
		if (mbossMove.ReturnLastFlag() == true) {
			if (player == null) return;
			player.GetComponent<PlayerMove> ().SetMoveSpeed(walkSp, dashSp);
			bsp.gameObject.SetActive (true);
			//キーに触れていない間, MainTimerを加算しない
			mainTimer += Time.deltaTime;
			if(!Input.anyKey)addTimer += Time.deltaTime;

			AddNoiseValue ();
			LimitFinish ();


			LookAtObject (girl.transform, kami.transform, 3f, 1f);
			BlackOut ();
			OpenDoor ();

			if(Door.activeSelf==true){
				Vector3 newRotation2 = Quaternion.LookRotation(DoorFront.transform.position - player.transform.position).eulerAngles;
				player.transform.rotation = Quaternion.Slerp (player.transform.rotation, Quaternion.Euler(newRotation2), Time.deltaTime*rotateSpeed);
			}

			if (fadeFlag == true) {

				LeanTween.moveLocal(player,DoorFront.transform.position,moveDuration).setDelay(2);
			}
		}
	}

	//ノイズ加算処理まとめ
	void AddNoiseValue(){
		if (fade == null || count>addDistance.Length-1) return;
		distance = Vector3.Distance (player.transform.position, girl.transform.position);

		//一定距離近づくたび値を加算する
		if (((int)distance / (int)addDistance[count]) == 0) {
			if (count > addDistance.Length-2) {
				//初期化
				InitializeProduction ();
				ClearStaging ();
			} else {
				NoiseManager.Instance.AddNoise (add_N_Value);
				AddParticle ();
			}
			count++;
		}

		//一定時間操作がなければ値を加算する
		if (addTimer > add_LimitTime) {
			NoiseManager.Instance.AddNoise (add_N_Value);
			addTimer = 0;
		}

		if (Input.GetKeyDown (KeyCode.W)) {
			addTimer = 0;
		}
	}

	void AddParticle(){
		bsp.emissionRate = bsp.emissionRate + add_EmissionRate;
	}

	void InitializeProduction(){
		NoiseManager.Instance.Init ();
		bsp.emissionRate = 0;
	}

	//ゲーム強制終了処理
	void LimitFinish(){
		if (player == null)	return;
		if (mainTimer > finishLimitTime) {
			player.GetComponent<PlayerMove> ().SetMoveSpeed(0, 0);
			fade.GetComponent<Fade>().isEnd = true;

			//fadeし終えたらセーブしていたステージに移動する
			if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f){
                ToGameOver.Apply(player.transform);	
			}
		}
	}

	void BlackOut(){
		if (blackOutFlag != true) return;
		whiteOut.gameObject.SetActive(true);
		blackOutTimer += Time.deltaTime;

		Color color = whiteOut.color;
		color.a -= Time.deltaTime;
		whiteOut.color = color;

		if (blackOutTimer > 1.0f) {
			blackOutFlag = false;
		}
	}

	void OpenDoor(){
		if (openDoorFlag == true) {
			lastTimer += Time.deltaTime;

			//扉開ける
			Door.transform.eulerAngles = Vector3.Lerp (startAngle, endAngle, lastTimer);

			if (lastTimer > 1.0f) {
				openDoorFlag = false;
			} 
		}
	}

	//少女を見る～男を見るまで
	void LookAtObject(Transform trans1,Transform trans2,float firstRimit, float secondRimit){
		if (start != true) return;

		if (first == false) {
			Vector3 newRotation1 = Quaternion.LookRotation(trans1.position - player.transform.position).eulerAngles;
			player.transform.rotation = Quaternion.Slerp (player.transform.rotation, Quaternion.Euler(newRotation1), Time.deltaTime*rotateSpeed);

			rotateTimeF += Time.deltaTime;

			if (rotateTimeF >= firstRimit) {
				first = true;
			}
		}

		if (first == true) {
			//少女消える
			girl.SetActive(false);
			rotateTimeS += Time.deltaTime;

			Vector3 newRotation2 = Quaternion.LookRotation(trans2.position - player.transform.position).eulerAngles;
			player.transform.rotation = Quaternion.Slerp (player.transform.rotation, Quaternion.Euler(newRotation2), Time.deltaTime*rotateSpeed);

			if (rotateTimeS >= secondRimit) {
				start = false;
			}
		}
	}

	void ClearStaging(){

		StartCoroutine (Staging());
	}

	IEnumerator Staging(){
		player.GetComponent<PlayerMove> ().enabled = false;
		player.GetComponent<MouseLook> ().enabled = false;
		player.transform.Find("Spotlight").GetComponent<MouseLook> ().enabled = false;
		camera.GetComponent<MouseLook> ().enabled = false;

		//少女を見る～男を見るまで
		start = true;

		yield return new WaitForSeconds(5);
		//パーティクル出す
		ParticleSystem fi = Instantiate(finishEffect,kami.transform.position,Quaternion.identity)as ParticleSystem;
		yield return new WaitForSeconds(3);

		//ブラックアウトする
		whiteOut.gameObject.SetActive(true);

		//男消える
		kami.SetActive(false);
		//パーティクル消える
		Destroy (fi);
		yield return new WaitForSeconds(3);
		blackOutFlag = true;
		Door.SetActive (true);

		yield return new WaitForSeconds(5);

		openDoorFlag = true;

		yield return new WaitForSeconds(5);

		fadeFlag = true;

		yield return new WaitForSeconds (1);
		//フェードする
		fade.GetComponent<Fade>().isEnd = true;

		//fadeし終えたらセーブしていたステージに移動する
		if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f){
			result = PlayerPrefs.GetInt ("Count");
			if (result >= 10) {
				Debug.Log ("Happy");
				Application.LoadLevel ("");
			} else if (result < 10 || result == null) {
				Debug.Log ("EndA");
				Application.LoadLevel ("EndA");
			}
		}
	}
}
