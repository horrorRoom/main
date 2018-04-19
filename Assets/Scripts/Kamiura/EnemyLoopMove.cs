using UnityEngine;
using System.Collections;

//必須コンポーネント
[RequireComponent(typeof(Kamiura))]

public class EnemyLoopMove : MonoBehaviour {

    /*==所持コンポーネント==*/
    private Transform tr;

    /*==外部設定変数==*/
    [SerializeField]
    private GameObject[] List;   //移動の開始地点
    [SerializeField]
    private float speed=1;        //移動速度
    [SerializeField]
    private int moveNumber = 1;//現在の移動先

    /*==外部参照変数==*/
    public bool isMove = true;  //移動を行うか？falseにすることで停止できます。

    void Awake()
    {
        //所持コンポーネント取得
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        //プレイヤーを発見しているなら移動しない
        if (GetComponent<Kamiura>().GetIsPlayerLook()) return;
        //移動しない設定だったら移動しない
        if (!isMove) return;

        //移動
        Vector3 translation = Vector3.forward * speed * Time.deltaTime;
        tr.Translate(translation);

        //引き返すかどうかの判定
        Vector3 vec;
        //進行方向を向く
        tr.LookAt(List[moveNumber].transform.position);
        //距離によって判定
        vec = List[moveNumber].transform.position - tr.position;
        if (Vector3.Magnitude(vec) <= 0.5f) {
            moveNumber++;
            if (moveNumber >= List.Length) { moveNumber = 0; }
        }

    }
}
