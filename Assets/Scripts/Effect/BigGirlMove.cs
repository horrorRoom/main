using UnityEngine;
using System.Collections;

public class BigGirlMove : MonoBehaviour {

    /*==外部設定変数==*/
    [SerializeField]
    private GameObject Girl;

    //移動するかどうか
    bool isMove = false;

    //移動する時間
    float time = 0;

    //移動しだす時間
    [SerializeField]
    float MaxTime = 3;

    //移動スピード
    [SerializeField]
    float MaxSpeed = 8.0f;

    float speed = 0;
    float speedTime = 0;

    // Use this for initialization
    void Start () {}
	
	// Update is called once per frame
	void Update () {
        if (!isMove) return;

        //スピードが上がっていく
        speedTime += 1.0f * Time.deltaTime;
        if (speedTime >= 0.5f)
        {
            speed += 1.0f;
            speedTime = 0;
        }
        if (speed > MaxSpeed) speed = MaxSpeed;

        //移動
        time += 1.0f * Time.deltaTime;
        if (time > MaxTime)
        {
            Girl.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.tag != "Player") return;
        Girl.active = true;

        isMove = true;
    }

}
