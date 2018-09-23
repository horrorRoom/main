/*********************************************************/
//ゴール時の自動でドアが閉じる演出
/*********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoClause : MonoBehaviour {
    //ドアが閉まるエリア
    [SerializeField]
    private DoorBase door;

    /*********************************************************/
    // Use this for initialization
    /*********************************************************/
    void Start () {}

    /*********************************************************/
    // Update is called once per frame
    /*********************************************************/
    void Update () {}

    /*********************************************************/
    /// <summary>
    /// プレイヤーが入っていたらドアを閉める
    /// </summary>
    /*********************************************************/
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            door.CloseDoor();
        }
    }
}
