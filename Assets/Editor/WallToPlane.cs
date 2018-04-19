using UnityEngine;
using UnityEditor;
using System.Collections;

public class WallToPlane : EditorWindow {


    // メニューのWindowにEditorExという項目を追加。
    [MenuItem("Window/Select ObjectCube To Plane")]
    static void Open()
    {
        // メニューのWindow/EditorExを選択するとOpen()が呼ばれる。
        // 表示させたいウィンドウは基本的にGetWindow()で表示＆取得する。
        EditorWindow.GetWindow<WallToPlane>("SelectToPlane"); // タイトル名を"EditorEx"に指定（後からでも変えられるけど）
    }

    // Windowのクライアント領域のGUI処理を記述
    void OnGUI()
    {
        GUI.Label( new Rect( 10, 10, 250, 20 ), "選択したオブジェクトにスクリプト追加" );
        if( GUI.Button( new Rect( 10, 30, 200, 20 ), "追加" ) )
        {
            Add();
        }
        GUI.Label(new Rect(10, 60, 300, 20), "スクリプトの入っているオブジェクトをDebug.logで表示");
        if (GUI.Button(new Rect(10, 80, 200, 20), "表示"))
        {
            Print();
        }

        GUI.Label(new Rect(10, 110, 250, 20), "スクリプト削除");
        if (GUI.Button(new Rect(10, 130, 200, 20), "削除"))
        {
            AllDelete();
        }
    }


    void Add()
    {
        Debug.Log("Add:"+Selection.gameObjects.Length);

        foreach (GameObject obj in Selection.gameObjects)
        {
            obj.AddComponent<CubeToPlane>();
        }
    }

    void AllDelete()
    {
        CubeToPlane[] scripts = GameObject.FindObjectsOfType<CubeToPlane>();
        Debug.Log("Delete:" + scripts.Length);

        foreach (CubeToPlane script in scripts)
        {
            DestroyImmediate( script );
        }
    }
        
    void Print()
    {
        CubeToPlane[] scripts = GameObject.FindObjectsOfType<CubeToPlane>();
        Debug.Log("Delete:" + scripts.Length);

        foreach (CubeToPlane script in scripts)
        {
            Debug.Log( "CubeToPlane"+script.gameObject.name );
        }
    }

}
