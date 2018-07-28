/*********************************************************/
//ドアノブ
/*********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKnob : MonoBehaviour {

    [SerializeField]
    private DoorBase mDoor;

    /*********************************************************/
    // ドアノブがついているドアを返す
    /*********************************************************/
    public DoorBase Door () { return mDoor; }
}
