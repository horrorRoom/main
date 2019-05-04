using UnityEngine;
using System.Collections;

/// <summary>
/// ステージの情報を保存(リザルトや次のステージに持っていく
/// </summary>
public class Save : MonoBehaviour {
    private string sceneName;

    void Start()
    {
        //すでに生成されていたら消去
        GameObject[] saves = GameObject.FindGameObjectsWithTag("Save");
        if (saves.Length > 1)
        {
            Destroy(this.gameObject);
        }
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
