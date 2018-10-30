using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LineRendererOperation : MonoBehaviour {
    private LineRenderer lineRenderer;
    [SerializeField]
    int animationCount;
    [SerializeField]
    OculusInput oculus;
    private List<KeyValuePair<int, Vector3>> points = new List<KeyValuePair<int, Vector3>>();
    private Vector3 animationDistance;
    private Vector3 endPos;

    void EndPos(Vector3 _point) { endPos = _point; }
    public Vector3 EndPos() { return endPos; }

	// Use this for initialization
	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        endPos = Vector3.zero;
        if (animationCount == 0) { animationCount = 1; }
        lineRenderer.positionCount = animationCount;
	}

    public void LineSet(Vector3 _start, Vector3 _end)
    {
        EndPos(_start);
        points.Add(new KeyValuePair<int, Vector3>(0, _start));
        animationDistance.x = Mathf.Abs(_end.x - _start.x) / animationCount;
        animationDistance.y = Mathf.Abs(_end.y - _start.y) / animationCount;
        animationDistance.z = Mathf.Abs(_end.x - _start.z) / animationCount;
        for (int aCnt = 1; aCnt < animationCount; ++aCnt)
        {
            points.Add(new KeyValuePair<int, Vector3>(aCnt, endPos + animationDistance * aCnt));
        }
        lineRenderer.enabled = false;
    }

    public IEnumerator LineOpen()
    {
        for (int animation = 0; animation < animationCount; ++animation)
        {
            lineRenderer.SetPosition(points[animation].Key, points[animation].Value);
            yield return null;
        }
        lineRenderer.enabled = true;
        oculus.AnimationMode(ANIMATION.Hookking);
    }

    public IEnumerator LineClose()
    {
        Debug.Log("LineClose()");
        for (int animation = animationCount - 1; animation > 0; --animation)
        {
            Vector3 prevPoint = lineRenderer.GetPosition(animation - 1);
            lineRenderer.GetPosition(animation).Set(prevPoint.x, prevPoint.y, prevPoint.z);
            if (animation == 1) { lineRenderer.enabled = false; }
            yield return null;
        }
        oculus.AnimationMode(ANIMATION.None);
    }
}
