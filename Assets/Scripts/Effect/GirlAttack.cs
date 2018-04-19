using UnityEngine;
using System.Collections;

public class GirlAttack : MonoBehaviour {

    [SerializeField]
    private GameObject girl;

    private bool isMove = false;
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() {
        if (isMove && girl!=null) { girl.transform.position += new Vector3(7.0f,0,0) * Time.deltaTime; }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        isMove = true;
    }

}
