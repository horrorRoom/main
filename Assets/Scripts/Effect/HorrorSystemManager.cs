/**************************************/
//なにかを押した時のシステム
/**************************************/
using UnityEngine;
using System.Collections;

public class HorrorSystemManager : MonoBehaviour {

    [SerializeField]
    int system=0;

    [SerializeField]
    GameObject []target;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    public void Play()
    {
        //ボタンを押したらカミウラーを呼ぶ
        if (system == 0) {
            target[0].GetComponent<PointMove>().isMove = true;
            target[1].GetComponent<AudioSource>().Play();
        }
        //鍵を開ける
        if (system == 1) {
            //鍵を削除
            Destroy(target[0]);
            //ドアをあくようにする
            target[1].GetComponent<Door>().enabled = true;
        }
    }
}
