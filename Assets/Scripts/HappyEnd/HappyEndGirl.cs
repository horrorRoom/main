using UnityEngine;
using System.Collections;

public class HappyEndGirl : MonoBehaviour {

    // 出現する時間
    public float appendTime;

    // 消失する時間
    public float vanishTime;

    // 対象Object
    public GameObject girl;

    private float timer;


    // Use this for initialization
    void Start () {
        girl.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(girl == null)
        {
            return;
        }
        // 一定時間たったら
        if (appendTime < timer )
        {
            // 出現
            girl.SetActive(true);
            // さらにたったら消す
            if (vanishTime < timer)
            {
                girl.SetActive(false);
                Destroy(this);
            }
        }
	}
}
