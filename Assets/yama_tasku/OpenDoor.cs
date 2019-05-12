using UnityEngine;
using System;
using System.Collections;

public class OpenDoor : MonoBehaviour {
    [SerializeField]
    private Title titleController;
    [SerializeField]
    private float endAngle;          //最終的に向けたいy軸アングル
    [SerializeField]
    private float endTime;           //何秒かけて閉じたいか

    private float startAngle;       //開始時のアングル（Scene上に配置した時点でのy軸アングル） 
    private bool isOpen = false;   //ドアが閉じているか？
	private float timer = 0.0f;     //タイマー

	[SerializeField]private int doorNum;

	public int levelCount = 0;

	void Start () {
		//開始時のアングルを取得
		startAngle = transform.eulerAngles.y;
	}
	
	void Update () {
		if (titleController.IsMove() && doorNum == titleController.GetRotateCount())
        {
			//タイマー増加
			timer += Time.deltaTime;

			transform.eulerAngles = Vector3.Lerp (new Vector3 (0, startAngle, 0), new Vector3 (0, endAngle, 0), timer / endTime);
			if (timer >= 1.0f) isOpen = true;
		}
	}

    /// <summary>
    /// ドアが開いているかどうか返す
    /// </summary>
    public bool IsOpen() { return isOpen; }
}

