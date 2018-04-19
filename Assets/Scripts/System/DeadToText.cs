using UnityEngine;
using System.Collections;

/// <summary>
/// 死んだら(ゲームオーバーになったら)テキストを置く
/// </summary>
public class DeadToText : MonoBehaviour {

    public Sprite[] textSprites;
    public float playerHeight;
    [SerializeField]
    static private Vector3 spritePosition;
    static private Quaternion spriteRotation;
    static private bool isDraw;
    private SpriteRenderer spriteRendrer;

    //public GameObject player;

    void Start()
    {
        if( isDraw )
        {
            CreateTextSprite();
        }
    }


    static public void SetSpriteTransform( Transform player )
    {
        spritePosition = player.position;
        spriteRotation = player.rotation;
        isDraw = true;

    }

    public void CreateTextSprite()
    {
        if(textSprites.Length == 0 )
        {
            Debug.unityLogger.LogError("not sprite","スプライトを設定してください");
            return;
        }
        if( !spriteRendrer )
        {
            GameObject obj = new GameObject("DeadToText");
            spriteRendrer = obj.AddComponent<SpriteRenderer>();
            spriteRendrer.sprite = textSprites[0];
        }
        if ( spriteRendrer )
        {
            spriteRendrer.transform.rotation = Quaternion.Euler(0, spriteRotation.eulerAngles.y, 0) * Quaternion.AngleAxis(90, Vector3.right);
            spriteRendrer.transform.position = spritePosition - Vector3.up * playerHeight;
            int random = Random.Range(0, textSprites.Length );
            spriteRendrer.sprite = textSprites[random];
        }
        
    }
	
}
