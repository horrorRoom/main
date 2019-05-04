using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージの管理
/// </summary>
public class StageController : MonoBehaviour {
    [SerializeField]
    //現在のいるシーン
    private string sceneName;
    [SerializeField]
    //ゴールで跳ぶシーン
    private string nextSceneName;

    private Save save;

    /// <summary>
    /// Use this for initialization
    /// </summary>
    void Start () {
        save = GameObject.FindGameObjectWithTag("Save").GetComponent<Save>();
        //シーンを保存
        save.SetSceneName(sceneName);

        //BGM
        //SoundManager.GetInstance().BGMPlay("stageBgm", Sound.PlayerMode.LOOP);
    }

    /// <summary>
    /// 次のシーン名
    /// </summary>
    /// <returns></returns>
    public string SceneName() { return sceneName; }

    /// <summary>
    /// 次のシーン名
    /// </summary>
    /// <returns></returns>
    public string NextSceneName() { return nextSceneName; }
}
