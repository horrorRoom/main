using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Option : MonoBehaviour {
	public Slider slider;

	void Start () {
		slider = GameObject.Find("Slider").GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void BackTitle(){
		Debug.Log("Click. nowValue = " + slider.value);

		PlayerPrefs.SetFloat ("MouseSensitivity", slider.value);
		Application.LoadLevel("Title_new");
	}
}
