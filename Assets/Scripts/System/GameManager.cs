/***********************************************************/
//ゲームのマネージャー
/***********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager singleInstance = new GameManager();

    public static GameManager GetInstance()
    {
        return singleInstance;
    }

    /// <summary>
    /// 演出中
    /// </summary>
    bool isPerformance=false;

    /// <summary>
    /// 演出中かどうか
    /// </summary>
    public bool IsPerformance { set { this.isPerformance =value; } get { return this.isPerformance; } }
}
