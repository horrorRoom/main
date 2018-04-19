using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class NoiseManager : MonoBehaviour {
	protected static NoiseManager instance;
	
	public static NoiseManager Instance {
		get{
			if (instance == null) {
				instance = (NoiseManager)FindObjectOfType(typeof(NoiseManager));
				
				if (instance == null) {
					Debug.LogError("NoiseManager Instance Error");
				}
			}
			return instance;
		}
	}

	public NoiseAndScratches noise;
	public float grainMin, grainMax;
	public bool NoiseSet, NoiseCheck, NoiseOnece = false;
	public float delayTime, def_time;

	public AnimationCurve curve;
	private float flasingTime;
	public bool flasingFlag = false;


	public void Awake()
	{
		if(this != Instance)
		{
			Destroy(this);
			return;
		}
		
		//DontDestroyOnLoad(this.gameObject);
		noise = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<NoiseAndScratches>();
		def_time = delayTime;
		Init ();
	}
		
	public void Init(){
		noise.grainIntensityMin = 0;
		noise.grainIntensityMax = 0;
		NoiseSet = false;
	}

	//指定した時間の間,ノイズを出す
	public void MomentNoise(){
		if (NoiseCheck == true){
			delayTime -= Time.deltaTime;
			if (NoiseSet != true) {
				noise.grainIntensityMin = grainMin;
				noise.grainIntensityMax = grainMax;
				NoiseSet = true;
			}
			
			if (delayTime < 0) {
				Init ();
				delayTime = def_time;
				NoiseCheck = false;
			}
		}
	}

	/// <summary>
	/// 外部呼出し向けのノイズ機能
	/// </summary>
	/// <param name="t">T.</param>
	public void MomentNoise_External(float t){
		if (NoiseOnece == true) return;

		if (NoiseOnece != true) {
			t -= Time.deltaTime*5;

			if (NoiseSet != true) {
				noise.grainIntensityMin = grainMin;
				noise.grainIntensityMax = grainMax;
				NoiseSet = true;
			}

			if (t < 0) {
				Init ();
				NoiseOnece = true;
			}
		}
	}

	//作成したアニメーションカーブに合わせてノイズを出す
	public void FlashingNoise(){
		if (flasingFlag == true) {
			flasingTime += Time.deltaTime;

			noise.grainIntensityMin = curve.Evaluate (flasingTime);
			noise.grainIntensityMax = curve.Evaluate (flasingTime);
			
			if (flasingTime > curve.keys [curve.length - 1].time) {
				flasingTime = 0;
			}
		} else if(flasingFlag==false){
			flasingTime = 0;
			noise.grainIntensityMin = 0;
			noise.grainIntensityMax = 0;
		}
	}

	//呼び出すたびに引数value分Noiseの値を加算する
	public void AddNoise(float value){
		noise.grainIntensityMin += value;
		noise.grainIntensityMax += value;
	}

	//呼び出し先にNoiseCheckの状態を返す
	public bool ReturnNoiseCheckFlag(){
		return NoiseCheck;
	}

	//呼び出し先からNoiseCheckの状態を変更する
	public bool SetNoiseCheckFlag(bool flag){
		return NoiseCheck = flag;
	}

	//呼び出し先にflasingFlagの状態を返す
	public bool ReturnFlasingFlag(){
		return flasingFlag;
	}

	//呼び出し先からflasingFlagの状態を変更する
	public bool SetFlasingFlag(bool flag){
		return flasingFlag = flag;
	}
}
