using UnityEngine;
using System.Collections;

public class TimeOpenDoor : MonoBehaviour {
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private float stopTime = 0;
    [SerializeField]
    private float reloadTime=0;

	// Use this for initialization
	void Start () {
        position = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update() {
        //フェードアウトが完了していなかったら何もしない
        if (GameObject.FindGameObjectWithTag("Fade").GetComponent<Fade>().isStart) return;

        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        if (reloadTime > 0.0f) { reloadTime -= 1.0f * Time.deltaTime; }

        if (Vector3.Distance(position, playerPosition) < 1.0f && reloadTime <= 0) { stopTime += 1.0f * Time.deltaTime; }
        else if(reloadTime <= 0.0f){
            reloadTime = 1.0f;
            position = playerPosition;
        }


        if (stopTime > 10.0f)
        {
            gameObject.GetComponent<Door>().enabled = true;
        }

    }
}
