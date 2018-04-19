using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndingAProduction : MonoBehaviour {
	public Text[] textList = new Text[12]; 

	private bool[] alphaFlag = new bool[13];
	private bool heteFlag = false;
	private bool updateFlag = false; //複数更新防止用
	private float alpha=0f;
	private int alphaCount=0;

	private float waitTimer=0f;
	public float waitTime=2f;

	private GameObject[] objList;
	private Text[] heteList = new Text[100];

	private float heteTimer=0f;
	public float heteTime=1f;
	private int heteCount=0;

	private bool finishFlag;

	public fadePanel fade;

	// Use this for initialization
	void Start () {
		objList = GameObject.FindGameObjectsWithTag ("hete");

		for (int i = 0; i < objList.Length; i++) {
			heteList [i] = objList [i].GetComponent<Text> ();
			Color initC = heteList [i].color;
			initC.a = 0;
			heteList [i].color = initC;
		}
		fade.isStart = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (fade.fadeAlpha<=0.0f) {				
			alphaFlag[alphaCount] = true;
		}

		EndA_Product ();
		EndA_Product_Last ();
	}

	void EndA_Product(){
		if (alphaFlag [alphaCount] == true && heteFlag != true) {
			alpha += Time.deltaTime;

			Color c = textList [alphaCount].color;
			c.a = alpha;
			textList [alphaCount].color = c;

			if (alpha > 1f) {
				alpha = 1f;

				waitTimer += Time.deltaTime;
				if (waitTimer >= waitTime) {
					alpha = 0f;

					Color endC = textList [alphaCount].color;
					endC.a = 0;
					textList [alphaCount].color = endC;

					alphaFlag [alphaCount + 1] = true;
					alphaCount += 1;

					if (alphaCount >= textList.Length) {
						waitTimer = 0f;
						heteFlag = true;
					} else {
						waitTimer = 0f;
					}
				}
			}
		}
	}

	void EndA_Product_Last(){
		if (heteFlag == true && finishFlag!=true) {
			heteTimer += Time.deltaTime;

			if (heteTimer > heteTime && heteCount<heteList.Length) {
				Color heteC = heteList [heteCount].color;
				heteC.a = 1f;
				heteList [heteCount].color = heteC;
				heteCount += 1;
				heteTimer = 0f;

				if (heteCount >= heteList.Length) {
					finishFlag = true;
				}
			}
		}
	}

	public void StartEndA(){
		if (updateFlag != true) {
			alphaFlag[0] = true;
			updateFlag = true;
		}
	}

	public bool ReturnFinishFlag(){
		return finishFlag;
	}
}
