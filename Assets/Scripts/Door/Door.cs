/*********************************************************/
//ドアをひらくプログラム
/*********************************************************/
using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public bool mIsOpenOnly;
    public Transform mReferencePoint;
    public float mRotateAngle;
    public float mSpeed;

    private float mAngle;
    private bool mIsOpen;

    //押し出し用当たり判定
    private GameObject mExtrusion;

    // Use this for initialization

    //ドアを開ける音
    [SerializeField]
    private AudioSource OpenDoor;
    //閉まる音
    [SerializeField]
    private AudioClip cloose;
    //鍵がかかってる音
    [SerializeField]
    private AudioClip lock_door;
    //ドアが開く音
    [SerializeField]
    private AudioClip open_door;
    //開けている最中
    public bool isOpenNow = false;

    //ループする位置
    [SerializeField]
    GameObject LoopPoint;

    //ゴール用のドアかどうか
    [SerializeField]
    bool isGoalDoor=false;

    void Start()
    {
        mAngle = 0.0f;
        mIsOpen = false;
        mExtrusion = gameObject.transform.Find("Extrusion").gameObject;
        mExtrusion.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mExtrusion = gameObject.transform.Find("Extrusion").gameObject;

        DoorCloss();

        if (mIsOpen == true)
        {
            mExtrusion.SetActive(true);
            mAngle = Mathf.Min(mAngle + Mathf.Abs(mSpeed * Time.deltaTime), Mathf.Abs(mRotateAngle));
            if (mAngle < Mathf.Abs(mRotateAngle))
            {
                transform.RotateAround(mReferencePoint.position, new Vector3(0.0f, 1.0f, 0.0f), mSpeed * Time.deltaTime);
            }
            else { isOpenNow = false; }
        }
        else
        {
            mExtrusion.SetActive(false);
            mAngle = Mathf.Max(mAngle - Mathf.Abs(mSpeed * Time.deltaTime), 0.0f);
            if (mAngle > 0.0f)
            {
                transform.RotateAround(mReferencePoint.position, new Vector3(0.0f, 1.0f, 0.0f), -mSpeed * Time.deltaTime);
            }
            else { isOpenNow = false; }
        }
    }

    //ドアを開けた時のアクション
    public void Action()
    {
        //SE
        if (!this.gameObject.GetComponent<Door>().enabled) OpenDoor.GetComponent<AudioSource>().clip = lock_door;
        else {
            OpenDoor.GetComponent<AudioSource>().clip = open_door;
        }

        OpenDoor.Play();
        if (mExtrusion != null) mExtrusion.SetActive(true);
        //ゴール演出
        if (isGoalDoor)
        {
            //フェードアウトさせる
            GameObject.FindGameObjectWithTag("Goal").GetComponent<Goal>().GoalDoorOpen();
            return;
        }

        //ループ用アクション
        if (LoopPoint != null)
        {
            LoopAction();
            return;
        }

        if (!(mIsOpenOnly == true && mIsOpen == true))
        {
            mIsOpen = !mIsOpen;
        }
    }

    //ループアクション
    void LoopAction()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 heading = transform.position - player.transform.position;
        //距離
        float distance = heading.magnitude;
        //方向
        Vector3 direction = heading / distance;
        //プレイヤーを移動
        player.transform.position = LoopPoint.transform.position - direction * 2;
        //ドアを開けた状態にする
        LoopPoint.GetComponent<Door>().mIsOpen = true;
    }

    //ドアを閉じる
    void DoorCloss()
    {
        if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) > 8.0f)
        {
            mIsOpen = false;
            mExtrusion.SetActive(false);
        }
    }
}
