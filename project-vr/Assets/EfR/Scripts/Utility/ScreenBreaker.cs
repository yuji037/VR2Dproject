using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ScreenBreaker : MonoBehaviour
{

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [SerializeField]
    Vector3 start;

    [SerializeField]
    float height;

    [SerializeField]
    float width;

    Vector3 rightBottomPos;
    [SerializeField]
    int widthCount = 10;

    [SerializeField]
    int heightCount = 10;

    Vector3 breakStartPos;

    Dictionary<int, Vector3> verticalsPostions = new Dictionary<int, Vector3>();
    // Use this for initialization
    void Start()
    {
        SetCompornent();
        CreateMesh();
        var vec = new Vector3(Random.Range(0,width), -Random.Range(0, height), 0);
        Debug.Log("BreakVec="+vec);
        Break(vec);
        //StartCoroutine(WaveCoroutine());
    }
    void SetCompornent()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    Mesh mesh;

    struct VerticalParam
    {
        public Vector3 pos;
        public Vector3 velocity;

    }

    List<Vector3> verticals = new List<Vector3>();
    void CreateMesh()
    {
        mesh = new Mesh();
        mesh.vertices = CreateDualVerticals();
        foreach (var i in mesh.vertices)
        {
            verticals.Add(i);
        }
        mesh.triangles = CreateDualTriangles();
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;

    }

    Vector3[] CreateDualVerticals()
    {
        var verticals = new Vector3[(widthCount - 1) * (heightCount - 1) * 6];
        var oneHeight = height / (heightCount - 1);
        var oneWidth = width / (widthCount - 1);
        int num = 0;
        for (int w = 0; w < widthCount - 1; w++)
        {
            for (int h = 0; h < heightCount - 1; h++)
            {
                var s = new Vector3(oneWidth * w, oneHeight * h, 0);

                //開始地点をキーに三角メッシュの中心点を入れる
                verticalsPostions.Add(num, s );
                verticalsPostions.Add(num + 3, s + new Vector3(0,oneHeight));

                List<Vector3> triList = new List<Vector3>()
                {
                    s,
                    s+new Vector3(0,oneHeight),
                    s+new Vector3(oneWidth,0),
                    s+new Vector3(0,oneHeight),
                    s+new Vector3(oneWidth,oneHeight),
                    s+new Vector3(oneWidth,0),
                };
                foreach (var i in triList)
                {
                    verticals[num] = i;
                    num++;
                }
            }
        }
        return verticals;
    }
    int[] CreateDualTriangles()
    {
        int[] tris = new int[verticals.Count];
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
    struct BreakParam
    {
        public int idx;
        public Vector3 firstVelocity;
        public BreakParam(int _idx,Vector3 _firstVelocity)
        {
            idx = _idx;
            firstVelocity = _firstVelocity;
        }
    }
    List<BreakParam> breakingList = new List<BreakParam>();
    void Break(Vector3 pos)
    {
        //var near = verticalsPostions.FirstOrDefault(x => 0.1f > Vector3.Distance(x.Value, pos));
        //var firstIdx = near.Key;
        var centerIdx=widthCount* heightCount *3;
        breakStartPos = verticalsPostions[centerIdx];
        StartCoroutine(AddBreakingList(centerIdx));
        StartCoroutine(BreakMoveRoutine());
    }
    IEnumerator BreakMoveRoutine()
    {
        while (true)
        {
            for (int i = 0; i < breakingList.Count; i++)
            {
                verticals[breakingList[i].idx] -= new Vector3(0, 0.5f * Time.deltaTime, -1.0f * Time.deltaTime);
                verticals[breakingList[i].idx+1] -= new Vector3(0, 0.5f * Time.deltaTime, -1.0f * Time.deltaTime);
                verticals[breakingList[i].idx+2] -= new Vector3(0, 0.5f * Time.deltaTime, -1.0f * Time.deltaTime);
            }
            meshFilter.mesh.SetVertices(verticals);
            yield return null;
        }
    }
    Vector3 GetBreakDirection(Vector3 pos)
    {
        var  dir=(pos - breakStartPos).normalized;
        return dir ;
    }
    IEnumerator AddBreakingList(int idx)
    {
        if (!verticalsPostions.ContainsKey(idx)) yield break;
        Debug.Log("Break="+idx);
        breakingList.Add(new BreakParam(idx, GetBreakDirection(verticalsPostions[idx])));
        verticalsPostions.Remove(idx);
        yield return new WaitForSeconds(0.1f);
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
                    var vec = verticals[num];
                    verticals[num] = new Vector3(vec.x, vec.y, o * w);
                    num++;
                }
            }
            timer += Time.deltaTime;
            meshFilter.mesh.SetVertices(verticals);

            yield return null;
        }
    }
    IEnumerator ControlCoroutine()
    {

        verticals[0] += new Vector3(0, 0.1f * Time.deltaTime, 0);
        meshFilter.mesh.SetVertices(verticals);
        yield return null;
    }
}
