using UnityEngine;
using System.Collections;

public class noise_test : MonoBehaviour {
	public bool check = false;
	void Start () {
		NoiseManager.Instance.Init ();
	}

	void Update () {

		//if (Input.GetKeyDown (KeyCode.Return)) NoiseManager.Instance.SetNoiseCheckFlag (true);

		//NoiseManager.Instance.MomentNoise();


		if (Input.GetKeyDown (KeyCode.X)) NoiseManager.Instance.SetFlasingFlag (true);
		if (Input.GetKeyDown (KeyCode.C)) NoiseManager.Instance.SetFlasingFlag (false);

		NoiseManager.Instance.FlashingNoise ();
	}
}
