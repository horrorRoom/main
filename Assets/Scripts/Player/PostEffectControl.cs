/**====================================================================*/
/**
 * カメラにかけるポストエフェクトを操作する。
 * 作成者：守屋　作成日：15/11/21*/
/**====================================================================*/


using UnityEngine;
using System.Collections;
//イメージエフェクトを使用
using UnityStandardAssets.ImageEffects;

/// <summary>
/// メインカメラの持つイメージエフェクトを利用しやすくするクラスです。
/// </summary>
public class PostEffectControl : MonoBehaviour
{
    /*==外部設定変数==*/
    [SerializeField]
    private float vortexTime;           //渦巻きエフェクトを行う時間
    [SerializeField]
    private AnimationCurve vortexCurve; //渦巻きのカーブ
    [SerializeField]
    private float twirlTime;            //ゆがみエフェクトを行う時間
    [SerializeField]
    private AnimationCurve twirlCurve;  //ゆがみのカーブ

    /*==内部設定変数==*/
    private GameObject camera;  //メインカメラ

    /*==外部参照変数==*/

    /*==================*/
    /* 更新前初期化   */
    /*==================*/
	void Start()
    {
	    //ゲームオブジェクト取得
        camera = GameObject.FindGameObjectWithTag("MainCamera");
	}

    /*==================*/
    /* 更新   */
    /*==================*/
	void Update()
    {

	}

    /// <summary>
    /// ノイズを開始または終了します
    /// </summary>
    public void Noise()
    {
        NoiseAndGrain noise = camera.GetComponent<NoiseAndGrain>();
        noise.enabled = !noise.enabled;
    }

    /// <summary>
    /// 渦巻きエフェクトを開始します
    /// 短時間の間に連続して呼び出すと、挙動がおかしくなるので注意してください
    /// </summary>
    public void StartVortex()
    {
        StartCoroutine(VortexCoroutine());
    }
    
    /// <summary>
    /// 渦巻きエフェクトのコルーチン
    /// </summary>
    IEnumerator VortexCoroutine()
    {
        //カメラが持つ、Vortexコンポーネントを取得
        Vortex vortex = camera.GetComponent<Vortex>();
        //Vortexコンポーネントをアクティブにする
        vortex.enabled = true;

        //線形補間を利用し、アングルにアニメーションカーブを適用する
        for (float i = 0f; i < vortexTime; i += Time.deltaTime)
        {
            float rate = vortexCurve.Evaluate(i / vortexTime);//アニメーションカーブから、現在の進捗を取得
            vortex.angle = Mathf.Lerp(-720.0f, 720.0f, rate);//変化幅を変更したい場合はminとmaxを変更
            yield return null;
        }

        //Vortexコンポーネントを非アクティブにする
        vortex.enabled = false;
    }

    /// <summary>
    /// ゆがみエフェクトを開始します
    /// 短時間の間に連続して呼び出すと、挙動がおかしくなるので注意してください
    /// </summary>
    public void StartTwirl()
    {
        StartCoroutine(TwirlCoroutine());
    }

    /// <summary>
    /// ゆがみエフェクトのコルーチン
    /// </summary>
    IEnumerator TwirlCoroutine()
    {
        //カメラが持つ、Twirlコンポーネントを取得
        Twirl twirl = camera.GetComponent<Twirl>();
        //Vortexコンポーネントをアクティブにする
        twirl.enabled = true;

        //線形補間を利用し、アングルにアニメーションカーブを適用する
        for (float i = 0f; i < twirlTime; i += Time.deltaTime)
        {
            float rate = twirlCurve.Evaluate(i / twirlTime);//アニメーションカーブから、現在の進捗を取得
            twirl.angle = Mathf.Lerp(0.0f, 360.0f, rate);//変化幅を変更したい場合はminとmaxを変更
            yield return null;
        }

        //Vortexコンポーネントを非アクティブにする
        twirl.enabled = false;
    }
}
