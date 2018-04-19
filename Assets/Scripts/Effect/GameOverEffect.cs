using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class GameOverEffect : MonoBehaviour
{
    public NoiseAndScratches mNoise;//カメラのノイズ
    public GameObject mGirl;//幽霊オブジェクト
    public float mEffectTime;//演出の時間
    public float mIntensity;//演出の時のノイズの強さ

    private float mCurentEffectTime;//現在の演出の時間
    private float mOriginalIntensityMax;//元々のノイズの強さ
    // Use this for initialization
    void Start()
    {
        mCurentEffectTime = 0.0f;
        mOriginalIntensityMax = mNoise.grainIntensityMax;
    }

    // Update is called once per frame
    void Update()
    {
        mCurentEffectTime = Mathf.Min(mCurentEffectTime + Time.deltaTime, mEffectTime);
        if(mCurentEffectTime >= mEffectTime && mGirl.activeSelf == true)
        {
            mNoise.grainIntensityMax = mIntensity;
            mGirl.SetActive(false);
        }
        else if(mGirl.activeSelf == false)
        {
            mNoise.grainIntensityMax = mOriginalIntensityMax;
        }
    }
}
