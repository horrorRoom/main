using UnityEngine;
using System.Collections;

public class Painting_Production : MonoBehaviour {
	private Ray ray;
	private RaycastHit rayHit;

	public GameObject obj;	   		//絵画のゲームオブジェクト(柄が変わるところ)
	public GameObject parent; 		//絵画の額縁
	public GameObject ura;    		//objの裏にある本来の絵
	private bool releaseRay;        //オブジェクトを取得した後に目線を外したか

	public float fadeTime = 0.3f;	//絵画の絵が変化する時間
	public float fallTime = 2.0f;	//絵画の落下までの時間
	private bool hit_paint;			//parentの当たり判定

	// Use this for initialization
	void Start () {
        releaseRay = false;
    }
	
	// Update is called once per frame
	void Update () {
		ray = new Ray(transform.position, transform.forward);
		if(Physics.Raycast(ray,out rayHit,30.0f))
		{
			//タグがPaintObjなら取得
			if(rayHit.collider.CompareTag("PaintObj")){
				obj = rayHit.collider.gameObject;
				parent = obj.transform.root.gameObject;
				ura = parent.transform.Find("uraPaint").gameObject;
			}
		}

        if (obj != null && rayHit.collider.tag != "PaintObj") releaseRay = true;
		if (releaseRay == true) ChangeTexture();

		if(parent == null) return; 

		hit_paint = parent.GetComponent<Hit_Paint>().Hit;
		if(hit_paint == true) VanishPaint();
	}

	void ChangeTexture()
	{
		if(obj == null) return;
		Color clr = obj.GetComponent<Renderer>().material.color; 
		clr.a -= fadeTime * Time.deltaTime;
		obj.GetComponent<Renderer>().material.color = clr;

		if(clr.a <= 0) fallTime -= 1.0f * Time.deltaTime;
		if(fallTime <= 0) 
		{
			fallTime = 0;
			parent.GetComponent<Rigidbody>().useGravity = true;
		}
	}
					
	void VanishPaint()
	{
		if(obj == null) return;
		Color uraClr = ura.GetComponent<Renderer>().material.color;
		uraClr = Color.gray;
		ura.GetComponent<Renderer>().material.color = uraClr;

		//取得していた絵画のオブジェクトを解放
		obj = null;
		parent = null;
		ura = null;
	}
}
