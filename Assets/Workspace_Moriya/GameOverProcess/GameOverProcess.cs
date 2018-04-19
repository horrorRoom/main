/**====================================================================*/
/**
 * 男に捕まった時のゲームオーバーに移行するまでの演出
 * ①プレイヤーを男のほうに向けさせ、プレイヤーと男の座標も調整
 * ②少女をアニメーションで動かす
 * 作成者：守屋　作成日：15/2/20*
/**====================================================================*/
using UnityEngine;
using System.Collections;

public class GameOverProcess : MonoBehaviour {
    /*==所持コンポーネント==*/
    private Transform tr;

    /*==内部設定変数==*/
    //プレイヤー
    private GameObject player;
    //最後に見る地点
    private Vector3 lastLookPos;
    //男
    private GameObject kamiura;
    //後ずさり方向(プレイヤーから男へ向かうベクトル)
    private Vector3 backVec;

    //少女(子オブジェクト)
    private GameObject girl;

    /*==外部設定変数==*/
    //振り向きを終えるまでの時間
    [SerializeField]
    private float m_TurnCompleteTime = 0.4f;
    //処理開始からゲームオーバー画面へ移行するまでの時間
    [SerializeField]
    private float m_SceneChangeTime = 5.0f;
    //少女の初期位置、プレイヤーからの距離
    [SerializeField]
    private float m_GirlInitLength = 3.3f;
    //少女が動き始める時間
    [SerializeField]
    private float m_GirlAnimStartTime = 1.0f;
    //少女のy軸の高さ
    [SerializeField]
    private float m_GirlAxisYHeight = -4.0f;
    //少女が見るy軸の高さ
    [SerializeField]
    private float m_GirlLookAxisYHeight = -4.4f;
    //男が後ずさりをやめる時間
    [SerializeField]
    private float m_BackMoveEndTime = 1.0f;
    //男後ずさり速度
    [SerializeField]
    private float m_BackMoveSpeed = 1.2f;
    //プレイヤーが最後に見るy軸の高さ
    [SerializeField]
    private float m_PlayerLookAxisYHeight = -3.0f;

    // 少女移動調整
    [SerializeField]
    private float m_GirlMoveLength = 1.0f;
    // カメラが少女の顔を拡大する値
    [SerializeField]
    private float m_CameraViewUp = 1.0f;


    /*==================*/
    /* 生成前初期化   */
    /*==================*/
    void Awake()
    {
        //所持コンポーネントの取得
        tr = GetComponent<Transform>();
    }

    /*==================*/
    /* 更新前初期化   */
    /*==================*/
    void Start()
    {
        //ゲームオブジェクト取得
        player = GameObject.FindGameObjectWithTag("Player");
        girl = tr.Find("GameOverGirl").gameObject;

        //少女はまだ動かさない
        girl.SetActive(false);
    }

    //処理開始
    public void ProcessStart(GameObject kamiura_)
    {
        kamiura = kamiura_;

        //プレイヤーの動きをすべて制限
        player.GetComponent<PlayerMove>().playState = (int)State.admiratiopn;

        Vector3 playerPos = player.transform.position;
        Vector3 playerToKamiuraVec = Vector3.Normalize(kamiura.transform.position - playerPos); 
        //少女の位置と向きを設定
        Vector3 girlPos = playerPos + playerToKamiuraVec * m_GirlInitLength;
        girlPos = new Vector3(girlPos.x, m_GirlAxisYHeight, girlPos.z);
        girl.transform.position = girlPos;
        girl.transform.LookAt(new Vector3(playerPos.x, m_GirlLookAxisYHeight, playerPos.z));

        //最後に見る地点設定
        lastLookPos = new Vector3(girlPos.x, m_PlayerLookAxisYHeight, girlPos.z);

        //コルーチン開始
        StartCoroutine(Run());
    }

    //処理
    IEnumerator Run()
    {
        float timer = 0.0f;
        float backTimer = 0.0f;
        //処理開始直前のプレイヤーの目前の座標
        Vector3 initPlayerFrontPos = player.transform.position + player.transform.forward;
        //高さを調整
        initPlayerFrontPos = new Vector3(initPlayerFrontPos.x, m_PlayerLookAxisYHeight, initPlayerFrontPos.z);
        //男の移動直前の座標
        float initKamiuraPosY = kamiura.transform.position.y;
        //後ずさり方向
        backVec = kamiura.transform.position - player.transform.position;
        backVec = Vector3.Normalize(new Vector3(backVec.x,0,backVec.z));


        while (true)
        {
            timer += Time.deltaTime;
            if (timer < m_TurnCompleteTime)
            {
                //プレイヤーとカメラとライトを男に向ける
                player.GetComponent<PlayerMove>().PositionLookAt(Vector3.Lerp(initPlayerFrontPos, lastLookPos, timer / m_TurnCompleteTime));
            }
            else if (backTimer < m_BackMoveEndTime)
            {
                backTimer += Time.deltaTime;
                //後ずさりする
                kamiura.transform.position += backVec * m_BackMoveSpeed * Time.deltaTime;
                //プレイヤーの地点
                Vector3 playerPos = player.transform.position;
                playerPos = Camera.main.transform.position;
                //プレイヤーを向けさせる
                kamiura.transform.LookAt(new Vector3(playerPos.x, initKamiuraPosY, playerPos.z));
                //少女が視る地点更新
                girl.transform.LookAt(new Vector3(playerPos.x, m_GirlLookAxisYHeight, playerPos.z));
                // 少し近づく
                girl.transform.position += girl.transform.forward * Time.deltaTime * m_GirlMoveLength;
               
            }

            if (timer > m_GirlAnimStartTime)
            {
                //少女アニメーション開始
                girl.SetActive(true);


                // カメラで拡大
                Camera.main.fieldOfView -= m_CameraViewUp * Time.deltaTime;
                Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 5, 60);
            }

            if (timer > m_SceneChangeTime)
                //シーン切り替え
                ToGameOver.Apply(player.transform);

            yield return null;
        }
    }

}
