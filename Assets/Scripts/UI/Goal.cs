/*********************************************************/
//ゴール時の演出
/*********************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour {
    /*==外部設定変数==*/
    [SerializeField]
    private string nextSceneName;
    [SerializeField]
    private float waitTime = 1.0f;//ゴールしてからフェードするまでの待機時間（秒）

    /*==内部設定変数==*/
    private GameObject fade;
    private float time=0.0f;
    private bool isFade = false;
    //ドアノブをまわしたかどうか
    private bool isOpen=false;

    /*==外部参照変数==*/
    public bool isGoal = false;

    public AudioSource OpenDoor;

    public HelpManager[] helpManager = new HelpManager[3];

    void Start()
    {
        fade = GameObject.FindGameObjectWithTag("Fade");
        if (SceneManager.GetActiveScene().name == "Stage1") {
            GameObject[] obj = GameObject.FindGameObjectsWithTag("HelpManager");
            for (int i = 0; i < obj.Length; i++) {
                helpManager[i] = obj[i].GetComponent<HelpManager>();
            }
        }
    }

    void Update()
    {
        if (isOpen) FadeOut();
    }

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!isGoal) OpenDoor.Play();
            //プレイヤーが触れたらゴール
            isGoal = true;

            if (SceneManager.GetActiveScene().name == "Stage1")
            {
                for (int i = 0; i < 3; i++)
                {
                    helpManager[i].goal = true;
                }
            }
        }
    }

    public void GoalDoorOpen() {
        isOpen = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().PlayerStateEffect();
    }

    //フェードアウト
    void FadeOut()
    {
        //エラーチェック
        if (Message.ErrorMessage(fade,"Goal.cs\nfadeオブジェクトがnullです。")) return;
        //ゴールしていなければリターン
        if (!isGoal) return;

        //フェードアウトが完了したら次のシーンへ
        if (!fade.GetComponent<Fade>().isEnd && isFade) Application.LoadLevel(nextSceneName);

        
        //数秒たったらフェードアウトさせる
        time += Time.deltaTime;
        if (time >= waitTime && !isFade)
        {
            fade.GetComponent<Fade>().isEnd = true;
            isFade = true;
        }
        
    }
}
