/**====================================================================*/
/**
 * チームロゴシーンの進行
 * 作成者：守屋　作成日：16/04/16*/
/**====================================================================*/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TeamLogoScene : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private AudioSource audio;
    //フェードイン用の黒画像
    private Image fadeImage;

    /*==外部設定変数==*/
    //効果音
    [SerializeField]
    private AudioClip m_LightOnOffSE;
    //移行先シーン名
    [SerializeField]
    private string m_NextSceneName = "Title_new";
    //明かりがつく時間
    [SerializeField]
    private float m_LightOnTime = 2.0f;
    //明かりが消える時間
    [SerializeField]
    private float m_LightOffTime = 7.0f;
    //シーン全体の所要時間
    [SerializeField]
    private float m_SceneChangeTime = 9.0f;

    /*==内部設定変数==*/
    //時間計測用
    private float m_Timer = 0.0f;
    //各タイミングでSEを鳴らしたか？
    private bool m_IsLightOnEnd = false;
    private bool m_IsLightOffEnd = false;

    void Awake()
    {
        //コンポーネント取得
        audio = GetComponent<AudioSource>();
        fadeImage = transform.Find("Canvas").Find("FadeBlack").GetComponent<Image>();
    }

	void Start() 
    {
        m_Timer = 0.0f;
        fadeImage.color = new Color(0, 0, 0, 1);
	}

	void Update() 
    {
        m_Timer += Time.deltaTime;

        //スペースキーを押すと進行
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!m_IsLightOnEnd && m_Timer < m_LightOnTime)
                m_Timer = m_LightOnTime;
            else if (!m_IsLightOffEnd && m_Timer < m_LightOffTime)
                m_Timer = m_LightOffTime;
            else if (m_Timer < m_SceneChangeTime)
                m_Timer = m_SceneChangeTime;
        }

        //経過時間により進行
        if (!m_IsLightOnEnd && m_Timer >= m_LightOnTime)
        {
            m_IsLightOnEnd = true;
            audio.PlayOneShot(m_LightOnOffSE);
            fadeImage.color = new Color(0, 0, 0, 0);
        }
        if (!m_IsLightOffEnd && m_Timer >= m_LightOffTime)
        {
            m_IsLightOffEnd = true;
            audio.PlayOneShot(m_LightOnOffSE);
            fadeImage.color = new Color(0, 0, 0, 1);
        }

        if (m_Timer >= m_SceneChangeTime)
            SceneManager.LoadScene(m_NextSceneName);
	}
}
