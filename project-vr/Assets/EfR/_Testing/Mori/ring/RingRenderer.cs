using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public struct Ring
{
  public Vector3 pos;

  public Quaternion rotate;

  public Vector3 scale;

  public float innerPercentage;

  public float fanAngle;

  public Color color;
}

[ExecuteInEditMode()]
public class RingRenderer : MonoBehaviour
{
  public Shader shader;

  Material material;

  ComputeBuffer buffer;

  List<Ring> rings;

  void Awake()
  {
    if (material == null)
    {
      material = new Material(shader);
      material.hideFlags = HideFlags.DontSave;
    }
    if (buffer == null)
      buffer = new ComputeBuffer(10000, Marshal.SizeOf(typeof(Ring)));
    if (rings == null)
      rings = new List<Ring>();

  }

  void OnDisable()
  {
    if (buffer != null)
      buffer.Dispose();
    buffer = null;
  }

  void Update()
  {
    Awake();
    rings.Clear();
  }

  void OnRenderObject()
  {

    Awake();
    material.SetPass(0);

    material.SetBuffer("Rings", buffer);

    buffer.SetData(rings.ToArray());
    Graphics.DrawProcedural(MeshTopology.Points, rings.Count);
  }

  public void Push(Ring ring)
  {
    rings.Add(ring);
  }

}