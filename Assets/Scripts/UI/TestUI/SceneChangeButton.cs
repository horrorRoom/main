using UnityEngine;
using System.Collections;

public class SceneChangeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void OnClick( string name )
    {
        Application.LoadLevel(name);
    }
}
