using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Help_ON : MonoBehaviour {
    
    [HideInInspector] public bool switch_on = false;
    float time = 0f;
    Color thisColor;
    [SerializeField] private float emageSpeed = 0.5f;

    void Start () {
        thisColor = GetComponent<Renderer>().material.color;
        thisColor.a = 0f;
        GetComponent<Renderer>().material.color = thisColor;
    }
	
	void Update () {
        if (switch_on == true) {
            time += Time.deltaTime * emageSpeed;
            thisColor.a = time;
            GetComponent<Renderer>().material.color = thisColor;
        }
	}
}
