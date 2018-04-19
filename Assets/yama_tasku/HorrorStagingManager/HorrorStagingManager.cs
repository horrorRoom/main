using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HorrorStagingManager : MonoBehaviour {
	[System.Serializable]
	public class SEs
	{
		public int setNum; 		  	//SEの番号. いちいち設定するのが面倒なので修正予定
		public AudioClip se;	 	//SE本体
		public float volume = 1f; 	//音の大きさ
	}
	[SerializeField]
	SEs[] seParameter;
	float nowVolume= 1f;		 	//音声の基礎数値
	private GameObject player;	 	//プレイヤー
	public float plusPos = 3f;   	//位置補正時のプレイヤーとの距離
	public int divisionNum;		 	//演出が出る確率の割る数

	//それぞれ演出するか,移動するポジション,どの音を鳴らすかの設定用ランダム変数
	int randCheck, randPos, randSE;
	Vector3 setPos;				 	//補正するポジション(Retrun用)

	AudioSource aud_sorce;
	Dictionary<int, SEs> seDictionary;				

	float s_Timer = 0f;				//演出用タイマー
	public float stagingTime = 60f;	//どれくらいの時間に一度演出判定をするのか

	public AnimationCurve curve;	//StagingSE_FadeInOut用のアニメーションカーブ
	float anm_Timer = 0f;

	void Awake () {
		seDictionary = new Dictionary<int, SEs>();
		for (int i = 0; i < seParameter.Length; i++)
		{
			seDictionary[seParameter[i].setNum] = seParameter[i];
		}
		aud_sorce = this.GetComponent<AudioSource> ();
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		s_Timer += Time.deltaTime;

		if(s_Timer >= stagingTime) PlayHorrorSE();
	}

	//メインの機能
	//パブリックなのは地雷式の罠を作ったときのため
	public void PlayHorrorSE(){
		randCheck = Random.Range (0, 100);

		if (randCheck % divisionNum == 0) {
			randPos = Random.Range (0, 3);
			randSE = Random.Range (0, seParameter.Length - 1);
			this.gameObject.transform.position = SelectPosition (randPos);

			SEs se = seDictionary [randSE];

			aud_sorce.loop = false;
			aud_sorce.clip = se.se;
			aud_sorce.volume = nowVolume * se.volume;
			aud_sorce.Play ();

			if (aud_sorce.isPlaying != true) {
				aud_sorce.Stop ();
				s_Timer = 0f;
			}
		} else {
			s_Timer = 0f;
		}
	}



	//ポジションの設定
	Vector3 SelectPosition(int insPos){
		if (insPos == 0) {
			setPos = new Vector3 (player.transform.position.x,
				-3.5f, 
				player.transform.position.z + plusPos);
		} else if (insPos == 1) {
			setPos = new Vector3 (player.transform.position.x,
				-3.5f, 
				player.transform.position.z - plusPos);
		} else if (insPos == 2) {
			setPos = new Vector3 (player.transform.position.x - plusPos,
				-3.5f, 
				player.transform.position.z);
		} else if (insPos == 3) {
			setPos = new Vector3 (player.transform.position.x + plusPos,
				-3.5f, 
				player.transform.position.z);
		}

		return setPos;
	}

	//フェードインアウトするSE演出用関数
	//※使用例→踏切音
	//引数 flag:使用するところで作ってあるフラグを設定し,flagをtrueにすると動く
	public void StagingSE_FadeInOut(bool flag){
		if (flag == true) {
			anm_Timer += Time.deltaTime;
			SEs se = seDictionary [4];

			aud_sorce.loop = false;
			aud_sorce.clip = se.se;
			aud_sorce.volume = nowVolume * se.volume * curve.Evaluate (anm_Timer);
			aud_sorce.Play ();

			if (aud_sorce.isPlaying != true) {
				aud_sorce.Stop ();
				anm_Timer = 0f;
				flag = false;
			}
		}
	}
}
