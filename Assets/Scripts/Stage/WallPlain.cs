using UnityEngine;
using System.Collections;

public class WallPlain : MonoBehaviour {

    [SerializeField]
    GameObject Plane;

    float length=0.5001f;

    // Use this for initialization
    void Start () {
        Vector3 position = gameObject.transform.position;
        //左
        GameObject Wall1 = Instantiate(Plane, position + new Vector3(-length, 0, 0), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)) as GameObject;
        Wall1.transform.localScale = new Vector3(transform.localScale.x / 10, transform.localScale.y, transform.localScale.z / 10);
        //右
        GameObject Wall2 = Instantiate(Plane, position + new Vector3(length, 0, 0), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+180)) as GameObject;
        Wall2.transform.localScale = new Vector3(transform.localScale.x / 10, transform.localScale.y, transform.localScale.z / 10);
        //
        //GameObject Wall3 = Instantiate(Plane, position + new Vector3(-length, 0, 0), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z+90)) as GameObject;
        //Wall3.transform.localScale = new Vector3(transform.localScale.x / 10, transform.localScale.y, transform.localScale.z / 10);
        //
        //GameObject Wall4 = Instantiate(Plane, position + new Vector3(length, 0, 0), Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z-90)) as GameObject;
        //Wall4.transform.localScale = new Vector3(transform.localScale.x / 10, transform.localScale.y, transform.localScale.z / 10);
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
