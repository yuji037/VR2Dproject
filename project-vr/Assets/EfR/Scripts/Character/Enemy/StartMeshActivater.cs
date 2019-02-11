using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMeshActivater : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(ActiveRoutine());
    }
    IEnumerator ActiveRoutine()
    {
        var skinMeshs = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var i in skinMeshs)
        {
            i.enabled = false;
        }
        yield return new WaitForSeconds(3.0f);
        foreach (var i in skinMeshs)
        {
            i.enabled = true;
        }
    }
}
