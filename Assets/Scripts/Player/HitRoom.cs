using UnityEngine;
using System.Collections;

/// <summary>
/// 現在プレイヤーがいる部屋の番号を管理するクラス
/// </summary>
public class HitRoom : MonoBehaviour {
	public bool isHit;
    //今いる場所の番号
    public int number=0;

	void Start () {
		isHit = false;
	}
	
	void OnTriggerStay(Collider other)
	{
        if (other.gameObject.tag == "Room")
        {
            isHit = true;
            number = other.GetComponent<Area>().number;
        }
	}
	void OnTriggerExit(Collider other)
	{
        if (other.gameObject.tag == "Room")
        {
            isHit = false;
        }
	}
}
