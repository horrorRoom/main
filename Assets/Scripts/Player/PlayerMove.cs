/**==========================================================================*/
/**
 * プレイヤーの移動。（歩き、ジャンプ）
 * CharacterControllerを通して移動させてます。
 * 作成者：守屋   作成日：15/08/19*/
/**==========================================================================*/

using UnityEngine;
using System.Collections;

public enum State
{
    play = 0,
    admiratiopn = 1,
    effect=2
};

//必須コンポーネント
[RequireComponent(typeof(CharacterController))]

public class PlayerMove : MonoBehaviour
{
    /*==所持コンポーネント==*/
    private Transform tr;
    private CharacterController controller;

    /*==外部設定変数==*/
    //移動速度
    public float m_MoveSpeed = 3.0f;
    //ジャンプ初速度
    public float m_JumpSpeed = 8.0f;
    //重力
    public float m_Gravity = 22.0f;
    //ダッシュ時速度
    public float m_DashSpeed = 6.0f;
    //プレイヤーの状態
    public int playState = (int)State.play;

    //壁衝突時のダメージエフェクト
    public GameObject   damageEffect = null;

    /*==内部設定変数==*/
    //フェードイン・アウトオブジェクト
    private GameObject fade;
    //移動方向を示すベクトル
    private Vector3 m_MoveVector;
    private bool m_IsNockBack = false;

    //カメラ
    [SerializeField]
    GameObject[] RatateObject;

    /* ==[ギョル追記開始]== */
    // メスイベント時にプレイヤの移動を止めるために作成
    /// <summary> 現在プレイヤが移動可能状態なのか？ </summary>
    public bool IsMovable;
    /* ==[ギョル追記終了]== */

    /*==================*/
    /* 生成前前初期化   */
    /*==================*/
    void Awake()
    {
        //コンポーネント取得
        tr = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
    }

    /*==================*/
    /* 更新前初期化   */
    /*==================*/
    void Start()
    {
        //オブジェクト取得
        fade = GameObject.FindGameObjectWithTag("Fade");

        gameObject.GetComponent<MouseLook>().enabled = false;

        // [ギョル追記開始] ======
        IsMovable = true;
        // [ギョル追記終了] ======
    }

    /*==================*/
    /* 更新処理   */
    /*==================*/
    void Update()
    {
        //エラーチェック
        if (Message.ErrorMessage(fade,"PlayerMove.cs\nFadeオブジェクトがnullです。")) return;

        //フェードアウトが完了していなかったら何もしない
        if (fade.GetComponent<Fade>().isStart) return;

        //通常の状態じゃないなら動かせない
        if (playState != (int)State.play)
        {
            //カメラの固定
            CameraFixing();
            return;
        }

        //カメラ旋回許可
        CameraRotate();

        MoveAxisControl();
        //JumpControl();

        // [ギョル追記開始] ======
        if(IsMovable)
        {
            Move();
        }
        // [既存のコード]
        // Move();
        // [ギョル追記終了] ======
    }

    /*================================*/
    /* XZ方向の移動の入力状態を取得   */
    /*================================*/
    private Vector3 GetMoveInput()
    {
        //入力状態を取得
        Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //加速と減速をいい感じに補正
        if (directionVector != Vector3.zero)
        {
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;
            directionLength = Mathf.Min(1, directionLength);
            directionLength = directionLength * directionLength;
            directionVector = directionVector * directionLength;
        }
        return directionVector;
    }

    /*======================================================*/
    /* 現在向いている方向（forward）を基準としたXZ方向移動   */
    /*======================================================*/
    private void MoveAxisControl()
    {
        //地面に接地している時のみ有効
        if (controller.isGrounded)
        {
            //現在向いている方向を参照
            Vector3 forward = tr.TransformDirection(Vector3.forward);
            Vector3 right = tr.TransformDirection(Vector3.right);
            Vector3 inputVector = GetMoveInput();

            //移動方向を決定
            m_MoveVector = inputVector.x * right + inputVector.z * forward;
            //移動速度を加える
            if (Input.GetKey(KeyCode.LeftShift))
                m_MoveVector *= m_DashSpeed;
            else
                m_MoveVector *= m_MoveSpeed;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
                    transform.position += (transform.up *2.0f);
            NockBack(transform.position - transform.forward * 3);
        }
    }

