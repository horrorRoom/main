/*********************************************************/
/// <summary>
///ドアを開くプログラム
/// </summary>
/*********************************************************/
using UnityEngine;
using System.Collections;

public class Door : DoorBase
{
    [SerializeField]
    private Transform mReferencePoint;
    [SerializeField]
    private float mRotateAngle;

    private float mAngle = 0.0f;

    //押し出し用当たり判定
    private GameObject mExtrusion;

    //ドアを開ける音
    private AudioSource DoorSound;

    //閉まる音
    [SerializeField]
    private AudioClip cloose;
    //鍵がかかってる音
    [SerializeField]
    private AudioClip lock_door;
    //ドアが開く音
    [SerializeField]
    private AudioClip open_door;

    //ループする位置
    [SerializeField]
    GameObject LoopPoint;

    /*********************************************************/
    // Use this for initialization
    /*********************************************************/
    void Start()
    {
        DoorSound = gameObject.GetComponent<AudioSource>();

        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        mGoal = GameObject.FindGameObjectWithTag("Goal").GetComponent<Goal>();

        mAngle = 0.0f;
        mExtrusion = gameObject.transform.Find("Extrusion").gameObject;
        mExtrusion.SetActive(false);
    }

    /*********************************************************/
    // Update is called once per frame
    /*********************************************************/
    void Update()
    {
        DoorCloss();

        if (mIsOpen)
        {
            mExtrusion.SetActive(true);
            mAngle = Mathf.Min(mAngle + Mathf.Abs(mSpeed * Time.deltaTime), Mathf.Abs(mRotateAngle));
            if (mAngle < Mathf.Abs(mRotateAngle))
            {
                transform.RotateAround(mReferencePoint.position, new Vector3(0.0f, 1.0f, 0.0f), mSpeed * Time.deltaTime);
            }
        }
        else
        {
            mExtrusion.SetActive(false);
            mAngle = Mathf.Max(mAngle - Mathf.Abs(mSpeed * Time.deltaTime), 0.0f);
            if (mAngle > 0.0f)
            {
                transform.RotateAround(mReferencePoint.position, new Vector3(0.0f, 1.0f, 0.0f), -mSpeed * Time.deltaTime);
            }
        }
    }

    /*********************************************************/
    /// <summary>
    /// ドアを開けた時のアクション
    /// </summary>
    /*********************************************************/
    public override void Action()
    {
        //SE
        if (!gameObject.GetComponent<Door>().enabled) DoorSound.clip = lock_door;
        else DoorSound.clip = open_door;

        mIsOpen = true;

        DoorSound.Play();
        if (mExtrusion != null) mExtrusion.SetActive(true);
        //ゴール演出
        if (isGoalDoor)
        {
            //フェードアウトさせる
            mGoal.GoalDoorOpen();
            return;
        }

        //ループ用アクション
        if (LoopPoint != null)
        {
            LoopAction();
            return;
        }

        if (!(mIsOpenOnly && mIsOpen))　 mIsOpen = !mIsOpen;
    }

    /*********************************************************/
    /// <summary>
    /// ループアクション
    /// </summary>
    /*********************************************************/
    void LoopAction()
    {
        Vector3 heading = transform.position - mPlayer.position;
        //距離
        float distance = heading.magnitude;
        //方向
        Vector3 direction = heading / distance;
        //プレイヤーを移動
        mPlayer.position = LoopPoint.transform.position - direction * 2;
        //ドアを開けた状態にする
        LoopPoint.GetComponent<Door>().mIsOpen = true;
    }

    /*********************************************************/
    /// <summary>
    /// ドアを閉じる
    /// </summary>
    /*********************************************************/
    void DoorCloss()
    {
        if (Vector3.Distance(transform.position, mPlayer.position) > 8.0f)
        {
            mIsOpen = false;
            mExtrusion.SetActive(false);
        }
    }
}
