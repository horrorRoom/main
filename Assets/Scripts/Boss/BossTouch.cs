using UnityEngine;
using System.Collections;

public class BossTouch : MonoBehaviour
{

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    //当たり判定
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;

        ToGameOver.Apply(collider.transform);
    }
}