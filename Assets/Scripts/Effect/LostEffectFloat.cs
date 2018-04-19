/*********************************************************/
//泥が上がっていく演出
//大森　恵太郎
/*********************************************************/
using UnityEngine;
using System.Collections;

public class LostEffectFloat : MonoBehaviour {

    float time = 0;

    [SerializeField]
    float height;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        time += 1.0f * Time.deltaTime;

        if (time > 0.8f)
        {
            transform.position += new Vector3(0,1.0f,0);
            time = 0;
        }

        if (transform.position.y >= height) transform.position = new Vector3(transform.position.x,height,transform.position.z);
	}
}
