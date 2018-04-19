using UnityEngine;
using System.Collections;

public class TitleSelect : MonoBehaviour {

    public int number=0;

    public GameObject Fade;

    public bool isFadeOut=false;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
        if (Fade.GetComponent<Fade>().isStart) return;

        if (isFadeOut)
        {
            if(!Fade.GetComponent<Fade>().isEnd){
                if (number == 0) Application.LoadLevel("Stage1");
                if (number == 1) Application.Quit();
            }
            return;
        }

        //決定
        if (Input.GetKey(KeyCode.Space))
        {
            Fade.GetComponent<Fade>().isEnd = true;
            isFadeOut = true;
        }
        
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            number = 0;
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            number = 1;
        }
	}
}
