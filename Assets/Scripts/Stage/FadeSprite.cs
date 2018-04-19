using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーが近づいたらスプライトのアルファを上げる
/// </summary>
public class FadeSprite : MonoBehaviour 
{
	public SpriteRenderer mSprite;//<!アルファを変更するスプライト
	public float mDistance;//<!アルファを変更し始める距離
	public float mFadeTime;//<!アルファを変更し終わるまでの時間

	private GameObject mPlayer;//<!プレイヤーオブジェクト
	private bool mIsTrigger;//<!イベントを起こしたか
	private float mAlpha;//<!透明度
	private float mTime;//<!現在のカウント

	// Use this for initialization
	void Start () 
	{
		mPlayer = GameObject.FindGameObjectWithTag("Player");
		mIsTrigger = false;
		mAlpha = 0.0f;
		mTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () 
	{
		float distance = Vector3.Distance (mPlayer.transform.position, transform.position);
		if (distance <= mDistance) 
		{
			mIsTrigger = true;
		}
		if (mIsTrigger == true)
		{
			mTime += Time.deltaTime;
			mAlpha = Mathf.Lerp(0.0f,1.0f,mTime/mFadeTime);
		}
		Color color = mSprite.color;
		color.a = mAlpha;
		mSprite.color = color;
	}
}
