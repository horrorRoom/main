using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Route : MonoBehaviour
{
    //初期化
    Dictionary<string, int> time;
    Dictionary<string, int> prev;
    Dictionary<string, int> ans;
    Dictionary<string, int> q;
    Dictionary<string, List<GameObject>> troot;
    public Dictionary<string, List<GameObject>> ansroot;
    int min;
    string line;
    private const int costMax = 999999;
    public List<GameObject> wayPointList;
    public Dictionary<string, Dictionary<string, int>> cost;
    public Dictionary<string, Dictionary<string, int>> costAns;

    private void getCost()
    {
        cost = new Dictionary<string, Dictionary<string, int>>();
        for (int x = 0; x < wayPointList.Count; x++)
        {
            var list = new Dictionary<string, int>();

            for (int y = 0; y < wayPointList.Count; y++)
            {
                if (x == y)
                {
                    list.Add(wayPointList[y].name,0);
                    continue;
                }
                // Rayの作成
                Ray ray = new Ray(wayPointList[x].transform.position, Vector3.Normalize(wayPointList[y].transform.position - wayPointList[x].transform.position));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.transform.tag == "WayPoint")
                    {
                        int dis = (int)(hit.distance * 100);
                        list.Add(wayPointList[y].name, dis);
                        continue;
                    }
                    else
                    {
                        list.Add(wayPointList[y].name, costMax);
                        continue;
                    }

                }
                list.Add(wayPointList[y].name, costMax);
            }

            cost.Add(wayPointList[x].name,list);
        }

        foreach (KeyValuePair<string, Dictionary<string, int>> x in cost)
        {
            foreach (KeyValuePair<string, int> y in x.Value)
            {
                Debug.Log("[x]:" + x.Key + "[y]:" + y.Key + "value" + y.Value);
            }
        }
    }

    public void Initialize()
    {
        // ステージ内のwaypointを取得
        wayPointList = GameObject.FindGameObjectsWithTag("WayPoint").ToList();
        // waypointのパスコスト計算
        getCost();
    }

    public List<GameObject> GetRoot(GameObject start, GameObject goal) {
        // スタートとゴールを設定
        StartSetting(start);
        // ダイクストラで最短経路を返却
        return run(goal);
    }

    public GameObject NearestWayPoint(GameObject target, List<GameObject> list = null) {
        if(list == null) list = wayPointList;
        float dist = 0;
        GameObject result = null;
        foreach (var wayPoint in list)
        {
            // Rayの作成
            Ray ray = new Ray(target.transform.position, Vector3.Normalize(wayPoint.transform.position -target.transform.position));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.tag == "WayPoint")
                {
                    if (result == null || hit.distance <= dist)
                    {
                        dist = hit.distance;
                        result = wayPoint;
                    }
                    continue;
                }
            }
        }
        return result;
    }

    private void StartSetting(GameObject start)
    {
        time = new Dictionary<string, int>();
        prev = new Dictionary<string, int>();
        ans = new Dictionary<string, int>();
        q = new Dictionary<string, int>();
        ansroot = new Dictionary<string, List<GameObject>>();
        troot = new Dictionary<string, List<GameObject>>();
        time = cost[start.name];

        foreach (KeyValuePair<string, int> y in time)
        {
            prev.Add(y.Key, costMax + 1);
            ans.Add(y.Key, 0);
            q.Add(y.Key, 0);
            ansroot.Add(y.Key, new List<GameObject>());
            ansroot[y.Key].Add(start);
            troot.Add(y.Key, new List<GameObject>());
            troot[y.Key].Add(start);
        }
        q[start.name] = 1;
    }

    // prev[]から0以外の最小値を取り出すメソッド
    int Mini()
    {
        int x = costMax;
        foreach (KeyValuePair<string, int> item in prev)
        {
            if (x > item.Value && item.Value != 0)
            {
                x = item.Value;
            }
        }
        return x;
    }
    //time[]から0以外の最小値を取り出すメソッド
    int Minitime()
    {
        int x = costMax;
        foreach (KeyValuePair<string, int> item in time)
        {
            if (x > item.Value && item.Value != 0)
            {
                x = item.Value;
            }
        }
        return x;
    }

    private List<GameObject> run(GameObject goal)
    {
        // コスト初期化
        costAns = new Dictionary<string, Dictionary<string, int>>();
        costAns = cost;

        while (q.Any(x => x.Value == 0))
        {
            List<GameObject> rot = troot[time.First(x => x.Value == Minitime()).Key];
            //time[]とprev[]を比較し小さい値をprev[]に代入,また同値だった場合prev[a]に0を代入
            foreach (KeyValuePair<string, int> item in time)
            {
                if (item.Value < prev[item.Key])
                {
                    prev[item.Key] = item.Value;
                    ansroot[item.Key] = new List<GameObject>(rot);
                }
                else if (item.Key == time.First(x => x.Value == Minitime()).Key)
                {
                    prev[item.Key] = 0;
                    ansroot[item.Key] = new List<GameObject>(rot);
                }
            }

            //prev[]から最小値とその列番号を取り出し、ans[]に入力
            min = Mini();
            line = prev.First(x => x.Value == min).Key;
            ans[line] = min;


            //cos[][line]の行に飛び、minをそれぞれに足す
            foreach (KeyValuePair<string, int> item in time)
            {
                costAns[line][item.Key] = costAns[line][item.Key] + min;
                troot[item.Key] = new List<GameObject>(ansroot[item.Key]);
                var point = wayPointList.First(x => x.name == line);
                troot[item.Key].Add(point);
            }
            //cos[][line]とtime[]の置き換え
            time = costAns[line];

            //一列終了判定
            q[line] = 1;
            if (q[goal.name] == 1)
            {
                ansroot[line] = new List<GameObject>(troot[line]);
                // ゴールのオブジェクト返す
                break;
            }
        }
#if UNITY_DEBUG
    DebugLine(ansroot[goal.name]);
#endif
        return ansroot[goal.name];
    }

    public GameObject nestStart;
    public GameObject nestGoal;

    private void DebugLine(List<GameObject> targetList)
    {
#if UNITY_DEBUG
        foreach (var wayPoint in wayPointList)
        {
            if(wayPoint.GetComponent<LineRenderer>() != null)
                Destroy(wayPoint.GetComponent<LineRenderer>());
        }
        foreach (var target in targetList.Select((item, index) => new {Value = item, Index = index }))
        {
            if(target.Index == targetList.Count - 1) break;
            var renderer = target.Value.AddComponent<LineRenderer>();
            renderer.SetWidth(0.1f, 0.1f);
            // 頂点の数
            renderer.SetVertexCount(2);
            // 頂点を設定
            renderer.SetPosition(0, target.Value.transform.position);
            renderer.SetPosition(1, targetList[target.Index + 1].transform.position);
        }
#endif
    }

    void Start()
    {
        Initialize();
        var start = NearestWayPoint(nestStart);
        var goal = NearestWayPoint(nestGoal);
        var test = GetRoot(start, goal);
        foreach (var item in test)
        {
            Debug.Log(item.name);
        }
    }
}
