using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamegeEffect : MonoBehaviour {
    public Image damageImage = null;        // damage image
    public Animation damageAnim = null;     // damage Anim

    private float mbrTime = 0.0f;   // current time
	
	// Update is called once per frame
	void Update ()
    {
        if(!damageAnim.isPlaying)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }	
	}
}
