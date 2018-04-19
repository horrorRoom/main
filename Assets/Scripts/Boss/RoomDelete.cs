/**************************************************************/
//通ってきた部屋を消す
//大森　恵太郎
/**************************************************************/
using UnityEngine;
using System.Collections;

public class RoomDelete : MonoBehaviour {

    //消す場所
    [SerializeField]
    GameObject[] RoomList;

    //何番目の場所
    [SerializeField]
    int number=0;
    //プレイヤーがいるか調べる
    [SerializeField]
    bool isRoomSearch=false;

    //追いつめられたポイント
    public GameObject[] DeadSpace;

    bool isDead=false;
    //触れているかどうか
    bool isHit = true;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {}

    //部屋を消す
    public void DeadRoom()
    {
        if (isDead) return;
        isDead = true;
        if (RoomList.Length<=0) return;
        for (int i = 0; i < RoomList.Length; i++){ Destroy(RoomList[i]); }
    }

    //追い込まれるポイントにプレイヤーがいるか調べる
    public bool DeadSpaceSearch()
    {
        if (DeadSpace == null) return false;

        bool isHit = false;
        for(int i = 0; i < DeadSpace.Length; i++)
        {
            if (DeadSpace[i].GetComponent<BossPlayerSearch>().isArea()) isHit = true;
        }

        return isHit;
    }

    //部屋にプレイヤーがいるか返す
    public bool isPlayerHitRoom()
    {
        if (!isRoomSearch || !isHit) return false;
        return true;
    }

    //当たり判定
    void OnTriggerEnter(Collider collider)
    {
        //その部屋にプレイヤーがいるか調べる
        if(isRoomSearch && collider.tag=="Player") isHit = true;

        if (collider.tag != "Boss") return;

        //プレイヤー中を追いかけ中なら解除
        collider.GetComponent<BossMove>().RoomPlayerMoveReset();

        //部屋を消す
        DeadRoom();
    }

    //当たり判定
    void OnTriggerExit(Collider collider)
    {
        isHit = false;
    }

    public int getNumber() { return number; }
}
