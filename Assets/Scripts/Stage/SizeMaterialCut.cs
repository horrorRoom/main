using UnityEngine;
using System.Collections;

public class SizeMaterialCut : MonoBehaviour {

    Vector3 size;

	// Use this for initialization
	public void Start () {
        size = transform.localScale;

        // 3次元から2次元表現に
        float x = size.x;
        float y = size.z;

        Material mat = GetComponent<Renderer>().material;
        //mat.SetTextureOffset("_MainTex", Vector2.one);

        mat.SetTextureScale("_MainTex", new Vector2(x, y));
        mat.SetTextureScale("_EmissionMap", new Vector2(x, y));
        mat.SetTextureScale("_DetailAlbedoMap", new Vector2(x, y));



    }


}
