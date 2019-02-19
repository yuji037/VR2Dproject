using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ScreenBreaker : MonoBehaviour
{

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;


    [SerializeField]
    float height;

    [SerializeField]
    float width;

    Vector3 rightBottomPos;
    [SerializeField]
    int widthCount = 10;

    [SerializeField]
    int heightCount = 10;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float breakSpeed;

    [SerializeField]
    bool isRandomlyVelocity;

    [SerializeField]
    float multiplyRandoVelocityMin;

    [SerializeField]
    float multiplyRandoVelocityMax;

    [SerializeField]
    float deathTime = 0f;

    [SerializeField]
    float gravity = 0.98f;

    Vector3 breakStartPos;

    Dictionary<int, Vector3> verticalsPostions = new Dictionary<int, Vector3>();
    Mesh mesh;
    // Use this for initialization
    void Start()
    {
        SetCompornent();
        CreateMesh();
    }
    void SetCompornent()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color=(Color.red);
        Gizmos.DrawCube(transform.position,new Vector3(width,height,0.1f));
    }
    struct VerticalParam
    {
        public Vector3 pos;
        public Vector3 velocity;

    }

    List<Vector3> m_verticals = new List<Vector3>();
    void CreateMesh()
    {
        mesh = new Mesh();
        CreateDualVerticals();
      
        mesh.triangles = CreateDualTriangles();
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
    }

    void CreateDualVerticals()
    {
        var verticals = new Vector3[(widthCount - 1) * (heightCount - 1) * 6];
        var uvs=new Vector2[(widthCount - 1) * (heightCount - 1) * 6];
        var oneHeight = height / (heightCount - 1);
        var oneWidth = width / (widthCount - 1);
        int num = 0;
        for (int w = 0; w < widthCount - 1; w++)
        {
            for (int h = 0; h < heightCount - 1; h++)
            {
                var s = new Vector3(oneWidth * w-width*0.5f, oneHeight * h-height*0.5f, 0);

                //開始地点をキーに三角メッシュの中心点を入れる
                verticalsPostions.Add(num, s );
                verticalsPostions.Add(num + 3, s + new Vector3(0,oneHeight));

                var suv = new Vector2(s.x/width,s.y/height);
                var uvOneWidth = oneWidth / width;
                var uvOneHeight = oneHeight/ height;
                List<Vector2> uvList = new List<Vector2>()
                {
                    suv,
                    suv+new Vector2(0           ,uvOneHeight),
                    suv+new Vector2(uvOneWidth  ,0          ),
                    suv+new Vector2(0           ,uvOneHeight),
                    suv+new Vector2(uvOneWidth  ,uvOneHeight),
                    suv+new Vector2(uvOneWidth  ,0          ),
                };

                List<Vector3> triList = new List<Vector3>()
                {
                    s,
                    s+new Vector3(0,oneHeight),
                    s+new Vector3(oneWidth,0),
                    s+new Vector3(0,oneHeight),
                    s+new Vector3(oneWidth,oneHeight),
                    s+new Vector3(oneWidth,0),
                };
                for (int i=0;i<triList.Count;i++)
                {
                    verticals[num] = triList[i];
                    uvs[num] = uvList[i];
                    num++;
                }
            }
        }
        mesh.vertices= verticals;
        foreach (var i in mesh.vertices)
        {
            m_verticals.Add(i);
        }
        mesh.uv = uvs;
    }

    int[] CreateDualTriangles()
    {
        int[] tris = new int[m_verticals.Count];
        for (int i = 0; i < tris.Length; i++)
        {
            tris[i] = i;
        }
        return tris;
    }

    //Vector3[] CreateVerticals()
    //{
    //    var verticals = new Vector3[(widthCount + 1) * (heightCount + 1)];
    //    int num = 0;
    //    for (int w = 0; w <= widthCount; w++)
    //    {
    //        for (int h = 0; h <= heightCount; h++)
    //        {
    //            verticals[num] = new Vector3(width / (float)widthCount * w, -height / (float)heightCount * h, 0);
    //            Debug.Log(verticals[num]);
    //            num++;
    //        }
    //    }
    //    return verticals;
    //}


    int[] CreateTriangles()
    {
        int[] tris = new int[widthCount * heightCount * 6];
        int num = 0;
        for (int w = 0; w < widthCount; w++)
        {
            for (int h = 0; h < heightCount; h++)
            {
                var s = (heightCount + 1) * w + h;
                List<int> triList = new List<int>()
                {
                    s,
                    s+1,
                    s+heightCount+1,
                    s+1,
                    s+heightCount+2,
                    s+heightCount+1,
                };
                foreach (var i in triList)
                {
                    tris[num] = i;
                    num++;
                }
            }
        }
        return tris;
    }
    class BreakParam
    {
        public int idx;
        public Vector3 velocity;
        public BreakParam(int _idx,Vector3 _velocity)
        {
            idx = _idx;
            velocity = _velocity;
        }
    }
    List<BreakParam> breakingList = new List<BreakParam>();
    public void StartBreak()
    {
        Debug.Log("aaa");
        var centerIdx=widthCount* heightCount *3;
        breakStartPos = verticalsPostions[centerIdx];
        StartCoroutine(AddBreakingList(centerIdx));
        StartCoroutine(BreakMoveRoutine());
    }
    IEnumerator BreakMoveRoutine()
    {
        float timer = 0f;
        while (timer<deathTime)
        {
            for (int i = 0; i < breakingList.Count; i++)
            {
                for (int k=0;k<3;k++)
                {
                    m_verticals[breakingList[i].idx+k] -= breakingList[i].velocity*Time.deltaTime*moveSpeed;
                }
                breakingList[i].velocity *=(1.0f-(0.02f*Time.deltaTime));
                breakingList[i].velocity.y += gravity*Time.deltaTime;
            }
            meshFilter.mesh.SetVertices(m_verticals);
            timer += Time.deltaTime;
            yield return null;
        }
        Death();
    }
    Vector3 GetBreakDirection(Vector3 pos)
    {
        var dis = (pos - breakStartPos).magnitude;
        var  dir=(pos - breakStartPos).normalized;
        if (dir == Vector3.zero) dir = Vector3.up;
        dir *= (dis/(width/2.0f)*((isRandomlyVelocity)?Random.Range(multiplyRandoVelocityMin,multiplyRandoVelocityMax):1.0f));
        return -dir ;
    }
    IEnumerator AddBreakingList(int idx)
    {
        if (!verticalsPostions.ContainsKey(idx)) yield break;
        breakingList.Add(new BreakParam(idx, GetBreakDirection(verticalsPostions[idx])));
        verticalsPostions.Remove(idx);
        yield return new WaitForSeconds(0.1f*(1.0f/breakSpeed));
        var right = GetRight(idx);
        var left = GetLeft(idx);
        var up = GetUp(idx);
        var down = GetDown(idx);
        if(right!=-1)StartCoroutine(AddBreakingList(right));
        if(left!=-1)StartCoroutine(AddBreakingList(left));
        if(up!=-1)StartCoroutine(AddBreakingList(up));
        if(down!=-1)StartCoroutine(AddBreakingList(down));
    }

    int HeightVertCount
    {
        get { return (heightCount - 1) * 6; }
    }

    int GetRight(int idx)
    {
        return idx + HeightVertCount;
    }

    int GetLeft(int idx)
    {
        return idx - HeightVertCount;
    }

    int GetUp(int idx)
    {
        var c=idx /HeightVertCount;
        var upidx = idx - 3;
        if (c==(upidx/ HeightVertCount))
        {
            return upidx;
        }
        else
        {
            return -1;
        }
    }

    int GetDown(int idx)
    {
        var c = idx / HeightVertCount;
        var downidx = idx + 3;
        if (c == (downidx/ HeightVertCount))
        {
            return downidx;
        }
        else
        {
            return -1;
        }
    }
    IEnumerator WaveCoroutine()
    {
        float timer = 0f;
        while (true)
        {
            var o = Mathf.Sin(timer);
            int num = 0;
            for (int w = 0; w <= widthCount; w++)
            {
                for (int h = 0; h <= heightCount; h++)
                {
                    var vec = m_verticals[num];
                    m_verticals[num] = new Vector3(vec.x, vec.y, o * w);
                    num++;
                }
            }
            timer += Time.deltaTime;
            meshFilter.mesh.SetVertices(m_verticals);

            yield return null;
        }
    }
    IEnumerator ControlCoroutine()
    {

        m_verticals[0] += new Vector3(0, 0.1f * Time.deltaTime, 0);
        meshFilter.mesh.SetVertices(m_verticals);
        yield return null;
    }

    void Death()
    {
        Destroy(gameObject);
    }
}
