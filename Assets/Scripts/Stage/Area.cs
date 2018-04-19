using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour {

    public int number=0;

	// Use this for initialization
	void Start () {
        GameObject []AreaList = GameObject.FindGameObjectsWithTag("Room");
        for (int i = 0; i < AreaList.Length; i++)
        {
            if (AreaList[i].GetComponent<Area>().number == number)
            {
                number = AreaList[i].GetComponent<Area>().number++;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {}
}
