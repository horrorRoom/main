using UnityEngine;
using System.Collections;

//必須コンポーネント
[RequireComponent(typeof(Kamiura))]

/// <summary>
/// pointに次々移動をするクラス
/// </summary>
public class PointMove : MonoBehaviour
{
	/*==所持コンポーネント==*/
	private Transform tr;
	private int count;
	
	/*==外部設定変数==*/
	[SerializeField]
	private GameObject[] points;	//移動地点
	[SerializeField]
	private float speed;        //移動速度
	[SerializeField]
	private bool isGoal = false;//折り返して開始地点へ戻るか？
	
	/*==外部参照変数==*/
	public bool isMove = true;  //移動を行うか？falseにすることで停止できます。
	
	void Awake() {
		//所持コンポーネント取得
		tr = GetComponent<Transform>();
		count = 0;
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
		//移動地点がまだある場合
		if(count < points.Length)
		{
			//進行方向を向く
			tr.LookAt(points[count].transform.position);
			//距離によって判定
			vec = points[count].transform.position - tr.position;
			if (Vector3.Magnitude(vec) <= 0.5f) count++;
			return;
		}
		//移動地点がない場合、移動をやめる
		isMove = false;

	}
}
