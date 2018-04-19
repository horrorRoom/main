using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {
	private Transform tr;

	[SerializeField]
	private float endAngle;          //最終的に向けたいy軸アングル
	[SerializeField]
	private float endTime;           //何秒かけて閉じたいか

	private float startAngle;       //開始時のアングル（Scene上に配置した時点でのy軸アングル） 
	public bool isOpen = false;   //ドアが閉じているか？
	private float timer = 0.0f;     //タイマー

	private GameObject cameraObj;
	[SerializeField]private int doorNum;

	public int levelCount = 0;

	void Awake()
	{
		//所持コンポーネントの取得
		tr = GetComponent<Transform>();
	}
	
	void Start () {
		//開始時のアングルを取得
		startAngle = transform.eulerAngles.y;
		cameraObj = GameObject.FindGameObjectWithTag ("MainCamera");
	}
	
	void Update () {
		if (cameraObj.GetComponent<Title> ().moveFlag == true &&
			doorNum == cameraObj.GetComponent<Title> ().RotateCount) {
			//タイマー増加
			timer += Time.deltaTime;

			transform.eulerAngles =
				Vector3.Lerp (new Vector3 (0, startAngle, 0), new Vector3 (0, endAngle, 0), timer / endTime);
			if (timer >= 1.0f) isOpen = true;
		}
	}
}

