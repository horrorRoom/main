using UnityEngine;
using System.Collections;

public class RayHitObject : MonoBehaviour {
	private Ray mRay;
	private RaycastHit mRayHit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		mRay = new Ray(transform.position, transform.forward);
		if(Physics.Raycast(mRay,out mRayHit,2.0f))
		{
            //ホラーアイテム
            if (mRayHit.collider.gameObject.tag == "HorrorItem")
            {
                mRayHit.collider.gameObject.transform.Find("MousePlease").GetComponent<SpriteRenderer>().enabled=true;
                if (Input.GetMouseButtonDown(0))
                {
                    mRayHit.collider.gameObject.GetComponent<HorrorItemDraw>().Play();
                }
            }

            //ホラーシステム
            if (mRayHit.collider.gameObject.tag == "HorrorSystem")
            {
                mRayHit.collider.gameObject.transform.Find("MousePlease").GetComponent<SpriteRenderer>().enabled = true;
                if (Input.GetMouseButtonDown(0))
                {
                    mRayHit.collider.gameObject.GetComponent<HorrorSystemManager>().Play();
                }
            }

                //ドアノブ
                if (mRayHit.collider.gameObject.tag == "Cylinder")
			{
                if(mRayHit.collider.transform.parent.gameObject.GetComponent<Door>().isOpenNow) return;
                
                mRayHit.collider.gameObject.transform.Find("MousePlease").GetComponent<SpriteRenderer>().enabled = true;

                if (Input.GetMouseButtonDown(0)){
					mRayHit.collider.transform.parent.gameObject.GetComponent<Door>().Action();
				}
			}
		}
	}
}
