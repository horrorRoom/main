using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kamiura : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;
    private AudioSource audio;

    /*==外部設定変数==*/
    [SerializeField]
    private float speed;             //移動スピード
    [SerializeField]
    private AudioClip attackVoice;   //プレイヤーを追いかけているときに流す効果音
    [SerializeField]
    bool isNoTimeDash = false;      //ノータイムでおいかけてくる


    /*==内部設定変数==*/
    private GameObject player;      //プレイヤーオブジェクト
    private GameObject goal;        //ゴールオブジェクト
    private GameObject playerSearch;//プレイヤーが近くにいるかを探るオブジェクト
    private float time = 0;
    private bool isSound = false;   //サウンドを再生したか？
    private bool isChaseEnd = false;   //プレイヤーを追いかけるのをやめるか？

    void Awake()
    {
        //所持コンポーネントの取得
        tr = GetComponent<Transform>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        //ゲームオブジェクト取得
        player = GameObject.FindGameObjectWithTag("Player");
        goal = GameObject.FindGameObjectWithTag("Goal");
        playerSearch = tr.Find("PlayerSearch").gameObject;
        //再生開始
        audio.Play();
    }

    void Update()
    {
        //問題がある場合はリターンする
        if (Message.ErrorMessage(player, "Kamiura.cs\nplayerオブジェクトがnullです。")) return;
        if (Message.ErrorMessage(goal, "Kamiura.cs\ngoalオブジェクトがnullです。")) return;

        //ゴールしていたら消滅
        if (goal.GetComponent<Goal>().isGoal)
            Destroy(this.gameObject);
        
        //PlayerSearchオブジェクトがプレイヤーを発見したら
        if (!isChaseEnd && GetIsPlayerLook())
        {
            NoiseManager.Instance.MomentNoise_External(0.1f);

            //タイムを増加
            time += Time.deltaTime;
            
            // 1秒のディレイ
            if (time <= 1.0f && !isNoTimeDash) return;
            //効果音を再生
            if (!isSound) Sound();
            //移動
            Move();
        }
    }


    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        //プレイヤーのいる地点を前方に据える
        tr.LookAt(new Vector3(player.transform.position.x, tr.position.y, player.transform.position.z));
        //前方に向かって移動速度分の移動をする
        tr.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision coll)
    {
        //プレイヤーとヒットした場合、ゲームオーバー画面へ移行
        if(coll.gameObject.tag == "Player")
        {
            //Application.LoadLevel("GameOver");
            if (!isChaseEnd)
            {
                isChaseEnd = true;
                GameObject.Find("GameOverProcess").GetComponent<GameOverProcess>().ProcessStart(this.gameObject);
            }
        }
    }

    //サウンド
    void Sound()
    {
        audio.clip = attackVoice;
        audio.mute = false;
        audio.Play();
        isSound = true;
    }

    /// <summary>
    /// プレイヤーを発見したか？
    /// </summary>
    public bool GetIsPlayerLook()
    {
        return playerSearch.GetComponent<PlayerSearch>().isPlayerLook;
    }
}
