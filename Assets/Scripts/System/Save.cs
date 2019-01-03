/***********************************************************/
//ステージの情報を保存(リザルトや次のステージに持っていく
/***********************************************************/
using UnityEngine;
using System.Collections;

public class Save : MonoBehaviour {
    [SerializeField]
    //ステージの管理
    private StageController stageController;

    private string sceneName;

    void Start()
    {   
        //すでに生成されていたら消去
        GameObject[] saves = GameObject.FindGameObjectsWithTag("Save");
        if (saves.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

	void Update () {
        DontDestroyOnLoad(gameObject);
	}

    /// <summary>
    /// 現在のいたステージ名
    /// </summary>
    /// <returns></returns>
    public string GetSceneName() { return sceneName; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    public void SetSceneName(string name) { sceneName = name; }
}
