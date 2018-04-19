using UnityEngine;
using System.Collections;

public class StartSceneFade : MonoBehaviour {

    public GameObject Fade;

    float time=0.0f;

    bool isFade=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Fade.GetComponent<Fade>().isStart) return;

        if (isFade && !Fade.GetComponent<Fade>().isEnd)
        {
            Application.LoadLevel("TeamLogo");
        }

        time += 1.0f * Time.deltaTime;

        if (time >= 0.5f)
        {
            isFade = true;
            Fade.GetComponent<Fade>().isEnd = true; ;
        }
	}
}
