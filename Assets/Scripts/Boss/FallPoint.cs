using UnityEngine;
using System.Collections;

public class FallPoint : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y > GameObject.FindGameObjectWithTag("Player").transform.position.y)
        {
            ToGameOver.Apply(GameObject.FindGameObjectWithTag("Player").transform);
        }
	}
}
