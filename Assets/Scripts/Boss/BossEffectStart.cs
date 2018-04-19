/**************************************************************/
//ボス登場演出
//大森　恵太郎
/**************************************************************/
using UnityEngine;
using System.Collections;

public class BossEffectStart : MonoBehaviour
{
    [SerializeField]
    GameObject kami;
    [SerializeField]
    GameObject girl;
    [SerializeField]
    GameObject smoke;
    [SerializeField]
    GameObject lastWordEffect;

    //時間
    float time = 0;
    //演出状態
    int state = 0;

    //演出をスタートするかどうか
    bool isStart = false;

    Vector3 startPoint;

    // Use this for initialization
    void Start(){
        startPoint = girl.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart) return;

        if (state == 0) { KamiDraw(); }
        if (state == 1) { GirlDraw(); }
        if (state == 2) { GirlFly(); }
        if (state == 3) { BossMoveStart(); }
    }

    //当たり判定
    void OnTriggerEnter(Collider collider)
    {
        isStart = true;
    }

    //スーツ男を出現
    void KamiDraw()
    {
        Vector3 position = /*new Vector3(0, 6.5f, 0) +*/ GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 newRotation = Quaternion.LookRotation(kami.transform.position - position).eulerAngles;
        GameObject.FindGameObjectWithTag("Player").transform.rotation = Quaternion.Slerp(GameObject.FindGameObjectWithTag("Player").transform.rotation, Quaternion.Euler(newRotation), Time.deltaTime);

        //プレイヤーの状態 動けなくさせる
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().PlayerStateEffect();

        //出現演出
        if(time==0) Instantiate(smoke,kami.transform.position + new Vector3(0,1.0f,0),Quaternion.Euler(0,0,0));

        if (time >= 0.2f) kami.SetActive(true);

        time += 1.0f * Time.deltaTime;
        if (time > 3.0f)
        {
            time = 0;
            state++;
        }
    }

    //少女現る
    void GirlDraw()
    {
        //出現演出
        if (time == 0) Instantiate(smoke, girl.transform.position + new Vector3(0, 1.0f, 0), Quaternion.Euler(0, 0, 0));

        if (time >= 0.2f) girl.SetActive(true);

        time += 1.0f * Time.deltaTime;
        if (time > 1.0f)
        {
            time = 0;
            state++;
        }
    }

    //少女が浮かびだす
    void GirlFly()
    {
        girl.transform.position = Vector3.Lerp(startPoint, startPoint + new Vector3(0, 1.8f, 0), time / 3);

        //ステージに液体を出す
        if (time == 0)
        {
            Instantiate(lastWordEffect, new Vector3(-25.43f, -10.2f, -174.1f), Quaternion.Euler(0, 0, 0));
            Instantiate(lastWordEffect, new Vector3(-23.48f, -10.2f, -175.5f), Quaternion.Euler(0, 0, 0));
            Instantiate(lastWordEffect, new Vector3(-30.36f, -10.2f, -175.8f), Quaternion.Euler(0, 0, 0));
        }

        time += 1.0f * Time.deltaTime;
        if (time > 5.0f)
        {
            time = 0;
            state++;
        }
    }

    //ボスが動き出す
    void BossMoveStart()
    {
        time += 1.0f * Time.deltaTime;
        if (time > 1.0f)
        {
            time = 0;
            kami.GetComponent<BossMove>().isStart = true;
            //プレイヤーの状態
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().PlayerStatePlay();
            state++;
            //エフェクトスタート
            GameObject[] EffectList = GameObject.FindGameObjectsWithTag("LostWorldEffect");
            for (int i = 0; i < EffectList.Length; i++){ EffectList[i].transform.Find("GameObject").gameObject.SetActive(true); }
        }
    }
}