using UnityEngine;
using System.Collections;

public class GirlVanish : MonoBehaviour {

    public GameObject smoke;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        Instantiate(smoke,transform.position + new Vector3(0,1.0f,0),Quaternion.Euler(0,0,0));

        Destroy(this.gameObject);
    }

}
