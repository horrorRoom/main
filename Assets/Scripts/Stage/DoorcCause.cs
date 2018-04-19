using UnityEngine;
using System.Collections;

public class DoorcCause : MonoBehaviour {
    /*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField]
    private float endAngle;          //最終的に向けたいy軸アングル
    [SerializeField]
    private float endTime;           //何秒かけて閉じたいか

    /*==内部設定変数==*/
    private GameObject goal;        //親であるゴールオブジェクト 
    private float startAngle;       //開始時のアングル（Scene上に配置した時点でのy軸アングル） 
    private bool isCause = false;   //ドアが閉じているか？
    private float timer = 0.0f;     //タイマー

    void Awake()
    {
        //所持コンポーネントの取得
        tr = GetComponent<Transform>();
    }

	void Start () {
        //親を取得
        goal = transform.parent.gameObject;
        //開始時のアングルを取得
        startAngle = transform.eulerAngles.y;
	}

	void Update () {
        //ゴールしているなら
        if (goal.GetComponent<Goal>().isGoal)
        {
            //タイマー増加
            timer += Time.deltaTime;
            //アングルを変更
            //※注意：180度を超えるアングル変更は思った通りの方向に回転してくれないので控えてください。そんなに回るドアも珍しいとは思います。
            transform.eulerAngles =
                Vector3.Lerp(new Vector3(0, startAngle, 0), new Vector3(0, endAngle, 0), timer / endTime);
        }
	}
}
