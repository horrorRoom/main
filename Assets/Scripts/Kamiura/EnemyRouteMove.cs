using UnityEngine;
using System.Collections;

//必須コンポーネント
[RequireComponent(typeof(Kamiura))]

/// <summary>
/// startに指定した地点とgoalに指定した地点を往復する動きをするクラス
/// </summary>
public class EnemyRouteMove : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField]
    private GameObject start;   //移動の開始地点
    [SerializeField]
    private GameObject goal;    //移動の引き返し地点
    [SerializeField]
    private float speed;        //移動速度
    [SerializeField]
    private bool isGoal = false;//折り返して開始地点へ戻るか？

    /*==外部参照変数==*/
    public bool isMove = true;  //移動を行うか？falseにすることで停止できます。

	void Awake() {
	    //所持コンポーネント取得
        tr = GetComponent<Transform>();
	}
	
	void Update () {
        //プレイヤーを発見しているなら移動しない
        if (GetComponent<Kamiura>().GetIsPlayerLook()) return;
        //移動しない設定だったら移動しない
        if (!isMove) return;

        //移動
        Vector3 translation = Vector3.forward * speed * Time.deltaTime;
        tr.Translate(translation);

        //引き返すかどうかの判定
        Vector3 vec;
        if (isGoal)
        {
            //進行方向を向く
            tr.LookAt(start.transform.position);
            //距離によって判定
            vec = start.transform.position - tr.position;
            if (Vector3.Magnitude(vec) <= 0.5f) isGoal = false;
        }
        else
        {            
            tr.LookAt(goal.transform.position);
            vec = goal.transform.position - tr.position;
            if (Vector3.Magnitude(vec) <= 0.5f) isGoal = true;
        }
	}
}
