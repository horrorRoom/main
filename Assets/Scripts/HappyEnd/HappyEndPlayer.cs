using UnityEngine;
using System.Collections;

public class HappyEndPlayer : MonoBehaviour {

    public float startTime;
    public Animation anime;

    // 足音
    public float walkStartTime;
    public AudioSource workAudio;
    bool isWork;

    // ホワイトアウト用
    public Texture texture;
    public Fade fade;
    public float endTime;
    public AudioSource doorOpenAudio;

    public GameObject ui;
  
    float timer;

	// Use this for initialization
	void Start () {
        anime.enabled = false;

        //Debug.Log("a");
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        // カメラ移動アニメーション
        if ( timer > startTime && !anime.enabled )
        {
            //Debug.Log("a");
            anime.enabled = true;
            anime.Play();
        }

        // 歩き出すアニメーション
        if (timer > walkStartTime && !isWork)
        {
            isWork = true;
            workAudio.Play();
        }

        // ドアを開ける（音だけ）
        // 同時にホワイトアウト
        if ( timer > endTime && !fade.IsEnd() )
        {
            //fade.GetComponent<GUITexture>().texture = texture;
            fade.FadeOut();
            doorOpenAudio.Play();
        }
        // テスト用UI表示
        if (timer > endTime + 3.0f )
        {
            ui.SetActive(true);
            Destroy(this);
        }

    }
}
