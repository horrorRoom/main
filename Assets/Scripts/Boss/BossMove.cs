/**************************************************************/
//ボスの移動処理
//大森 恵太郎
/**************************************************************/
using UnityEngine;
using System.Collections;

public class BossMove : MonoBehaviour {
    //演出開始
    public bool isStart = false;

    /*==外部設定変数==*/
    [SerializeField]
    float MaxSpeed=1;
    [SerializeField]
    GameObject[] MovePointList;

    /*==所持コンポーネント==*/
    private Transform tr;

    //今のスピード
    float speed = 0.5f;
    float speedUpTime = 0;

    //今の移動場所
    int number = 0;

    //プレイヤーを追いかけ中かどうか
    [SerializeField]
    bool isPlayerSearch = false;
    //部屋の中のプレイヤーを追いかける
    [SerializeField]
    bool isRoomPlayerSearch = false;

    //追いつめられる場所を保存
    GameObject[] DeadSpace;
    int DeadSpaceNumber = 0;

    float position_y = 0;

	public bool LastFlag = false;

    // Use this for initialization
    void Start () {
        //所持コンポーネント取得
        tr = GetComponent<Transform>();
        //座標は固定
        position_y = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        if (!isStart) return;

        //加速をつけていく
        SpeedUp();

        //プレイヤーとの距離を調べる
        PlayerSearch();
        //ラスト一個にむかう最中なら
        if (number >= MovePointList.Length-1) {
            isPlayerSearch = false;
            isRoomPlayerSearch = false;
        }
		if (number == MovePointList.Length) LastFlag = true;
        if (number >= MovePointList.Length) return;

        //ルート通り走る
        if (!isPlayerSearch && !isRoomPlayerSearch)
        {
            //追いつめる場所へ移動
            if (DeadSpace != null) DeadSpaceMove();
            else RootMove();
        }
        //プレイヤーを追跡する
        else if (isPlayerSearch || isRoomPlayerSearch) PlayerMove();

        //座標は固定
        transform.position = new Vector3(transform.position.x,position_y, transform.position.z);
    }

    //加速度をつける
    void SpeedUp() {
        speedUpTime += 1.0f * Time.deltaTime;
        if (1.0f <= speedUpTime) {
            speed += 1.0f;
            speedUpTime = 0;
        }
        //最大速度
        if (MaxSpeed <= speed) speed =MaxSpeed;
    }

    //プレイヤーとの距離を調べる
    void PlayerSearch()
    {
        if (Vector3.Magnitude(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position) <= 4.0f){
            //プレイヤー追跡からプレイヤー見失ったら
            if (isPlayerSearch) { PointSearch(); }
            isPlayerSearch = true;
        }
        else{
            isPlayerSearch = false;
        }
    }

    //指定されたルート通り走る
    void RootMove()
    {
        //移動
        Vector3 translation = Vector3.forward * speed * Time.deltaTime;
        tr.Translate(translation);

        //引き返すかどうかの判定
        Vector3 vec;
        //進行方向を向く
        tr.LookAt(MovePointList[number].transform.position);
        //距離によって判定
        vec = MovePointList[number].transform.position - tr.position;
        if (Vector3.Magnitude(vec) <= 0.5f)
        {
            //今まで通ってきた道をすべて削除
            for (int i = 0; i < number; i++)
            {
                MovePointList[number].GetComponent<RoomDelete>().DeadRoom();
            }
            //追いつめられた場所にプレイヤーがいないか調べる
            if (MovePointList[number].GetComponent<RoomDelete>().DeadSpaceSearch()) {
                DeadSpace=MovePointList[number].GetComponent<RoomDelete>().DeadSpace;
                DeadSpaceNumber = 0;
            }
            //部屋の中にプレイヤーがいないか調べる
            isRoomPlayerSearch = MovePointList[number].GetComponent<RoomDelete>().isPlayerHitRoom();

            //次のポイント
            number++;
        }
    }

    //プレイヤーを追跡する
    void PlayerMove()
    {
        //移動
        Vector3 translation = Vector3.forward * speed * Time.deltaTime;
        tr.Translate(translation);

        //引き返すかどうかの判定
        Vector3 vec;
        //進行方向を向く
        tr.LookAt(GameObject.FindGameObjectWithTag("Player").transform.position);
        //距離によって判定
        vec = GameObject.FindGameObjectWithTag("Player").transform.position - tr.position;
    }

    //追いつめる場所へ移動する
    void DeadSpaceMove()
    {
        //移動
        Vector3 translation = Vector3.forward * speed * Time.deltaTime;
        tr.Translate(translation);

        //引き返すかどうかの判定
        Vector3 vec;
        //進行方向を向く
        tr.LookAt(DeadSpace[DeadSpaceNumber].transform.position);
        //距離によって判定
        vec = DeadSpace[DeadSpaceNumber].transform.position - tr.position;
        if (Vector3.Magnitude(vec) <= 0.5f)
        {
            //次のポイント
            DeadSpaceNumber++;
            if (number >= DeadSpace.Length) { DeadSpaceNumber = 0; }
        }
    }
    
    //部屋のプレイヤーを追いかけてる場合、リセットする
    public void RoomPlayerMoveReset()
    {
        if (!isRoomPlayerSearch) return;

        isRoomPlayerSearch = false;
    }

    //近くのポイントを探す
    void PointSearch()
    {
        int Num = MovePointList[0].GetComponent<RoomDelete>().getNumber();
        float length = Vector3.Magnitude(transform.position - MovePointList[0].transform.position);

        for (int i = 1; i < MovePointList.Length; i++)
        {
            if (length > Vector3.Magnitude(transform.position - MovePointList[i].transform.position))
            {
                length = Vector3.Magnitude(transform.position - MovePointList[i].transform.position);
                Num = MovePointList[i].GetComponent<RoomDelete>().getNumber();
            }
        }

        number = Num;
    }

	public bool ReturnLastFlag(){
		return LastFlag;
	}
}
