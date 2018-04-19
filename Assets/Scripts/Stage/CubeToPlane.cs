using UnityEngine;
using System.Collections;

public class CubeToPlane : MonoBehaviour {

    GameObject cube;
    public GameObject plane;

	// Use this for initialization
	void Start ()   {
        Create();

	}

    public void Create()
    {
        cube = this.gameObject;

        Vector3 position = cube.transform.position;
        Vector3 scale = cube.transform.localScale;

        scale = cube.transform.rotation * scale;
        
        // 横幅のスケール値を保存（縦幅はCubeの高さで一定）
        float[] widthScaleBox =
        {
            scale.x,
            scale.z,
            scale.x,
            scale.z
        };
        
        // Cubeの周りに板ポリ生成
        for (int i = 0; i < 4; ++i)
        {

            float angle = 90 * i;
            
            float radian = angle * Mathf.Deg2Rad;
            Vector3 planePos = cube.transform.position + new Vector3((scale.x + 0.0f) * Mathf.Sin(radian), 0, (scale.z + 0.0f) * Mathf.Cos(radian)) * 0.5f;
            //Vector3 planePos = /*cube.transform.position +*/new Vector3((scale.x + 0.0f) * rotateAngle.x, rotateAngle.y, (scale.z + 0.0f) * rotateAngle.z) * 0.5f;

            Vector3 pos = (cube.transform.rotation * planePos) + cube.transform.position;
            pos = planePos;
            //Debug.Log("kaiten"+ planePos);
            //Debug.Log( "p,y"+fromPitchYaw( rot.y, rot.x ) );
            //Quaternion rotate = plane.transform.rotation * Quaternion.AngleAxis( angle, Vector3.back );
            // 偶数だったら
            
            Quaternion rotate = Quaternion.AngleAxis(angle + 180.0f, Vector3.down);
            //GameObject obj = Instantiate(plane, pos, rotate) as GameObject;
            // ポリゴン生成
            GameObject obj = CreatePlane();
            obj.transform.position = pos;
            obj.transform.rotation = rotate;
            obj.GetComponent<Renderer>().material = cube.GetComponent<Renderer>().material;
            obj.transform.localScale = new Vector3(Mathf.Abs( widthScaleBox[i] ), Mathf.Abs( scale.y ), 1);
            obj.transform.SetParent(cube.transform);

            // 裏ポリゴン生成
            obj = CreatePlane();
            obj.transform.position = pos;
            obj.transform.rotation = rotate*Quaternion.AngleAxis(180.0f, Vector3.down); ;
            obj.GetComponent<Renderer>().material = cube.GetComponent<Renderer>().material;
            
            obj.transform.localScale = new Vector3(Mathf.Abs(widthScaleBox[i]), Mathf.Abs(scale.y), 1);
            obj.transform.SetParent(cube.transform);
        }
        cube.GetComponent<Renderer>().enabled = false;
    }

    // いたポリゴン生成
    public GameObject CreatePlane()
    {
        GameObject obj = new GameObject("SimplePlane");
        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        //Mesh m = (Mesh)AssetDatabase.LoadAssetAtPath("Assets/SimplePlane.asset", typeof(Mesh));
        Mesh m = null;
        if (m == null)
        {
            m = new Mesh();
            m.name = "SimplePlane";
            Vector3[] vertices = new Vector3[]
            {
                new Vector3( 0.5f,  0.5f, 0.0f),
                new Vector3(-0.5f, -0.5f, 0.0f),
                new Vector3(-0.5f,  0.5f, 0.0f),
                new Vector3( 0.5f, -0.5f, 0.0f),
            };
            int[] triangles = new int[]
            {
                0, 1, 2,
                3, 1, 0,
                //3, 2, 1,
                //0, 2, 3
            };
            Vector2[] uv = new Vector2[]
            {
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 0.0f),
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 0.0f)
            };
            m.vertices = vertices;
            m.triangles = triangles;
            m.uv = uv;
            m.RecalculateNormals();

            //AssetDatabase.CreateAsset(m, "Assets/SimplePlane.asset");
            //AssetDatabase.SaveAssets();
        }
        meshFilter.sharedMesh = m;
        m.RecalculateBounds();

        return obj;
    }

    /*
    Vector3 fromPitchYaw( float pitch, float yaw)
    {
        float rPitch = Mathf.Deg2Rad * pitch;
        float rYaw = Mathf.Deg2Rad * yaw;

        return new Vector3(
            Mathf.Cos(rPitch) * Mathf.Sin(rYaw),
            -Mathf.Sin(rPitch),
            Mathf.Cos(rPitch) * Mathf.Cos(rYaw)
        );
    }
    */
}
