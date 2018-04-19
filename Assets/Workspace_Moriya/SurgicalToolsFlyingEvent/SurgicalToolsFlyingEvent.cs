/**====================================================================*/
/**
 * 盆に乗った手術道具が、プレイヤー目がけて飛んでくるイベント
 * 作成者：守屋　作成日：16/11/23*/
/**====================================================================*/
using UnityEngine;
using System.Collections;

public class SurgicalToolsFlyingEvent : MonoBehaviour 
{
    [SerializeField, Tooltip("プレイヤー強制移動後の位置")]
    private GameObject m_PlayerMovedPosition;
    [SerializeField, Tooltip("飛ばしたいオブジェクトをここにセット")]
    private GameObject[] m_Tools;
    [SerializeField, Tooltip("飛ぶ前の浮いたときの座標")]
    private GameObject[] m_FloatPositions;
    [SerializeField, Tooltip("飛んだ後の座標")]
    private GameObject[] m_MovedPositions;
    [SerializeField, Tooltip("プレイヤーの移動完了時間")]
    private float m_PlayerMoveTime = 2.0f;
    [SerializeField, Tooltip("浮き始めるまでの待機時間")]
    private float m_FloatWaitTime = 1.0f;
    [SerializeField, Tooltip("浮くまでの完了時間")]
    private float m_FloatMoveTime = 5.0f;
    [SerializeField, Tooltip("飛び始めるまでの待機時間")]
    private float m_FlyingWaitTime = 2.0f;
    [SerializeField, Tooltip("飛び終わるまでの完了時間")]
    private float m_FlyingMoveTime = 0.5f;
    [SerializeField, Tooltip("最後の待機時間")]
    private float m_FinishWaitTime = 2.0f;

    [SerializeField, Tooltip("飛び始めた後、スローモーション開始時間")]
    private float m_StartSlowMotionTime = 0.2f;
    [SerializeField, Tooltip("スローモーションする時間の長さ")]
    private float m_SlowMotionSec = 1.4f;
    [SerializeField, Tooltip("スローモーション倍率")]
    private float m_SlowMotionMul = 20.0f;
    //プレイヤー
    private PlayerMove m_Player;
    //イベント開始時のプレイヤーの座標
    private Vector3 m_StartPlayerPosition;
    //イベント開始時にプレイヤーが向いていた地点
    private Vector3 m_StartPlayerLookPos;
    //飛ばしたいオブジェクトのイベント開始時の座標
    private Vector3[] m_StartToolPositions;

	void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        m_StartToolPositions = new Vector3[m_Tools.Length];
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //プレイヤーの座標保持
            m_StartPlayerPosition = m_Player.transform.position;
            m_StartPlayerLookPos = m_StartPlayerPosition + m_Player.transform.forward;
            //プレイヤーを動けなくする
            m_Player.PlayerStateEffect();

            //飛んでくるオブジェクトの座標保持
            for (int i = 0; i < m_Tools.Length; i++)
            {
                m_StartToolPositions[i] = m_Tools[i].transform.position;
            }

            //イベント開始
            StartCoroutine(PlayerMove());
        }
    }

    //プレイヤーを定位置に動かす処理
    IEnumerator PlayerMove()
    {
        float timer = 0.0f;
        while (true)
        {
            timer += Time.deltaTime;
            //移動
            m_Player.transform.position = Vector3.Lerp(m_StartPlayerPosition, m_PlayerMovedPosition.transform.position, timer/2.0f);
            //向きを変える
            m_Player.PositionLookAt(Vector3.Lerp(m_StartPlayerLookPos, m_Tools[0].transform.position, timer / m_PlayerMoveTime));
            
            //移動と向き変えが終了したら
            if (timer > m_PlayerMoveTime)
            {
                //浮く処理開始
                yield return StartCoroutine(ToolsFloat());
                yield break;
            }
            yield return null;
        }
    }

    //オブジェクトが浮く処理
    IEnumerator ToolsFloat()
    {
        //待機時間
        float waitTimer = 0.0f;
        while (waitTimer < m_FloatWaitTime)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }

        float timer = 0.0f;
        while (true)
        {
            timer += Time.deltaTime;
            //浮く
            for (int i = 0; i < m_Tools.Length; i++ )
            {
                m_Tools[i].transform.position = Vector3.Lerp(
                    m_StartToolPositions[i],
                    m_FloatPositions[i].transform.position,
                    timer / m_FloatMoveTime);
            }

            //プレイヤーの向きを変える
            m_Player.PositionLookAt(m_Tools[0].transform.position);

            //移動が終了したら
            if (timer > m_FloatMoveTime)
            {
                //飛ぶ処理開始
                yield return StartCoroutine(ToolsFlying());
                yield break;
            }
            yield return null;
        }
    }

    //オブジェクトが飛ぶ処理
    IEnumerator ToolsFlying()
    {
        //待機時間
        float waitTimer = 0.0f;
        while (waitTimer < m_FlyingWaitTime)
        {
            waitTimer += Time.deltaTime;
            yield return null;
        }

        float timer = 0.0f;
        float slowmotion = 0.0f;
        while (true)
        {
            //スローモーションする
            if (timer > m_StartSlowMotionTime && slowmotion < m_SlowMotionSec)
            {
                slowmotion += Time.deltaTime;
                timer += Time.deltaTime / m_SlowMotionMul;
            }
            else
            {
                timer += Time.deltaTime;
            }

            //移動が終了してないなら
            if (timer < m_FlyingMoveTime + m_FinishWaitTime)
            {
                //飛ばす
                for (int i = 0; i < m_Tools.Length; i++)
                {
                    m_Tools[i].transform.position = Vector3.Lerp(
                        m_FloatPositions[i].transform.position,
                        m_MovedPositions[i].transform.position,
                        timer / m_FlyingMoveTime);
                }

                //プレイヤーの向きを変える
                m_Player.PositionLookAtFreezeXZ(m_Tools[0].transform.position);

            }
            //移動が終了したら
            else
            {
                //移動可能
                m_Player.transform.up = Vector3.up;
                //ずれてしまったプレイヤーの前と上を直す
                Vector3 front = m_MovedPositions[0].transform.position - m_PlayerMovedPosition.transform.position;
                front.y = 0;
                front.Normalize();
                m_Player.transform.forward = front;
                //状態を移動可能に変更してイベント終了
                m_Player.PlayerStatePlay();
                yield break;
            }
            yield return null;
        }
    }
}
