/**====================================================================*/
/**
 * ①スイッチに近づいて左クリックすると始動
 * ②SpotlightとModelが指定秒数の間、同時に点滅
 * 作成者：守屋　作成日：15/1/4*/
/**====================================================================*/
using UnityEngine;
using System.Collections;

//必須コンポーネント
[RequireComponent(typeof(AudioSource))]
public class SwitchControl : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;
    private BoxCollider collider;

    /*==内部設定変数==*/
    //子オブジェクト
    private GameObject spotLight;
    private GameObject model;
    private GameObject button;

    //プレイヤー
    private GameObject player;
    //オーディオ
    private AudioSource audio;

    //処理開始フラグ
    private bool start = false;
    //点滅に使用
    private bool active = false; 

    /*==外部設定変数==*/
    //開始してから終了までにかける時間
    [SerializeField]
    private float endTime = 10.0f;
    //ガラスが割れる音
    [SerializeField]
    private AudioClip clip;

    /*==================*/
    /* 生成前初期化   */
    /*==================*/
    void Awake()
    {
        //所持コンポーネントの取得
        tr = GetComponent<Transform>();
        collider = GetComponent<BoxCollider>();
        audio = GetComponent<AudioSource>();
    }

    /*==================*/
    /* 更新前初期化   */
    /*==================*/
	void Start()
    {
		//ゲームオブジェクト取得
        spotLight = tr.Find("Spotlight").gameObject;
        model = tr.Find("Model").gameObject;
        button = tr.Find("Button").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
   
        //一応効果音をセットしておく
        audio.clip = clip;

        //当たり判定を無効化
        collider.enabled = false;

        //コルーチン開始
        StartCoroutine(StartCheck());


        //表示しない
        model.SetActive(false);
        spotLight.SetActive(false);
    }

    /*==================*/
    /* 更新   */
    /*==================*/
	void Update()
    {

	}

    /// <summary>
    /// 当たり判定
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        //部屋の当たり判定内にいる男を削除
        if (other.gameObject.tag == "Kamiura")
            Destroy(other.gameObject);
    }

    //ボタンの入力を受け付ける処理
    IEnumerator StartCheck()
    {
        while (true)
        {
            float dist = Vector3.Distance(player.transform.position, button.transform.position);
            bool flag = dist < 2.4f;
            if (flag) print("左クリックで発動");
            
            //Buttonとプレイヤーとの距離が近い　かつ　マウスの左トリガー
            if (flag && Input.GetMouseButtonDown(0))
            {
                //当たり判定を有効化
                collider.enabled = true;

                //ボタン回転処理開始
                yield return StartCoroutine(PushButton());
                //点滅処理開始
                yield return StartCoroutine(Flash());
                //コルーチン終了
                yield break;
            }
            yield return null;
        }
    }

    //SpotlightとModelを点滅させる
    IEnumerator Flash()
    {
        float timer = 0.0f; 
        while (true)
        {
            //6フレームに１度の確率で切り替え
            if (Random.Range(0, 6) == 0)
            {
                active = !active;
            }
            model.SetActive(active);
            spotLight.SetActive(active);

            //終了までの時間が経過したら
            timer += Time.deltaTime;
            if (timer > endTime)
            {
                //効果音再生
                audio.PlayOneShot(clip);
                //非アクティブにしてコルーチン終了
                model.SetActive(false);
                spotLight.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }

    //スイッチ押したとき、ボタンを回転させる処理
    IEnumerator PushButton()
    {
        float timer = 0.0f;
        Vector3 angles = Vector3.zero;
        while (true)
        {
            timer += Time.deltaTime;
            
            //ボタンをy軸回転
            angles = Vector3.Lerp(new Vector3(0, 18, 0), new Vector3(0, -20, 0), timer * 2.0f);
            button.transform.rotation = Quaternion.Euler(angles);

            if (timer > 1.0f) yield break;
            yield return null;
        }
    }
}
