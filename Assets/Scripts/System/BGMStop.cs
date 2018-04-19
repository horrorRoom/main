using UnityEngine;
using System.Collections;

public class BGMStop : MonoBehaviour {
    /*==所持コンポーネント==*/
    private AudioSource audio;

    /*==内部設定変数==*/
    private GameObject goalArea;
    private GameObject[] kamiuras;

    void Awake()
    {
        //所持コンポーネント取得
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        //オブジェクト取得
        goalArea = GameObject.FindGameObjectWithTag("Goal");
        kamiuras = GameObject.FindGameObjectsWithTag("Kamiura");
    }

	void Update () {
        if (Message.ErrorMessage(goalArea, "BGMStop.cs\ngoalAreaオブジェクトがnullです。")) return;
        
        //ゴールしていたら
        if (goalArea.GetComponent<Goal>().isGoal)
        {
            //BGMのピッチと音量を調整
            audio.pitch = 0.4f;
            audio.volume = 0.2f;
        }

        //kamiurasを巡回
        foreach (GameObject kamiura in kamiuras)
        {
            //kamiuraがプレイヤーを発見していたら
            if(kamiura != null && kamiura.GetComponent<Kamiura>().GetIsPlayerLook())
            {
                //BGMをミュートにする
                audio.mute = true;
            }
        }
	}
}
