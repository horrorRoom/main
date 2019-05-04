/*********************************************************/
//時間でドアを開ける
/*********************************************************/
using UnityEngine;
using System.Collections;

public class TimeOpenDoor : MonoBehaviour {
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private float mStopTime = 0;
    [SerializeField]
    private float mReloadTime=0;

    Transform mPlayerTransform;
    Fade mFade;

    /*********************************************************/
    // Use this for initialization
    /*********************************************************/
    void Start ()
    {
        mFade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Fade>();
        mPlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        position = mPlayerTransform.position;
    }

    /*********************************************************/
    // Update is called once per frame
    /*********************************************************/
    void Update()
    {
        //フェードアウトが完了していなかったら何もしない
        if (mFade.IsStart()) return;

        if (mReloadTime > 0.0f)  mReloadTime -= 1.0f * Time.deltaTime;

        if (Vector3.Distance(position, mPlayerTransform.position) < 1.0f && mReloadTime <= 0) mStopTime += 1.0f * Time.deltaTime;
        else if(mReloadTime <= 0.0f){
            mReloadTime = 1.0f;
            position = mPlayerTransform.position;
        }


        if (mStopTime > 10.0f) gameObject.GetComponent<Door>().enabled = true;
    }
}
