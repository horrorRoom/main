using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class WayPointAgent : MonoBehaviour {

    // 参考資料
    // https://gist.github.com/tsubaki/7e22cec8527c534e4c7a
    private WaypointProgressTracker tracker = null;

    [SerializeField, Range(0, 10)]
    protected float speed = 1;

    void Start()
    {
        tracker = GetComponent<WaypointProgressTracker>();
    }

    void Update()
    {
        Vector3 targetPosition = tracker.progressPoint.position + tracker.progressPoint.direction;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
