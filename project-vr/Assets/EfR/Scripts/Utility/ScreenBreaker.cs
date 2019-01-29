using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start()
    {
        SetCompornent();
        CreateMesh();
        StartCoroutine(WaveCoroutine());
    }
    void SetCompornent()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    Mesh mesh;
    List<Vector3> verticals = new List<Vector3>();
    void CreateMesh()
    {
        mesh = new Mesh();
        mesh.vertices = CreateVerticals();
        mesh.triangles = CreateTriangles();
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
        foreach (var i in mesh.vertices)
        {
            verticals.Add(i);
        }
    }
    Vector3[] CreateVerticals()
    {
        var verticals = new Vector3[(widthCount + 1) * (heightCount + 1)];
        int num = 0;
        for (int w = 0; w <= widthCount; w++)
        {
            for (int h = 0; h <= heightCount; h++)
            {
                verticals[num] = new Vector3(width / (float)widthCount * w, -height / (float)heightCount * h, 0);
                Debug.Log(verticals[num]);
                num++;
            }
        }
        return verticals;
    }
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
    IEnumerator WaveCoroutine()
    {
        float timer = 0f;
        while (true)
        {
            var o=Mathf.Sin(timer);
            int num = 0;
            for (int w = 0; w <= widthCount; w++)
            {
                for (int h = 0; h <= heightCount; h++)
                {
                    var vec = verticals[num];
                    verticals[num] = new Vector3(vec.x, vec.y, o*w);
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
