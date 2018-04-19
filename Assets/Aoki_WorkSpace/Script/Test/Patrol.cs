using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NavMeshがないと使えない
// 参考資料
// http://tsubakit1.hateblo.jp/entry/2014/10/05/230644
public class Patrol : MonoBehaviour {

    public Transform[] wayPoints;

    public int currentRoot;

    void Start()
    {
        if (wayPoints.Length == 0)
        {
            Debug.unityLogger.Log("Patrol 移動先が登録されていません");
            return;
        }
    }

    void Update()
    {
        if (wayPoints.Length == 0)
        {
            return;
        }
        Vector3 pos = wayPoints[currentRoot].position;

        if (Vector3.Distance(transform.position, pos) < 0.5f)
        {
            currentRoot = (currentRoot < wayPoints.Length - 1) ? currentRoot + 1 : 0;
        }

        GetComponent<NavMeshAgent>().SetDestination(pos);
    }
}
