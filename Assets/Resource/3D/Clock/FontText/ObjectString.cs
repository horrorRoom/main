using UnityEngine;
using System.Collections;

public class ObjectString : MonoBehaviour
{
    public string Text;
	public bool countFlag = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	public bool ReturnCountFlag(){
		return countFlag;
	}

	public void SetCountFlag(bool flag){
		countFlag = flag;
	}
}
