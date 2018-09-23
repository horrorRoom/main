/*********************************************************/
//ボタンが入力できる物の上にだすマーク
/*********************************************************/
using UnityEngine;
using System.Collections;

public class ItemMark : MonoBehaviour {
    //ドア用のUIかどうか
    [SerializeField]
    private bool isDoor=false;

    //プレイヤー
    private Transform player;

    float time=0;

    /*********************************************************/
    // Use this for initialization
    /*********************************************************/
    void Start () {
        transform.position += new Vector3(0, 0.3f, 0);
        //プレイヤー
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    /*********************************************************/
    // Update is called once per frame
    /*********************************************************/
    void Update() {
        //表示数を管理する
        DrawController();

        if (gameObject.GetComponent<SpriteRenderer>().enabled) {
            time += 1.0f * Time.deltaTime;
            if (time >= 1.0f) {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                time = 0;
            }
        }

        //プレイヤーの方向を向く
        transform.LookAt(GameObject.FindGameObjectWithTag("MainCamera").transform);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x+20, transform.eulerAngles.y +180, transform.eulerAngles.z * -1);

        //マークの位置
        //if (isDoor) UIPosition();
    }

    /*********************************************************/
    //マークの位置
    /*********************************************************/
    void UIPosition()
    {
        if(transform.eulerAngles.y >180) transform.position = transform.parent.transform.position + transform.parent.TransformDirection(Vector3.up) * 0.3f;
        if (transform.eulerAngles.y < 180) transform.position = transform.parent.transform.position + transform.parent.TransformDirection(Vector3.down) * 0.3f;

        transform.position = new Vector3(transform.position.x, transform.parent.position.y + 0.3f, transform.position.z);
    }

    /*********************************************************/
    //複数出された場合、１つしかでないようにする
    /*********************************************************/
    void DrawController()
    {
        if (gameObject.GetComponent<SpriteRenderer>().enabled==false) return;

        GameObject[] ItemMark = GameObject.FindGameObjectsWithTag("MouseMark");
        if (ItemMark.Length < 2) return;
        int draw_num = 0;
        for (int i = 0; i < ItemMark.Length; i++) {
            if (ItemMark[i].GetComponent<SpriteRenderer>().enabled) draw_num++;
        }

        //表示している数が１つ以上だった場合
        if (draw_num > 1)
        {
            int number = 0;
            float length = Vector3.Distance(player.position, ItemMark[0].transform.position);
            for (int i = 1; i < ItemMark.Length; i++)
            {
                if (length > Vector3.Distance(player.position, ItemMark[i].transform.position))
                {
                    length = Vector3.Distance(player.position, ItemMark[i].transform.position);
                    number = i;
                }
            }
            //一番距離が短いやつ以外は表示を消す
            for (int i = 0; i < ItemMark.Length; i++)
            {
                if (number != i){ ItemMark[i].GetComponent<SpriteRenderer>().enabled = false; }
            }
        }
    }

}
