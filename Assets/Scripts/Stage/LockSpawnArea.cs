using UnityEngine;
using System.Collections;

public class LockSpawnArea : MonoBehaviour {

	public float mAngle;//<!オブジェクトを出現させる基準角度からの誤差
	public GameObject mMan;//<!出現させるオブジェクト
	public GameObject mAngleObject;//<!オブジェクトを出現させる基準角度
	public float mDistance;//<!オブジェクトを出現させるプレイヤーからの位置

	private bool mIsExit;//<!範囲外に出たか？
	private GameObject mPlayer;//<!プレイヤーオブジェクト
	private bool mIsTrigger;//<!イベントを起こしたか？
	// Use this for initialization
	void Start () 
	{
		mIsExit = false;
		mIsTrigger = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mPlayer != null && mIsExit == false && mIsTrigger == false)
		{
			Vector3 position = mPlayer.transform.position + (mPlayer.transform.forward * mDistance);

            if (Vector3.Angle(mPlayer.transform.forward, mAngleObject.transform.forward) < mAngle)
			{
				GameObject obj = (GameObject)Instantiate(mMan,position,mPlayer.transform.rotation);
				obj.transform.Rotate(0.0f,180.0f,0.0f);
				mIsTrigger = true;
			}
		}
	}
	void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player") 
		{
			mPlayer = coll.gameObject;
		}
	}
	void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Player") 
		{
			mIsExit = true;
		}
	}
}
