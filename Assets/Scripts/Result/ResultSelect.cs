/***********************************************************/
//リザルトの選択
/***********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ResultSelect : MonoBehaviour {

    private SCENE_RESULT sceneResult = 0;

    //シーンのセーブデータ
    private Save save;

    //選択結果
    enum SCENE_RESULT
    {
        SCENE_BACK=0,
        TITLE=1,
    }

	void Start () {
        save = GameObject.FindGameObjectWithTag("Save").GetComponent<Save>();
    }
	
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) sceneResult = SCENE_RESULT.SCENE_BACK;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) sceneResult = SCENE_RESULT.TITLE;

        //決定
        if (Input.GetKey(KeyCode.Space))
        {
            Message.ErrorMessage(save, "ResultSelect.cs\nsaveオブジェクトがnulｌです。");
            if (sceneResult == SCENE_RESULT.SCENE_BACK) SceneManager.LoadScene(save.GetSceneName());
            if (sceneResult == SCENE_RESULT.TITLE) SceneManager.LoadScene("Title");
        }
    }

    /// <summary>
    /// 選択した結果を数字にして返す
    /// </summary>
    /// <returns></returns>
    public int GetSelectNumber() { return (int)sceneResult;  }
}
