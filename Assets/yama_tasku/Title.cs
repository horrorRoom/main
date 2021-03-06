﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトルシーン
/// </summary>
public class Title : MonoBehaviour {
    [Header("Object")]
    [SerializeField]
    private GameObject titlePlayer;
    [Header("System")]
    [SerializeField]
    private Fade fade;

    [Header("List")]
    /// <summary>
    /// スタート地点での移動先GameObject配列
    /// </summary>
	[SerializeField] private GameObject[] StartMovePoint = new GameObject[4];
    /// <summary>
    /// スタート地点のドアGameObject配列
    /// </summary>
	[SerializeField] private OpenDoor[] StartDoorList = new OpenDoor[3];
    /// <summary>
    /// レベル選択地点での移動先GameObject配列
    /// </summary>
	[SerializeField] private GameObject[] LevelMovePoint= new GameObject[3];
    /// <summary>
    /// スタート地点のドアGameObject配列
    /// </summary>
	[SerializeField] private OpenDoor[] LevelDoorList = new OpenDoor[1];
    
    [Header("Parameter")]
    //回転にかかる時間
    [SerializeField]
    private float rotateDuration = 1.0f;
    //移動にかかる時間
    [SerializeField]
    private float moveDuration = 1.0f;

    //見ている方向(選択中のもの)
    [SerializeField] private int rotateCount = 0;
    //移動中か
    [SerializeField] private bool moveFlag = false;
    //レベル選択中
    [SerializeField] private bool levelSelect = false;

    //最初に選択できるドアの数
    private const int SELECT_DOOR_NUM = 3;

    //最初に選択できるドアの種類
    private enum FirstDoor
    {
        LevelSelect=1,
        Exit = 2,
        Option=3,
    }

    //最初に選択できるドアの種類
    private enum LevelSelectDoor
    {
        FirstDoor = 2,
        Wall = 3,
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Start () {
        SoundManager.GetInstance().BGMPlay("kagome", Sound.PlayerMode.LOOP);
		PlayerPrefs.SetInt ("Count", 0);
	}

    /// <summary>
    /// 更新
    /// </summary>
	void Update () {
        //フェード中なら処理しない
        if (fade.IsStart()) return;

        if (!LeanTween.isTweening(titlePlayer))
        {
            //移動入力
            TitleInput();

            //もし移動先がドアならシーン移動
            if (!levelSelect) StartTitleMove();
            else if (levelSelect) LevelSelectMove();
        }
    }

    /// <summary>
    /// 移動入力
    /// </summary>
	void TitleInput()
    {
        if (moveFlag) return;

        if (Input.GetKeyDown(KeyCode.D))
        {
            rotateCount++;
            TitleRotate(0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            rotateCount--;
            TitleRotate(0);
        }
        if (rotateCount > SELECT_DOOR_NUM) rotateCount = 0;
        if (rotateCount < 0) rotateCount = SELECT_DOOR_NUM;

        if (levelSelect && rotateCount == SELECT_DOOR_NUM) return;
        if (rotateCount == 1) return;
        //移動(決定)
        if (Input.GetKeyDown(KeyCode.Space)) moveFlag = true;
    }

    /// <summary>
    /// スタート地点での操作
    /// </summary>
    void StartTitleMove(){
        if (!moveFlag) return;

        if (rotateCount == 0)
        {
            TitleMove(0, StartMovePoint, false);
            levelSelect = true;
        }
        else if (rotateCount != 0) STitleMoveCheck();
    }

    /// <summary>
    /// ドアがあるときのMove判定
    /// </summary>
    void STitleMoveCheck(){
        if (StartDoorList[rotateCount - 1]==null) return;
        
		if (StartDoorList [rotateCount - 1].IsOpen()  && moveFlag) {
			TitleMove(0, StartMovePoint, false);

			fade.FadeOut(()=> {
                if (rotateCount == (int)FirstDoor.Exit) Application.Quit();
                else if (rotateCount == (int)FirstDoor.Option) SceneManager.LoadScene("Option");
                else if (rotateCount == (int)FirstDoor.LevelSelect)
                {
                    int stageNum = PlayerPrefs.GetInt("SceneNum", 1);
                    SceneManager.LoadScene("Stage" + stageNum);
                }
            });
		}
	}

    /// <summary>
    /// レベル選択
    /// </summary>
	void LevelSelectMove(){
        if (!moveFlag) return;

        if (rotateCount == (int)LevelSelectDoor.FirstDoor)
        {
            TitleMove(0, LevelMovePoint, false);
            levelSelect = false;
        }
        else if (rotateCount != (int)LevelSelectDoor.FirstDoor && rotateCount != (int)LevelSelectDoor.Wall)
        {
            GameStageMove();
        }
    }

    /// <summary>
    /// そのままレベルのステージに行く
    /// </summary>
	void GameStageMove(){
		if (LevelDoorList [rotateCount].IsOpen() && moveFlag) {
            SoundManager.GetInstance().SEPlay("putOpenDoor");
            TitleMove(0, LevelMovePoint, false);

            fade.FadeOut(()=> {
                //fadeし終えたらセーブしていたステージに移動する
                if (rotateCount == 0) SceneManager.LoadScene("Stage1");
                else if (rotateCount == 1) SceneManager.LoadScene("Stage1");
            });
		}
	}

    /// <summary>
    /// 回転操作
    /// </summary>
    /// <param name="delay">回転に入るまでのラグタイム</param>
    void TitleRotate(float delay){
		LeanTween.rotateY(titlePlayer, 90.0f* rotateCount, rotateDuration).setDelay(delay);
	}

    /// <summary>
    /// 移動操作
    /// </summary>
    /// <param name="delay">移動に入るまでのラグタイム</param>
    /// <param name="obj">移動目標のGameObject配列</param>
    /// <param name="flag">移動フラグをどうするか</param>
    void TitleMove(float delay, GameObject[] obj, bool flag){
		LeanTween.moveLocal(titlePlayer, obj[rotateCount].transform.position, moveDuration).setDelay(delay);
		moveFlag = flag;
	}

    /// <summary>
    /// 移動中か
    /// </summary>
    public bool IsMove() { return moveFlag; }

    /// <summary>
    /// 向いている方向
    /// </summary>
    public int GetRotateCount() { return rotateCount; }
}
