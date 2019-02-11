using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteo : MonoBehaviour {
    [SerializeField]
    float delay=1.0f;

    [SerializeField]
    float radius;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,radius*transform.lossyScale.x);
    }

    // Use this for initialization
    void Start () {
        StartCoroutine(MeteoRoutine());
	}

    IEnumerator MeteoRoutine()
    {
        yield return new WaitForSeconds(delay);
        var colliders=Physics.OverlapSphere(transform.position,radius * transform.lossyScale.x);
        foreach (var i in colliders)
        {
            if (i.gameObject==PlayerManager.LocalPlayer)
            {
                Debug.Log("HitMeteo");
                PlayerRespawner.GetInstance().RespawnLocalPlayer();
            }
        }
        Destroy(gameObject);
    }

}
