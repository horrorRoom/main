using UnityEngine;
using System.Collections;

public class ShakeObject : MonoBehaviour
{
    public float mShakeAmount;
    public bool mShakeX;
    public bool mShakeY;
    public bool mShakeZ;

    private Vector3 mCriteriaPosition;//基準位置
    // Use this for initialization
    void Start()
    {
        mCriteriaPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if (mShakeX == true)
        {
            position.x = mCriteriaPosition.x + Random.Range(-mShakeAmount, mShakeAmount);
        }
        if (mShakeY == true)
        {
            position.y = mCriteriaPosition.y + Random.Range(-mShakeAmount, mShakeAmount);
        }
        if (mShakeZ == true)
        {
            position.z = mCriteriaPosition.z + Random.Range(-mShakeAmount, mShakeAmount);
        }
        transform.position = position;
    }
}
