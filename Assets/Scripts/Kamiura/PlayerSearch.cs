using UnityEngine;
using System.Collections;

/// <summary>
/// Kamiuraがプレイヤーを探す際に利用
/// </summary>
public class PlayerSearch : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;
    
    /*==内部設定変数==*/
    private GameObject player;  //プレイヤーオブジェクト
    [SerializeField]
    private bool isPlayerHit;   //判定球がプレイヤーと接触しているか？※初期化処理でfalseになります

    /*==外部参照変数==*/
    public bool isPlayerLook;   //プレイヤーを発見したか？※初期化処理でfalseになります


    //生成前初期化
    void Awake()
    {
        //所持コンポーネントの取得
        tr = GetComponent<Transform>();
    }

    //更新前初期化
    void Start()
    {
        //ゲームオブジェクト取得
        player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーは見つかっていない
        isPlayerHit = false;
        isPlayerLook = false;
    }

    //更新
	void Update()
	{
		if (Message.ErrorMessage(player,"PlayerSearch.cs\nplayerオブジェクトがnullです。")) return;
        //判定球内にプレイヤーが居るか？
        if (isPlayerHit)
        {
            //プレイヤーとの間に何もないか
            float distance = Vector3.Distance(player.transform.position, transform.position);
            Vector3 direction = (player.transform.position - transform.position).normalized;
            int layer = 1 << LayerMask.NameToLayer(player.tag);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(transform.position, direction, out hit))
            {
                if(hit.collider.gameObject.tag == player.tag)
                //プレイヤーを発見
                isPlayerLook = true;
            }
        }
	}

    void OnTriggerEnter(Collider coll)
    {
        //プレイヤーにヒットしたら
        if (coll.gameObject.tag == "Player")
        {
            //ヒットフラグをtrue
            isPlayerHit = true;
        }
    }
    void OnTriigerExit(Collider coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //ヒットフラグをfalse
            isPlayerHit = false;
        }
    }

    //同じ場所にいるかどうか
    bool IsHitArea()
    {
        return (tr.parent.Find("HitAreaCheck").GetComponent<HitAreaCheck>().number == player.GetComponent<HitRoom>().number);
    }
}
