using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndAManager : MonoBehaviour {
	public GameObject[] conclusion_role = new GameObject[2];
	public GameObject finishObj;
	public fadePanel fade;
	private bool flag = false;
	private bool finish = false;

	float firstTimer=0f, secondTimer=0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (finish == true) {
			//if (Input.GetKeyDown (KeyCode.Space)) {
				//fade.isEnd = true;

				//if(fade.fadeAlpha >= 1f){
				//	Application.LoadLevel("Title_new");
				//}
			//}
		} else {
			if (flag != true) {
				flag = this.GetComponent<EndingAProduction> ().ReturnFinishFlag ();
			} else if (flag == true) {
				finishObj.SetActive (true);

				firstTimer += Time.deltaTime;
				if (firstTimer >= 1f) {
					firstTimer = 1f;
					FadeFinishFunction ();
				}
			}
		}
	}
	void FadeFinishFunction(){
		if (fade == null) return;
		fade.isEnd = true;
		Debug.Log ("fadefinish in");
		if (fade.fadeAlpha >= 1f) {
			Debug.Log ("fadefinish if in");
			conclusion_role [0].SetActive (false);
			conclusion_role [1].SetActive (true);
			fade.isStart = true;

			finish = true;
		}
	}
}
