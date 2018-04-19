using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpManager : MonoBehaviour {
    List<Transform> HelpList = new List<Transform>();
    /*[HideInInspector]*/ public bool goal = false;
    [SerializeField] private float delay = 0f;
    [SerializeField] private float timeSpeed = 0.5f;
    void Start () {
        Transform children = GetComponentInChildren<Transform>();
        foreach (Transform obj in children)
        {
            HelpList.Add(obj);
        }
    }

	void Update ()
    {
        if (goal == true) {
            delay -= Time.deltaTime * timeSpeed;
            if (delay <= 0f)
            {
                for (int i = 0; i < HelpList.Count; i++)
                {
                    HelpList[i].gameObject.GetComponent<Help_ON>().switch_on = true;
                }
            }
            
        }
	}
}
