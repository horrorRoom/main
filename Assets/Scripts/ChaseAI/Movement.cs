using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
    敵行動制御クラス
*/
public class Movement : MonoBehaviour
{
    [ConstantField]
    [SerializeField]
    private Transform TF;
    [ConstantField]
    [SerializeField]
    private float speed;
    [ConstantField]
    [SerializeField]
    private float nextdist;
    [ConstantField]
    [SerializeField]
    private GameObject nextWayPoint;
    [ConstantField]
    [SerializeField]
    private int nextKey;
    [ConstantField]
    [SerializeField]
    private GameObject preWayPoint;
    [ConstantField]
    [SerializeField]
    private MoveState State;
    [ConstantField]
    [SerializeField]
    private MoveBehavior Move;
    public List<GameObject> MapWayPoints;

    [System.Serializable]
    class MoveBehavior
    {
        public float accel;
        public float speedMax;
        public float decelerationDist = 0;
        public float decelerationAccel = 0;
        public List<GameObject> wayPoints;
        public MoveMode mode;
        public Animation animation;
    };

    [SerializeField] MoveBehavior normal;
    [SerializeField] MoveBehavior attention;
    [SerializeField] MoveBehavior chase;

    [SerializeField]
    public Route route;

    enum MoveMode
    {
        Rotation = 1,
        Turn,
        Stop
    }

    enum MoveState
    {
        Normal = 1,
        Attention,
        Chase
    }

    enum ActionState
    {
        Stay = 1,
        Attention,
        Move1,
        Move2,
        Move3
    }
    /*
        chase中はplayerにrayを飛ばし続け、rayが当たった場合直線移動
    */
    #if UNITY_DEBUG
    LineRenderer renderer;
    #endif

    void Start()
    {
        initialize();

        #if UNITY_DEBUG

        renderer = this.gameObject.AddComponent<LineRenderer>();
        // 線の幅
        renderer.SetWidth(0.1f, 0.1f);

        #endif

    }
    public List<GameObject> testAttentionGameObject;
    public List<GameObject> testChaseGameObject;
    void Update()
    {
        if (checkNextWayPoint())
        {
            setNextWayPoint();
        }

        if (Input.GetMouseButtonDown(0))
        {
            setAttention(testAttentionGameObject);
        }
        if (Input.GetMouseButtonDown(1))
        {
            setNormal();
        }
        if (Input.GetMouseButtonDown(2))
        {
            setChase(testChaseGameObject);
        }

        if(checkDeceleration())
        {
            moveDeceleration();
        } else
        {
            moveForward();
        }
    
    #if UNITY_DEBUG

        // 頂点の数
        renderer.SetVertexCount(2);
        // 頂点を設定
        renderer.SetPosition(0, TF.position);
        renderer.SetPosition(1, TF.position + TF.forward);

    #endif
    }

    bool checkNextWayPoint()
    {
        bool checkNext = false;
        nextdist = Vector3.Distance(nextWayPoint.transform.position, this.transform.position);
        if (nextdist < 1) checkNext = true;
        return checkNext;
    }

    bool checkDeceleration()
    {
        if(Move.mode != MoveMode.Stop) return false;
        bool checkDeceleration = false;
        if (nextdist <= Move.decelerationDist && nextKey == Move.wayPoints.Count - 1) checkDeceleration = true;
        return checkDeceleration;
    }

    void moveForward()
    {
        if (speed <= Move.speedMax)
        {
            speed += Move.accel * Time.deltaTime;
        }
        Vector3 movForward = TF.position + TF.forward * speed * Time.deltaTime;
        // Debug.Log(movForward);
        TF.position = movForward;
    }

    void moveDeceleration()
    {
        if (speed > Move.decelerationAccel * Time.deltaTime)
        {
            speed -= Move.decelerationAccel * Time.deltaTime;
        }else
        {
            speed = 0;
        }
        Vector3 movForward = TF.position + TF.forward * speed * Time.deltaTime;
        // Debug.Log(movForward);
        TF.position = movForward;
    }

    void setNextWayPoint()
    {
        if (nextWayPoint == null)
        {
            nextKey = 0;
        }
        else if (nextKey < Move.wayPoints.Count - 1)
        {
            preWayPoint = nextWayPoint;
            nextKey++;
        }
        else if (nextKey == Move.wayPoints.Count - 1)
        {
            switch (Move.mode)
            {
                case MoveMode.Rotation:
                    preWayPoint = nextWayPoint;
                    nextKey = 0;
                    break;
                case MoveMode.Turn:
                    preWayPoint = nextWayPoint;
                    Move.wayPoints.Reverse();
                    nextKey = 1;
                    break;
                case MoveMode.Stop:
                    nextKey = Move.wayPoints.Count - 1;
                    break;
            }
        }

        nextWayPoint = Move.wayPoints[nextKey];
        this.transform.LookAt(nextWayPoint.transform);

    }

    //　マップのウェイポイントの取得
    void getMapWayPoints()
    {
        MapWayPoints = new List<GameObject>();
        MapWayPoints = route.wayPointList;
    }

    void setState(MoveState state)
    {
        State = state;
        switch (state)
        {
            case MoveState.Normal:
                Move = normal;
                break;
            case MoveState.Attention:
                Move = attention;
                break;
            case MoveState.Chase:
                Move = chase;
                break;
        }
    }

    void initialize()
    {
        TF = this.gameObject.transform;
        getMapWayPoints();
        setState(MoveState.Normal);
        setNextWayPoint();
    }
    public void setNormal()
    {
        Move = normal;
        setState(MoveState.Normal);
        nextWayPoint = route.NearestWayPoint(this.gameObject, normal.wayPoints);
        setNextWayPoint();
    }
    public void setAttention(List<GameObject> wayPointList)
    {
        TF = this.gameObject.transform;
        attention.wayPoints = wayPointList;
        Move = attention;
        setState(MoveState.Attention);
        nextWayPoint = null;
        setNextWayPoint();
    }
    public void setChase(List<GameObject> wayPointList)
    {
        TF = this.gameObject.transform;
        chase.wayPoints = wayPointList;
        Move = chase;
        setState(MoveState.Chase);
        nextWayPoint = null;
        setNextWayPoint();
    }
}
