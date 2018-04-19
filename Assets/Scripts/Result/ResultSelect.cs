using UnityEngine;
using System.Collections;

public class ResultSelect : MonoBehaviour {

    public int number = 0;

    GameObject save;

	void Start () {
        save = GameObject.FindGameObjectWithTag("Save");
    }
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            number = 0;
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            number = 1;
        }

        //決定
        if (Input.GetKey(KeyCode.Space))
        {
            Message.ErrorMessage(save, "ResultSelect.cs\nsaveオブジェクトがnulｌです。");
            if (number == 0) Application.LoadLevel(save.GetComponent<Save>().sceneName);
            if (number == 1) Application.LoadLevel("Title");
        }
    }
}
