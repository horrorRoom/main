/*********************************************************/
//Rayでオブジェクトを認識した際
/*********************************************************/
using UnityEngine;
using System.Collections;

public class RayHitObject : MonoBehaviour {
	private Ray mRay;
	private RaycastHit mRayHit;

    /*********************************************************/
    // Use this for initialization
    /*********************************************************/
    void Start () {}

    /*********************************************************/
    // Update is called once per frame
    /*********************************************************/
    void Update () 
	{
		mRay = new Ray(transform.position, transform.forward);
		if(Physics.Raycast(mRay,out mRayHit,2.0f))
		{
            GameObject rayHitObject = mRayHit.collider.gameObject;

            //ホラーアイテム
            if (rayHitObject.tag == "HorrorItem")
            {
                rayHitObject.transform.Find("MousePlease").GetComponent<SpriteRenderer>().enabled=true;
                if (Input.GetMouseButtonDown(0)) rayHitObject.GetComponent<HorrorItemDraw>().Play();
            }

            //ホラーシステム
            if (rayHitObject.tag == "HorrorSystem")
            {
                rayHitObject.transform.Find("MousePlease").GetComponent<SpriteRenderer>().enabled = true;
                if (Input.GetMouseButtonDown(0)) rayHitObject.GetComponent<HorrorSystemManager>().Play();
            }

            //ドアノブ
            if (rayHitObject.tag == "Cylinder")
			{
                //ドア
                DoorBase door = rayHitObject.GetComponent<DoorKnob>().Door();
                if(door.IsLockDoor()) return;

                rayHitObject.transform.Find("MousePlease").GetComponent<SpriteRenderer>().enabled = true;

                if (Input.GetMouseButtonDown(0)) door.Action();
			}
		}
	}
}
