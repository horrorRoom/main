using UnityEngine;
using System.Collections;

/// <summary>
/// 開閉式のドアを利用して壁の外側に出るのを防ぐ当たり判定
/// トリガーするとずらす
/// </summary>
public class DoorExtrusion : MonoBehaviour {

    /*==外部設定変数==*/
    //押し出す方向
    [SerializeField]
    private Vector3 extrusionVec = new Vector3(0.0f, 0.0f, 2.0f);

    /// <summary>
    /// 当たり判定
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position += extrusionVec;
        }
    }
}
