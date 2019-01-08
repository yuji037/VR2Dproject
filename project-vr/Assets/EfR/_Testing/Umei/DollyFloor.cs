using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DollyFloor : MonoBehaviour {
    [SerializeField]
    CinemachineSmoothPath path;

    [SerializeField]
    float defaultPos;

    [SerializeField]
    float moveSpeed;
	// Use this for initialization
	void Start () {
        StartCoroutine(MoveCoroutine());
	}
    IEnumerator MoveCoroutine()
    {
        float currentValue =defaultPos;
        while(true)
        {
            transform.position=path.EvaluatePosition(currentValue);
            currentValue += Time.deltaTime*moveSpeed;
            yield return null;
        }
    }
}
