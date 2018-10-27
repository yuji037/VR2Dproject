using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class LineRendererOperation : MonoBehaviour {


    private LineRenderer lineRenderer;
    [SerializeField]
    int animationCount;
    private Vector3 animationDistance;

    [SerializeField]
    GameObject player;
    private Vector3 endPos;



    
    public void EndPos(Vector3 _point) { endPos = _point; }
    public Vector3 EndPos() { return endPos; }

	// Use this for initialization
	void Start () {
        endPos = Vector3.zero;
	}

    public IEnumerator LineAnimationSet(Vector3 _start, Vector3 _end)
    {
        if (_end == Vector3.zero) { yield return null; }
        animationDistance = (_end - endPos) / animationCount;
    }
}
