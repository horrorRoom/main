using UnityEngine;
using System.Collections;

public class DrawOutline : MonoBehaviour {
	private Material itemDefaultMat; 					//アイテムの通常時のマテリアル
	[SerializeField] private float rayLength = 30.0f;	//レイの長さ(表示するかの判定用)
	private GameObject item;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 rayPos = new Vector3 (transform.position.x,
		                              transform.position.y - 0.7f,
		                              transform.position.z);

		//デバック用.後で消す
		Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
		Debug.DrawRay(rayPos, forward, Color.green);
	}

	//オブジェクトが可視状態のとき、カメラごとに一度呼び出されるコールバック
	void OnWillRenderObject(){
#if UNITY_EDITOR
		if(Camera.current.name != "SceneCamera"  && Camera.current.name != "Preview Camera")	
#endif
		{
			Ray ray;
			RaycastHit rayHit;
			
			Vector3 rayPos = new Vector3 (transform.position.x,
			                              transform.position.y - 0.7f,
			                              transform.position.z);
			
			ray = new Ray(rayPos, transform.forward);
			if (Physics.Raycast (ray, out rayHit, rayLength)) {
				if(rayHit.collider.CompareTag("Item")){
					item = rayHit.collider.gameObject;
					item.GetComponent<Material>().shader = Shader.Find("Custom/Outline");


				}

				if(!rayHit.collider.CompareTag("Item")){
					item = rayHit.collider.gameObject;
					item.GetComponent<Material>().shader = Shader.Find("Custom/Outline");
					
					
				}
			}
		}
	}
}
