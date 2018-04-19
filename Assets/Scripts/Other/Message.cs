using UnityEngine;
using System.Collections;

public class Message : MonoBehaviour
{
    /// <summary>
    /// 引数に入れたオブジェクトがnullかどうかを返すと同時に、nullの場合はエラーメッセージを出力
    /// </summary>
    /// <returns>true:null,false:nullではない</returns>
    public static bool ErrorMessage<T>(T obj)
    {
         if (obj == null)
         {
             Debug.LogError("nullゲームオブジェクトを指定しています。");
             return true;
         }
         return false;
    }
    public static bool ErrorMessage<T>(T obj, string message)
    {
        if (obj == null)
        {
            Debug.LogError(message);
            return true;
        }
        return false;
    }

}
