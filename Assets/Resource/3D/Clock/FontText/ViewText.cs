using UnityEngine;
using System.Collections;

public class ViewText : MonoBehaviour
{
    private Ray ray;
    private RaycastHit rayHit;

    public string objectTag;
    public string objectName;
    public string fontText = "";

    public ObjectString text;

    public GUIStyle gui;
    private Color textColor;

    public Vector3 vec;

	public int CheckCount = 0; //いくつの血文字を見たか
	public GameObject goal;

    // Use this for initialization
    void Start()
    {
        textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        gui.fontStyle = FontStyle.Bold;
        //文字のサイズ調整
        gui.fontSize = Screen.width / 35;

		CheckCount = PlayerPrefs.GetInt ("Count", 0);
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out rayHit, 5.0f))
        {
            //違うテキストオブジェクトだった場合
            if (rayHit.collider.gameObject.GetComponent<ObjectString>() != null && rayHit.collider.gameObject.GetComponent<ObjectString>().Equals(text) == false)
            {
                text = rayHit.collider.gameObject.GetComponent<ObjectString>();
                fontText = text.Text;
                textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            //同じテキストだった場合
            else if (rayHit.collider.gameObject.GetComponent<ObjectString>() != null && rayHit.collider.gameObject.GetComponent<ObjectString>().Equals(text) == true)
            {
                textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
            //それ以外のオブジェクトの場合
            else
            {
                text = null;
            }

			if (rayHit.collider.CompareTag("Logo") && rayHit.collider.GetComponent<ObjectString> () != null) {
				if(rayHit.collider.GetComponent<ObjectString>().ReturnCountFlag() != true){
					CheckCount += 1;
					rayHit.collider.GetComponent<ObjectString> ().SetCountFlag (true);
				}
			}
        }
        //rayが当たらなかった場合
        else
        {
            text = null;
        }

        textColor.a = Mathf.Max(0.0f, textColor.a - 0.01f);
        this.gui.normal.textColor = textColor;

		SaveCount ();
    }
    void OnGUI()
    {
        Rect textPos = new Rect((Screen.width / 2.0f) - ((fontText.Length * (gui.fontSize / 2)) / 2), (Screen.height / 1.2f) - (gui.fontSize / 2),
                                (Screen.width / 2.0f) + ((fontText.Length * (gui.fontSize / 2)) / 2), (Screen.height / 1.2f) + (gui.fontSize / 2));

        GUI.Label(textPos, fontText, gui);
    }
	void SaveCount(){
		if (goal == null) return;

		if (goal.GetComponent<Goal> ().isGoal == true) {
			PlayerPrefs.SetInt ("Count", CheckCount);
		}
	}
}
