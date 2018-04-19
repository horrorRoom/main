using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fadePanel : MonoBehaviour {
	public bool isStart=false;
	public bool isEnd = false;

	/// <summary>フェード中の透明度</summary>
	public float fadeAlpha = 1.0f;

	private float time=0.0f;

	void Update () {
		if (Input.GetKeyDown (KeyCode.Return))	isStart = true;

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

		GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, fadeAlpha);
	}
}