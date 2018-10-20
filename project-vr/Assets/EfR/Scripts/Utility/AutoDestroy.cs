using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

    public float waitTime = 3.0f;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, waitTime);
	}
    private void OnDestroy()
    {
        // レンダラのマテリアルを破棄(パーティクルシステムのレンダラも含まれる)
        var thisRenderer = this.GetComponent<Renderer>();
        if (thisRenderer != null && thisRenderer.materials != null)
        {
            foreach (var m in thisRenderer.materials)
            {
                DestroyImmediate(m);
            }
        }
    }
}
