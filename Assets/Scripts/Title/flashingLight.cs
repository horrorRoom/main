using UnityEngine;
using System.Collections;

public class flashingLight : MonoBehaviour {

    public float FIRSTWAITTIME = 6.0f;
    public float[] INTERVALTIME = { };

    private float sumTime;
    private float intervalTime;
    private int randSelectNum;
   
    private Light spotLight;
	private LightShafts lishtShafts;

	// Use this for initialization
	void Start () {
        //自身のライトを取得
        spotLight = this.GetComponent<Light>();
		lishtShafts = this.GetComponent<LightShafts> ();
        RandTimeSetting();
	}
	
	// Update is called once per frame
	void Update () {
        sumTime += Time.deltaTime;
        if (sumTime > FIRSTWAITTIME)
        {
            intervalTime += Time.deltaTime;
            if (intervalTime > INTERVALTIME[randSelectNum])
            {
                spotLight.enabled = !spotLight.enabled;
				lishtShafts.enabled = !lishtShafts.enabled;
                intervalTime = 0.0f;
                RandTimeSetting();
            }
        } 

	}

    void RandTimeSetting() {
        randSelectNum = (int)Random.Range(0.0f, INTERVALTIME.Length);
    }


}
