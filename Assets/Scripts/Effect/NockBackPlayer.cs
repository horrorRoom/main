using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::
// @Author : go arakawa June 2017
// @Brief : Nock Back Player Event
//:::::::::::::::::::::::::::::::::::::::::::::::::::::::
public class NockBackPlayer : MonoBehaviour {

    public GameObject girlPrefab = null;    // girl prefab
    public bool isDestroy = false;          // is destroy
    public float flyTime = 2.0f;            // Fly Anim Time
    public float flyHeight = 2.0f;          // Fly Anim Height
    public float nockbackLength = 3.0f;     // NockBack Length

    private PlayerMove mbrPlayer = null;    // player prefab
    private GameObject mbrGirl = null;      // girl instance
    private float mbrGirlScale = 12;        // scale

	// Use this for initialization
	void Start () {
        mbrPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // @Breif : Start Anim
    private IEnumerator Play()
    {
        mbrPlayer.PlayerStateEffect();
        //Create Instance
        if (mbrGirl == null)
        {
            mbrGirl = Instantiate(girlPrefab, transform, false);
            mbrGirl.transform.localScale = new Vector3(mbrGirlScale, mbrGirlScale, mbrGirlScale);
        }
        //Look At
        mbrGirl.transform.LookAt(mbrPlayer.transform.position);
        mbrGirl.transform.eulerAngles = new Vector3(0.0f, mbrGirl.transform.eulerAngles.y, 0.0f);
        mbrPlayer.PositionLookAtFreezeXZ(mbrGirl.transform.position);

        // Start Fiy Anim
        yield return StartCoroutine(FlyPlayer(flyTime));

        // End Event
        mbrPlayer.PlayerStatePlay();
        if (isDestroy)
        {
            Destroy(gameObject);
        }
        yield break;
    }

    // @Breif : Player Flying Anim
    private IEnumerator FlyPlayer (float fEndTime)
    {
        float fTime = 0.0f;
        // Init Position
        Vector3 vStartPos = mbrPlayer.transform.position;
        Vector3 vEndPos = mbrPlayer.transform.position + (mbrPlayer.transform.up * flyHeight);
        while (true)
        {
            mbrPlayer.transform.position = Vector3.Lerp(vStartPos, vEndPos, fTime / fEndTime);
            fTime += Time.deltaTime;
            if (fTime >= fEndTime)
            {
                // Make knockback animation when you finish skipping
                Vector3 vNockBackPos = mbrPlayer.transform.position + mbrGirl.transform.forward * nockbackLength;
                yield return StartCoroutine(mbrPlayer.NockBack(vNockBackPos));
                yield break;
            }

            yield return null;
        }
    }

    // @Breif : On Trigger Enter
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Play());
        }
    }

}
