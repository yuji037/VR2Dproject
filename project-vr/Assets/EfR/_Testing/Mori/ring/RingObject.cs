using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class RingObject : MonoBehaviour
{
[SerializeField]
  RingRenderer ringRenderer;

  [Range(0, 1)]
  public float innerPercentage = 0.8f;

  [Range(0, Mathf.PI * 2 + 0.01f)]
  public float fanAngle = 1.5f;

  public Color color = Color.black;

  void LateUpdate()
  {
    PushToRenderer();
  }

  void PushToRenderer()
  {
    var ring = new Ring();

    ring.pos = transform.position;
    ring.rotate = transform.rotation;
    ring.scale = transform.localScale;
    ring.innerPercentage = innerPercentage;
    ring.fanAngle = fanAngle;
    ring.color = color;

    ringRenderer.Push(ring);
  }
}