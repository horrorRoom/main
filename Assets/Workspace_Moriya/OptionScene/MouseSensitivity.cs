/**====================================================================*/
/**
 * マウスの感度を保管するクラス
 * 作成者：守屋　作成日：15/4/20*/
/**====================================================================*/

using UnityEngine;
using System.Collections;

public class MouseSensitivity : MonoBehaviour 
{
    [SerializeField]
    private float m_Sensitivity = 10.0f;
    [SerializeField]
    private float m_MaxSensitivity = 20.0f;

    void Awake()
    {
        MouseLook ml = GameObject.Find("Main Camera").GetComponent<MouseLook>();
        ml.sensitivityX = ml.sensitivityY = m_Sensitivity;
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void AddSensitivity(float value)
    {
        m_Sensitivity += value;
        SensitivityClamp();
    }

    public void SetSensitivity(float value)
    {
        m_Sensitivity = value;
        SensitivityClamp();
    }

    public float GetSensitivity()
    {
        return m_Sensitivity;
    }

    public float GetMax()
    {
        return m_MaxSensitivity;
    }

    private void SensitivityClamp()
    {
        m_Sensitivity = Mathf.Clamp(m_Sensitivity, 1.0f, m_MaxSensitivity);
    }
}
