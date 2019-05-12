using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

/// <summary>
/// フェード
/// </summary>
public class Fade : MonoBehaviour {

    [SerializeField]
    private bool isStart=false;
    [SerializeField]
    private bool isEnd = false;
    [SerializeField]
    /// <summary>フェード中の透明度</summary>
    private float fadeAlpha = 1.0f;
    [SerializeField]
    private Image image;

    private float time=0.0f;

    //フェード終了時アクション
    Action fadeEndAction=null;

    //フェードインでアルファを減らしていく時間
    const float FADE_IN_TIME= 0.2f;
    //フェードインでアルファを減らす量
    const float FADE_IN_ALPHA = 0.057f;

    const float FADE_OUT_TIME = 0.5f;

    /// <summary>
    /// 更新
    /// </summary>
    void Update () {
        if (isStart)
        {
            time += 1.0f * Time.deltaTime;
            if (time >= FADE_IN_TIME)
            {
                this.fadeAlpha -= FADE_IN_ALPHA;
                time = 0.0f;
            }
            if (this.fadeAlpha <= 0.0f)
            {
                this.fadeAlpha = 0.0f;
                isStart = false;
                if (fadeEndAction != null) fadeEndAction();
            }
        }

        if (isEnd)
        {
            time += 1.0f * Time.deltaTime;
            if (time >= FADE_OUT_TIME)
            {
                this.fadeAlpha += 0.1f;
                time = 0.0f;
            }
            if (this.fadeAlpha >= 1.0f)
            {
                fadeAlpha = 1.0f;
                isEnd = false;
                if(fadeEndAction != null) fadeEndAction();
            }
        }

        image.color = new Color(image.color.r, image.color.b, image.color.g, fadeAlpha);
	}

    /// <summary>
    /// フェードイン中か返す
    /// </summary>
    /// <returns></returns>
    public bool IsStart() { return isStart; }

    /// <summary>
    /// フェードアウト中か返す
    /// </summary>
    /// <returns></returns>
    public bool IsEnd() { return isEnd; }

    /// <summary>
    /// フェードイン
    /// </summary>
    public void FadeIn(Action action = null) {
        isStart =true;
        fadeEndAction = action;
    }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public void FadeOut(Action action=null) {
        isEnd = true;
        fadeEndAction = action;
    }
}
