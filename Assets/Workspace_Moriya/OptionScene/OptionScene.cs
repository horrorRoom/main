/**====================================================================*/
/**
 * オプションシーン管理クラス
 * メインカメラにアタッチして使用
 * 作成者：守屋　作成日：15/4/20*/
/**====================================================================*/

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionScene : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;
    private MouseSensitivity ms;
    private MouseLook ml;
    private Transform centerLine;
    private Transform centerLineMaxPos;
    private Transform centerLineMinPos;
    private Transform moveStartPos;
    private Transform moveEndPos;
    private Fade fade;

    /*==内部設定変数==*/
    //カメラが見る地点のインデックス番号
    private int m_LookIndex;
    private int m_LookIndexPrev;
    //カメラが見る地点を変更中か？
    private bool m_IsChangeLook = false;
    //カメラが見る地点を元に戻している途中か？
    private bool m_IsReturnLook = false;

    /*==外部設定変数==*/
    //カメラが見る地点を配列に格納
    [SerializeField]
    private Transform[] m_CameraLookPos;
    //カメラが動く地点
    [SerializeField]
    private Transform m_CameraMovePos;
    //回転に要する時間
    [SerializeField]
    private float m_LookChangeEndSec = 1.0f;
    //視点戻しに要する時間
    [SerializeField]
    private float m_LookReturnEndSec = 0.5f;
    //移動に要する時間
    [SerializeField]
    private float m_MoveEndSec = 2.0f;

    void Awake()
    {
        tr = GetComponent<Transform>();
    }

    void Start()
    {
        //2つ以上MouseSensitivityがある場合は余分な分削除
        GameObject[] sensitivityObj = GameObject.FindGameObjectsWithTag("MouseSensitivity");
        if (sensitivityObj.Length > 1)
        {
            for (int i = 1; i < sensitivityObj.Length; i++)
            {
                GameObject.Destroy(sensitivityObj[i]);
            }
        }

        ms = GameObject.Find("MouseSensitivity").GetComponent<MouseSensitivity>();
        ml = GameObject.Find("Main Camera").GetComponent<MouseLook>();
        centerLine = GameObject.Find("CenterLine").transform;
        centerLineMaxPos = GameObject.Find("Max").transform;
        centerLineMinPos = GameObject.Find("Min").transform;
        moveStartPos = GameObject.Find("CameraMoveStartPos").transform;
        moveEndPos = GameObject.Find("CameraMoveEndPos").transform;
        fade = GameObject.Find("Fade").GetComponent<Fade>();

        m_LookIndex = 0; 
        m_LookIndexPrev = 0;
        tr.LookAt(m_CameraLookPos[0]);

        centerLine.position = Vector3.Lerp(
            centerLineMinPos.position,
            centerLineMaxPos.position,
            ms.GetSensitivity() / ms.GetMax());

        ml.enabled = false;
    }

    void Update()
    {
        if (fade.IsStart()) return;

        ChangeLookControl();
        if (m_LookIndex == 0 && !m_IsChangeLook)
        {
            SensitivityControl();
            ml.enabled = true;
        }
        else
            ml.enabled = false;

        if (m_LookIndex == 2 && Input.GetKeyDown(KeyCode.Space))
        {
            StartMove();
        }
    }

    private void SensitivityControl()
    {
        bool isAdd = false;

        if (Input.GetKeyDown(KeyCode.W))
        {
            ms.AddSensitivity(1.0f);
            isAdd = true;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ms.AddSensitivity(-1.0f);
            isAdd = true;
        }

        if (isAdd)
        {
            ml.sensitivityX = ml.sensitivityY = ms.GetSensitivity();
            centerLine.position = Vector3.Lerp(
                centerLineMinPos.position,
                centerLineMaxPos.position,
                ms.GetSensitivity() / ms.GetMax());
        }
    }

    private void ChangeLookControl()
    {
        if (!m_IsChangeLook)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_LookIndexPrev = m_LookIndex;
                m_LookIndex++;
                StartChangeLook();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                m_LookIndexPrev = m_LookIndex;
                m_LookIndex--;
                StartChangeLook();
            }
        }
    }

    private void StartChangeLook()
    {
        //クランプ
        if (m_LookIndex > m_CameraLookPos.Length - 1)
            m_LookIndex = 0;
        else if (m_LookIndex < 0)
            m_LookIndex = m_CameraLookPos.Length - 1;

        ml.SetRotationY(0.0f);
        StartCoroutine(ChangeLook());
    }

    //見る地点切り替え
    IEnumerator ChangeLook()
    {
        if (m_LookIndexPrev == 0)
        {
            Coroutine returnlook = StartCoroutine(ReturnLook());
            yield return returnlook;
        }

        Vector3 lookPos;
        float timer = 0;
        m_IsChangeLook = true;

        while (timer < m_LookChangeEndSec)
        {
            timer += Time.deltaTime;
            lookPos = Vector3.Lerp(
                m_CameraLookPos[m_LookIndexPrev].position,
                m_CameraLookPos[m_LookIndex].position,
                timer / m_LookChangeEndSec);
            tr.LookAt(lookPos);
            yield return null;
        }

        m_IsChangeLook = false;
        yield break; 
    }

    private void StartMove()
    {
        StartCoroutine(Move());
    }

    //部屋の外へ移動
    IEnumerator Move()
    {
        float timer = 0;

        while (timer < m_MoveEndSec)
        {
            timer += Time.deltaTime;
            tr.position = Vector3.Lerp(
                moveStartPos.position,
                moveEndPos.position,
                timer / m_MoveEndSec);
            yield return null;
        }

        SceneManager.LoadScene("Title");
        yield break;
    }

    //視点を元に戻す
    IEnumerator ReturnLook()
    {
        //最初に見ていた地点
        Vector3 startPos = tr.position + tr.forward;
        //元に戻す地点
        Vector3 endPos = m_CameraLookPos[0].position;

        float timer = 0.0f;
        Vector3 lookPos;
        m_IsReturnLook = true;

        while (timer < m_LookReturnEndSec)
        {
            timer += Time.deltaTime;
            lookPos = Vector3.Lerp(startPos, endPos, timer / m_LookReturnEndSec);
            tr.LookAt(lookPos);
            yield return null;
        }

        m_IsReturnLook = false;
        yield break; 
    }
}