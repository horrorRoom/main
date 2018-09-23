/*********************************************************/
//アイテムを確認するシステム
/*********************************************************/
using UnityEngine;
using System.Collections;

public class HorrorItemDraw : MonoBehaviour {

    bool isPlay = false;

    [SerializeField]
    float coolTime = 0;
    [SerializeField]
    float time = 0;

    [SerializeField]
    private GameObject Draw;

    [SerializeField]
    private AudioSource sound;

    /*********************************************************/
    // Use this for initialization
    /*********************************************************/
    void Start () {
	
	}

    /*********************************************************/
    // Update is called once per frame
    /*********************************************************/
    void Update() {
        //描画までの時間
        if (time > 0.0f) time -= 1.0f * Time.deltaTime;
        
        //次描画までのクールタイム
        if (coolTime > 0) coolTime-=1.0f*Time.deltaTime;

        if (!isPlay) return;

        if (Input.GetMouseButtonDown(0) && time<=0.0f)
        {
            isPlay = false;
            Draw.SetActive(false);
            coolTime = 1.0f;
            //通常モードに
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().playState = (int)State.play;
        }
	}

    /*********************************************************/
    //実行Play
    /*********************************************************/
    public void Play() {
        if (coolTime > 0 || isPlay || time > 0) return;

        isPlay = true;
        Draw.SetActive(true);

        sound.Play();

        //観賞状態に
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().playState = (int)State.admiratiopn;
        time = 0.1f;
    }
}
