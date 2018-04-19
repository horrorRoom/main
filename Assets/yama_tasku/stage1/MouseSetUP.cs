using UnityEngine;
using System.Collections;

public class MouseSetUP : MonoBehaviour {
	private MouseSetUP ThisScript;

	// Use this for initialization
	void  OnEnable() {

		this.transform.GetComponent<MouseLook>().sensitivityX = PlayerPrefs.GetFloat ("MouseSensitivity");
		this.transform.GetComponent<MouseLook>().sensitivityY = PlayerPrefs.GetFloat ("MouseSensitivity");

		ThisScript = GetComponent<MouseSetUP> ();

		//remove
		Object.Destroy (ThisScript);
	}
}
