/***********************************************************/
//タイトルシーン
/***********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {
    /// <summary>
    /// スタート地点での移動先GameObject配列
    /// </summary>
	[SerializeField] private GameObject[] StartMovePoint = new GameObject[4];
    /// <summary>
    /// スタート地点のドアGameObject配列
    /// </summary>
	[SerializeField] private GameObject[] StartDoorList = new GameObject[3];
    /// <summary>
    /// レベル選択地点での移動先GameObject配列
    /// </summary>
	[SerializeField] private GameObject[] LevelMovePoint= new GameObject[3];
    /// <summary>
    /// スタート地点のドアGameObject配列
    /// </summary>
	[SerializeField] private GameObject[] LevelDoorList = new GameObject[2];

	public bool moveFlag = false;
	[SerializeField] private bool levelSelect = false;
    [SerializeField] private Fade fade;

    //回転にかかる時間
    public float rotateDuration = 1.0f;
    //移動にかかる時間
    public float moveDuration = 1.0f;
    public int RotateCount = 0;

    /// <summary>
    /// 初期化
    /// </summary>
    void Start () {
        SoundManager.GetInstance().BGMPlay("kagome", SoundManager.SoundPlayerMode.LOOP);
		PlayerPrefs.SetInt ("Count", 0);
	}

    /// <summary>
    /// 更新
    /// </summary>
	void Update () {
		if (!fade.isStart) {
			if (!LeanTween.isTweening (this.gameObject)) {
				TitleInput ();

				if (!levelSelect) StartTitleMove ();
				else if (levelSelect) LevelSelectMove ();
			}
		}
	}

    /// <summary>
    /// タイトルの初期化
    /// </summary>
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
			if (Input.GetKeyDown (KeyCode.Space)) moveFlag = true;
		//}
	}

    /// <summary>
    /// スタート地点での操作
    /// </summary>
    void StartTitleMove(){
		if (moveFlag) {
			if (RotateCount == 0) {
				TitleMove(0, StartMovePoint, false);
				levelSelect = true;
			}
			else if(RotateCount != 0)　STitleMoveCheck();
		}
	}

    /// <summary>
    /// ドアがあるときのMove判定
    /// </summary>
    void STitleMoveCheck(){
        if (StartDoorList[RotateCount - 1]==null) return;
        
		if (StartDoorList [RotateCount-1].GetComponent<OpenDoor> ().isOpen == true && moveFlag==true) {
			TitleMove(0, StartMovePoint, true);

			fade.isEnd = true;

			//fadeし終えたらセーブしていたステージに移動する
			if(fade.fadeAlpha >= 1.0f){
				if (RotateCount == 2) Application.Quit();
                else if (RotateCount == 3) SceneManager.LoadScene("Option");
                else if (RotateCount == 1) {
					int stageNum = PlayerPrefs.GetInt("SceneNum", 1);
                    SceneManager.LoadScene("Stage" + stageNum);
                } 
			}
		}
	}

    /// <summary>
    /// レベル選択
    /// </summary>
	void LevelSelectMove(){
		if (moveFlag) {
			if (RotateCount == 2) {
				TitleMove (0, LevelMovePoint, false);
				levelSelect = false;
			} else if (RotateCount != 2 && RotateCount != 3) {
				LTitleMoveCheck();
			}
		}
	}

    /// <summary>
    /// そのままレベルのステージに行く
    /// </summary>
	void LTitleMoveCheck(){
		if (LevelDoorList [RotateCount].GetComponent<OpenDoor> ().isOpen && moveFlag) {
            SoundManager.GetInstance().SEPlay("putOpenDoor");
            TitleMove(0, LevelMovePoint, true);


			fade.isEnd = true;

			//fadeし終えたらセーブしていたステージに移動する
			if(fade.fadeAlpha >= 1.0f){
				if (RotateCount == 0) SceneManager.LoadScene("Stage1");
				else if (RotateCount == 1) SceneManager.LoadScene("Stage1");
			}
		}
	}

    /// <summary>
    /// 回転操作
    /// </summary>
    /// <param name="delay">回転に入るまでのラグタイム</param>
    void TitleRotate(float delay){
		LeanTween.rotateY(this.gameObject, 90.0f*RotateCount, rotateDuration).setDelay(delay);
	}

    /// <summary>
    /// 移動操作
    /// </summary>
    /// <param name="delay">移動に入るまでのラグタイム</param>
    /// <param name="obj">移動目標のGameObject配列</param>
    /// <param name="flag">配列番号</param>
    void TitleMove(float delay, GameObject[] obj, bool flag){
		LeanTween.moveLocal(this.gameObject, obj[RotateCount].transform.position, moveDuration).setDelay(delay);
		moveFlag = flag;
	}
}
