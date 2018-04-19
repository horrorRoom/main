using UnityEngine;
using System.Collections;

public class MouseDelete : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // カーソルを表示しない
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        // カーソルをウィンドウから出さない
        Screen.lockCursor = true;
	}
}
