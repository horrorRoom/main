using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// ゲームオーバーシーンに飛ぶ時に使うクラス
/// </summary>
public class ToGameOver : MonoBehaviour {

    //[SerializeField]
    //private DeadToText deadToText;

 

    /// <summary>
    /// 何もせずにゲームオーバーシーンに飛ぶ　
    /// </summary>

    //public static void DebugApply()
    //{
    //     SceneManager.LoadScene("GameOver");
    //}

   
    /// <summary>
    /// ゲームオーバーシーンへ
    /// 文字を出すため座標と角度がほしい
    /// </summary>
    public static void Apply( Transform playerTransform )
    {
        DeadToText.SetSpriteTransform(playerTransform);
        SceneManager.LoadScene("GameOver");
        //Application.LoadLevel("GameOver");
    }
}
