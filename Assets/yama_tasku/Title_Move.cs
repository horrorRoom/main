using UnityEngine;
using System.Collections;

public class Title_Move : MonoBehaviour {
	public bool moveFlag = false;
	private Transform startPos;
	[SerializeField] private Transform endPos;

	public float time = 0.0f;
	[SerializeField] private float RotateTime = 1.0f;
	
	[SerializeField] private Transform[] rotateList = new Transform[4];
	[SerializeField] private Transform[] LevelList = new Transform[2];

	public int nowNum = 0;
	public bool levelselect = false;
	
	public float minAngle = 0.0F;
	public float maxAngle = 90.0F;

	private int stageNum;
	private GameObject fade;
	[SerializeField] private GameObject[] doorList = new GameObject[3];
	[SerializeField] private GameObject[] leveldoorList = new GameObject[2];
	
	private bool rotateFlag = false;
	public float checkangle;
	private float angle;
	public Vector3 initPos;


	public string NowName;

	[HideInInspector] public int count = 0;

	[SerializeField] private string[] levelname = new string[2];

	void Start () {
		fade = GameObject.FindGameObjectWithTag("Fade");
		startPos = this.transform;
		initPos = startPos.position;

		checkangle = transform.rotation.y;
	}

	void Update() {
		Debug.Log ("num:"+nowNum);
		if (fade.GetComponent<Fade> ().isStart != true) {

			if (moveFlag != true) TitleRotate ();
			if (moveFlag == true && nowNum == 0 && rotateFlag != true && levelselect == false) LerpMove ();
			
			ChengeAngle ();
		
			if (levelselect == false) LerpMoveS ();
			else if (levelselect == true) LevelMove ();


			if (levelselect == true && moveFlag != true) LevelRotate();

			//if (nowNum == null) return;
			if (Input.GetKeyDown (KeyCode.Space) && rotateFlag != true) moveFlag = true;
		}
	}

	//回転のキー入力(実際回してるのは後述のChengeAngle関数)
	void TitleRotate () {
		if (rotateFlag != true) {
			if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
				nowNum++;
				if(nowNum > 3) nowNum = 0;

				maxAngle = minAngle + 90.0f;
				rotateFlag = true;
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
				nowNum--;
				if(nowNum < 0) nowNum = 3;
				 
				maxAngle = minAngle - 90.0f;
				rotateFlag = true;
			}
		}
	}

	void LerpMove () {
		time += 0.5f * Time.deltaTime;
		transform.position = Vector3.Lerp(startPos.position, rotateList[nowNum].position, time / 10.0f);
	
		if (time >= 1.0f) {
			time = 0.0f;
			moveFlag = false;
			levelselect = true;
			count=1;
		}
	}

	//Scene移動用
	void LerpMoveS () {
		if (nowNum == 0) return;
		if (doorList [nowNum - 1].GetComponent<OpenDoor> ().isOpen == true) {
			time += 0.5f * Time.deltaTime;
			transform.position = Vector3.Lerp(startPos.position, rotateList[nowNum].position, time / 10.0f);
			
			if (time >= 1.0f) {
				time = 0.0f;
				moveFlag = false;
			}
			
			SceneMove();
		}
	}

	void LevelRotate(){
		if (rotateFlag != true) {
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				moveFlag = false;
				nowNum++;
				if(nowNum > 3) nowNum = 0;
				
				maxAngle = minAngle + 90.0f;
				rotateFlag = true;
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				moveFlag = false;
				nowNum--;
				if(nowNum < 0) nowNum = 3;
				
				maxAngle = minAngle - 90.0f;
				rotateFlag = true;
			}
		}
	}

	void LevelMove(){

		if (moveFlag == true) {
			if(nowNum == 2){
				Debug.Log("levelMove in");
				time += 0.5f * Time.deltaTime;
				startPos = this.transform;
				
				transform.position = Vector3.Lerp (startPos.position, initPos, time / 10.0f);
				
				if (time >= 1.0f) {
					time = 0.0f;
					moveFlag = false;
				}
			}

			if (leveldoorList [nowNum] == null) return;
			if (leveldoorList [nowNum].GetComponent<OpenDoor> ().isOpen == true) {
				time += 0.5f * Time.deltaTime;
				if(nowNum==0)transform.position = Vector3.Lerp(startPos.position, LevelList[0].position, time / 10.0f);
				if(nowNum==1)transform.position = Vector3.Lerp(startPos.position, LevelList[1].position, time / 10.0f);
				
				if (time >= 1.0f) {
					time = 0.0f;
					moveFlag = false;
				}
				
				SceneMove();
			}
		}
	}

	void ChengeAngle(){
		if (rotateFlag == false) return;
		time += RotateTime * Time.deltaTime;

		angle = Mathf.LerpAngle (minAngle, maxAngle, time);
		transform.eulerAngles = new Vector3(0, angle, 0);

		if (time >= 1.0f) {
			time = 0;

			minAngle = transform.eulerAngles.y;
			rotateFlag = false;
		}
	}

	void SceneMove(){
		if(levelselect == false){
			if(nowNum == 1){
				fade.GetComponent<Fade>().isEnd = true;

				//fadeし終えたらセーブしていたステージに移動する
				if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f){
					stageNum = PlayerPrefs.GetInt("SceneNum");
					if(stageNum == 0) stageNum = 1;

					Application.LoadLevel("Stage" + stageNum);
				}
			}
			if(nowNum == 2){
				fade.GetComponent<Fade>().isEnd = true;

				//fadeし終えたらゲームを終了する
				if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f) Application.Quit();
			}
			if(nowNum == 3){
				fade.GetComponent<Fade>().isEnd = true;

				//fadeし終えたらゲームを終了する
				if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f) Application.LoadLevel("OptionScene");
			}
		}
		else if(levelselect == true){
			//指定されたレベルのステージに移動する
			if(nowNum == 0){
				fade.GetComponent<Fade>().isEnd = true;
				
				//fadeし終えたらゲームを終了する
				if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f) Application.LoadLevel(levelname[0]);
			}
			if(nowNum == 1){
				fade.GetComponent<Fade>().isEnd = true;
				
				//fadeし終えたらゲームを終了する
				if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f)Application.LoadLevel(levelname[1]);
			}
		}
	}
}