    /*=======================================*/
    /* XZ方向移動（テスト用、使用しない）   */
    /*=======================================*/
    private void MoveControl()
    {
        if (controller.isGrounded)
        {
            m_MoveVector = GetMoveInput();
            m_MoveVector *= m_MoveSpeed;
        }
    }

    /*==============*/
    /* ジャンプ   */
    /*==============*/
    private void JumpControl()
    {
        if (Input.GetButtonDown("Jump"))
            m_MoveVector.y = m_JumpSpeed;
    }

    /*==============*/
    /* 移動処理  */
    /*==============*/
    private void Move()
    {
        if (!controller.isGrounded)
        {
            m_MoveVector.y -= m_Gravity * Time.deltaTime;
        }

        //controllerを利用して移動
        controller.Move(m_MoveVector * Time.deltaTime);
    }

    /*==============*/
    /*吹っ飛び処理  */
    /*==============*/
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        m_IsNockBack = false;
    }

    public IEnumerator NockBack(Vector3 vPosition)
    {
        m_IsNockBack = true;
        float fLength = Vector3.Magnitude(transform.position - vPosition);
        Vector3 vVector = (vPosition - transform.position).normalized;
        while (true)
        {
            controller.Move(vVector* 50 * Time.deltaTime);
            float fCurrentLength = Vector3.Magnitude(transform.position - vPosition);
            if (fCurrentLength >= fLength || !m_IsNockBack)
            {
                if (!m_IsNockBack)
                {
                    GameObject pObj = Instantiate(damageEffect);
                }
                m_IsNockBack = false;
                PlayerStatePlay();
                yield break;
            }
            yield return null;
        }
    }

    /*====================*/
    /* カメラの旋回許可    */
    /*====================*/
    void CameraRotate()
    {
        gameObject.GetComponent<MouseLook>().enabled = true;

        for (int i = 0; i < RatateObject.Length; i++) {
            RatateObject[i].GetComponent<MouseLook>().enabled = true;
        }
    }

    /*====================*/
    /* カメラを使えなくする */
    /*====================*/
    void CameraFixing()
    {
        gameObject.GetComponent<MouseLook>().enabled = false;

        for (int i = 0; i < RatateObject.Length; i++)
        {
            RatateObject[i].GetComponent<MouseLook>().enabled = false;
        }
    }

	public void SetMoveSpeed(float walkSpeed, float dashSpeed){
		m_MoveSpeed = walkSpeed;
		m_DashSpeed = dashSpeed;
	}

    /*================================================================================*/
    /* 02/20追加　プレイヤーとカメラとライトを指定した座標を向けさせる */
    /*================================================================================*/
    public void PositionLookAt(Vector3 pos)
    {
        gameObject.transform.LookAt(pos);
        for (int i = 0; i < RatateObject.Length; i++)
        {
            RatateObject[i].transform.LookAt(pos);
        }
    }

    public void PositionLookAtFreezeXZ(Vector3 pos)
    {
        gameObject.transform.LookAt(pos);
        for (int i = 0; i < RatateObject.Length; i++)
        {
            RatateObject[i].transform.LookAt(pos);
        }
        //XZ軸は動かさない
        gameObject.transform.eulerAngles = new Vector3(0.0f, gameObject.transform.eulerAngles.y, 0.0f);
    }

    //演出シーンなので動けなくさせる
    public void PlayerStateEffect(){ playState = (int)State.effect; }

    //ゲーム状態へ動けるようにする
    public void PlayerStatePlay(){ playState = (int)State.play; }
}
