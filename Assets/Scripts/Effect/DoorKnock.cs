using UnityEngine;
using System.Collections;

public class DoorKnock : MonoBehaviour {

    bool isHit = false;

    /*==外部設定変数==*/
    [SerializeField]
    private GameObject soundPosition;

    // Use this for initialization
    void Start() {}

    // Update is called once per frame
    void Update() {}

    void OnTriggerEnter(Collider collider) {
        if (isHit || collider.tag!="Player") return;

        soundPosition.GetComponent<AudioSource>().Play();

        isHit = true;
    }
}
