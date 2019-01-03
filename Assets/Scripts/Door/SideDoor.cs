/*********************************************************/
//横開きのドア
/*********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideDoor : DoorBase
{
    //ドア
    [SerializeField]
    Transform mDoor;

    //オートの処理があった場合
    [SerializeField]
    private DoorAutoClause doorAutoClause;

    const float mOpenPositionX = 1.75f;

    /*********************************************************/
    // Use this for initialization
    /*********************************************************/
    void Start ()
    {
        doorAutoClause = GetComponent<DoorAutoClause>();
        mPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        mGoal = GameObject.FindGameObjectWithTag("Goal").GetComponent<Goal>();
    }

    /*********************************************************/
    // Update is called once per frame
    /*********************************************************/
    void Update()
    {
        if (mIsOpen)
        {
            mDoor.localPosition += new Vector3(mSpeed, 0,0)*Time.deltaTime;
            if (mDoor.localPosition.x >= mOpenPositionX)
            {
                mDoor.localPosition = new Vector3(mOpenPositionX, 0, 0);
            }
        }
        else
        {
            mDoor.localPosition -= new Vector3(mSpeed, 0, 0) * Time.deltaTime;
            if (mDoor.localPosition.x <= 0)
            {
                mDoor.localPosition = new Vector3(0,0,0);
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
        if (!this.gameObject.GetComponent<SideDoor>().enabled)
        {
            SoundManager.GetInstance().SEPlay("slideLockDoor");
        }
        else SoundManager.GetInstance().SEPlay("slideOpenDoor");

        //ゴール演出
        if (isGoalDoor)
        {
            //フェードアウトさせる
            mGoal.GoalDoorOpen();
            return;
        }

        if (!(mIsOpenOnly && mIsOpen)) mIsOpen = !mIsOpen;
    }
}
