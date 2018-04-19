using UnityEngine;
using System.Collections;

public class HitAreaCheck : MonoBehaviour {

    //今いる場所の番号
    public int number = 0;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    //当たり判定
    void OnTriggerEnter(Collider other)
    {
        //現在いる場所の保存
        if (other.gameObject.tag == "Room")
        {
            number = other.gameObject.GetComponent<Area>().number;
        }
    }
}
