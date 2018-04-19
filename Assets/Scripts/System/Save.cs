using UnityEngine;
using System.Collections;

public class Save : MonoBehaviour {

    public string sceneName;

    void Start()
    {   
        //すでに生成されていたら消去
        GameObject[] saves = GameObject.FindGameObjectsWithTag("Save");
        if (saves.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

	void Update () {
        DontDestroyOnLoad(gameObject);
	}
}
