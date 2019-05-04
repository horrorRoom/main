using UnityEngine;
using UnityEngine.UI;
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

	void Update () {
        if (isStart)
        {
            time += 1.0f * Time.deltaTime;
            if (time >= 0.2f)
            {
                this.fadeAlpha -= 0.057f;
                time = 0.0f;
            }
            if (this.fadeAlpha <= 0.0f)
            {
                this.fadeAlpha = 0.0f;
                isStart = false;
            }
        }

        if (isEnd)
        {
            time += 1.0f * Time.deltaTime;
            if (time >= 0.5f)
            {
                this.fadeAlpha += 0.1f;
                time = 0.0f;
            }
            if (this.fadeAlpha >= 1.0f)
            {
                fadeAlpha = 1.0f;
                isEnd = false;
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
    public void FadeIn() { isStart=true; }

    /// <summary>
    /// フェードアウト
    /// </summary>
    public void FadeOut() { isEnd = true; }
}
