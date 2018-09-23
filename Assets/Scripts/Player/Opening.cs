/*********************************************************/
/// Rayでオブジェクトを認識した際
/*********************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject Spotlight;
    [SerializeField]
    Animator animator;

    /*********************************************************/
    /// <summary>
    /// Use this for initialization
    /// </summary>
    /*********************************************************/
    void Start () {
        player.GetComponent<MouseLook>().enabled = false;
        Spotlight.SetActive(false);
    }

    /*********************************************************/
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    /*********************************************************/
    void Update () {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime>1.0f)
        {
            player.GetComponent<MouseLook>().enabled = true;
            Spotlight.SetActive(true);
        }
	}
}
