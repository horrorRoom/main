using UnityEngine;
using System.Collections;

public class BossPlayerSearch : MonoBehaviour {

    bool isHit = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {}

    //範囲に入っているか返す
    public bool isArea() { return isHit; }

    //当たり判定
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag!="Player") return;
        isHit = true;
    }

    //当たり判定
    void OnTriggerExit(Collider collider)
    {
        isHit = false;
    }
}
