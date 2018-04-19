using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemSprite : MonoBehaviour {
	public Image itemPoint; 							//アイテムの上に出すスプライト
	[SerializeField] private float rayLength = 5.0f;	//rayの長さ
	[SerializeField] private GameObject item;							//見ているアイテム

	private Ray ray;
	private RaycastHit rayHit;

	private Camera MainCamera;							//メインのカメラ
	public Camera UICamera;								//UI用カメラ
	public float itemPoint_PlusX,itemPoint_PlusY;						//表示位置修正用変数
	
	void Start () {


		MainCamera = this.GetComponent<Camera> ();
	}

	void Update () {

		Vector3 rayPos = new Vector3 (transform.position.x,
		                              transform.position.y - 0.7f,
		                              transform.position.z);

		ray = new Ray(rayPos, transform.forward);
		if (Physics.Raycast (ray, out rayHit, rayLength)) {
			if(rayHit.collider.CompareTag("Item")){
				item = rayHit.collider.gameObject;

				Vector3 itemViewportPoint = MainCamera.WorldToViewportPoint (item.transform.position);
				Vector3 labelPos = UICamera.ViewportToWorldPoint (itemViewportPoint);
				labelPos.x = labelPos.x + itemPoint_PlusX;
				labelPos.y = labelPos.y + itemPoint_PlusY;
				labelPos.z = 0;

				itemPoint.transform.position = labelPos;

				itemPoint.enabled = true;
			}else if (!rayHit.collider.CompareTag("Item") || 
			 Vector3.Distance(item.transform.position,this.transform.parent.transform.position) > rayLength){
                if (itemPoint == null) return;
                itemPoint.enabled = false;
			}
		}else{
            if (itemPoint == null) return;
			itemPoint.enabled = false;
		}
	}
}
