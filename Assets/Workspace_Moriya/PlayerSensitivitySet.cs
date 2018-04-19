/**====================================================================*/
/**
 * マウスの感度をプレイヤーにセットする
 * 作成者：守屋　作成日：15/4/30*/
/**====================================================================*/


using UnityEngine;
using System.Collections;

public class PlayerSensitivitySet : MonoBehaviour
{
	void Start()
    {
        GameObject sensitivityObj = GameObject.Find("MouseSensitivity");
        if (sensitivityObj == null)
            return;

        MouseSensitivity ms = sensitivityObj.GetComponent<MouseSensitivity>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<MouseLook>().sensitivityX = ms.GetSensitivity();
        player.transform.Find("Main Camera").GetComponent<MouseLook>().sensitivityY = ms.GetSensitivity();
        player.transform.Find("Spotlight").GetComponent<MouseLook>().sensitivityY = ms.GetSensitivity();
	}
}
