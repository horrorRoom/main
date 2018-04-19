using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

// とりあえずテスト感
// Ｓｔａｔｅパターン作ってガッツリやりたい（理想）
public class AokiTestAI : MonoBehaviour {
    // 恐らく一時的に上浦ＡＩ使用
    // 最終的に関数だけしか残らないかも
    
    [SerializeField]
    Kamiura kamiuraAI;

    [SerializeField]
    Patrol patrol;

    void Start()
    {
        patrol.enabled = true;
    }

    void Update()
    {
        // 
        if(GetIsPlayerLook())
        {
            patrol.enabled = false;
        }
    }

    // プレイヤーを見つけた
    public bool GetIsPlayerLook()
    {
        return kamiuraAI.GetIsPlayerLook();
    }
    
}
