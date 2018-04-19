using UnityEngine;
using System.Collections;

public class SceneName : MonoBehaviour {

    public string name;

	void Start () {
        GameObject save;
        save = GameObject.FindGameObjectWithTag("Save");
        save.GetComponent<Save>().sceneName = name;
	}
	
	void Update () {}
}
