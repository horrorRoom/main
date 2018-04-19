using UnityEngine;
using System.Collections;

public class AnlockKey : MonoBehaviour
{

    [SerializeField]
    private GameObject Door;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Player") return;

        Destroy(this.gameObject);

        Door.GetComponent<Door>().enabled = true;
    }

}