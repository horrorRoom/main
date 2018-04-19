using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {
	public float rotateDuration; 												//回転にかかる時間
	public float moveDuration;													//移動にかかる時間
	public int RotateCount = 0;

	[SerializeField] private GameObject[] StartMovePoint = new GameObject[4];	//スタート地点での移動先GameObject配列
	[SerializeField] private GameObject[] StartDoorList = new GameObject[3];	//スタート地点のドアGameObject配列

	[SerializeField] private GameObject[] LevelMovePoint= new GameObject[3];	//レベル選択地点での移動先GameObject配列
	[SerializeField] private GameObject[] LevelDoorList = new GameObject[2];	//スタート地点のドアGameObject配列

	public bool moveFlag = false;
	[SerializeField] private bool levelSelect = false;
	private GameObject fade;
	private int stageNum;

	void Start () {
		PlayerPrefs.SetInt ("Count", 0);
		fade = GameObject.FindGameObjectWithTag("Fade");
	}

	void Update () {
		if (fade.GetComponent<Fade> ().isStart != true) {
			if (LeanTween.isTweening (this.gameObject) != true) {
				TitleInput ();

				if (levelSelect == false) StartTitleMove ();
				else if (levelSelect == true) LevelSelectMove ();
			}
		}
	}

	void TitleInput(){
		//if(LeanTween.isTweening(this.gameObject)!=true){
		if (moveFlag != true) {
			if (Input.GetKeyDown (KeyCode.D)) {
				RotateCount++;
				if (RotateCount > 3)
					RotateCount = 0;

				TitleRotate (0);
			}
			if (Input.GetKeyDown (KeyCode.A)) {
				RotateCount--;
				if (RotateCount < 0)
					RotateCount = 3;

				TitleRotate (0);
			}
		}

			if (levelSelect == true && RotateCount == 3) return;
            if (RotateCount == 1) return;
			if (Input.GetKeyDown (KeyCode.Space)) {
                moveFlag = true;
			}
		//}
	}

	//スタート地点での操作
	void StartTitleMove(){
		if (moveFlag == true) {
			if (RotateCount == 0) {
				TitleMove(0, StartMovePoint, false);
				levelSelect = true;
			}
			else if(RotateCount != 0) {
				STitleMoveCheck();
			}
		}
	}

	//ドアがあるときのMove判定
	void STitleMoveCheck(){
        if (StartDoorList[RotateCount - 1]==null) return;
        
		if (StartDoorList [RotateCount-1].GetComponent<OpenDoor> ().isOpen == true && moveFlag==true) {
			TitleMove(0, StartMovePoint, true);

			fade.GetComponent<Fade>().isEnd = true;

			//fadeし終えたらセーブしていたステージに移動する
			if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f){
				if (RotateCount == 2){
					Application.Quit ();
				}
				else if (RotateCount == 3) {
					Application.LoadLevel ("Option");
				}
				else if (RotateCount == 1) {
					stageNum = PlayerPrefs.GetInt("SceneNum", 1);
					Application.LoadLevel("Stage" + stageNum);
				} 
			}
		}
	}

	void LevelSelectMove(){
		if (moveFlag == true) {
			if (RotateCount == 2) {
				TitleMove (0, LevelMovePoint, false);
				levelSelect = false;
			} else if (RotateCount != 2 && RotateCount != 3) {
				LTitleMoveCheck();
			}
		}
	}

	void LTitleMoveCheck(){
		if (LevelDoorList [RotateCount].GetComponent<OpenDoor> ().isOpen == true && moveFlag==true) {
			TitleMove(0, LevelMovePoint, true);


			fade.GetComponent<Fade>().isEnd = true;

			//fadeし終えたらセーブしていたステージに移動する
			if(fade.GetComponent<Fade>().fadeAlpha >= 1.0f){
				if (RotateCount == 0){
					Application.LoadLevel ("Stage1");
				}
				else if (RotateCount == 1) {
					Application.LoadLevel ("Stage1");
				}
			}
		}
	}


	//回転操作
	//delay：回転に入るまでのラグタイム
	void TitleRotate(float delay){
		LeanTween.rotateY(this.gameObject, 90.0f*RotateCount, rotateDuration).setDelay(delay);
	}

	//移動操作
	//delay：移動に入るまでのラグタイム
	//obj：移動目標のGameObject配列
	//num：配列番号
	void TitleMove(float delay, GameObject[] obj, bool flag){
		LeanTween.moveLocal(this.gameObject, obj[RotateCount].transform.position, moveDuration).setDelay(delay);
		moveFlag = flag;
	}
}
