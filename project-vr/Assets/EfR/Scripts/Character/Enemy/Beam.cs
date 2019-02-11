using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

    [SerializeField]
    float delay = 1.0f;

    [SerializeField]
    Vector3 halfExtents;

    [SerializeField]
    Transform centerPos;

    private void OnDrawGizmos()
    {
        if (!centerPos) return;
        var preMat = Gizmos.matrix;
        //Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(centerPos.position, halfExtents*0.5f);
        //Gizmos.matrix = preMat;
    }

    // Use this for initialization
    void Start()
    {
        StartCoroutine(BeamRoutine());
    }

    IEnumerator BeamRoutine()
    {
        yield return new WaitForSeconds(delay);
        var colliders = Physics.OverlapBox(transform.position, halfExtents * transform.lossyScale.x,transform.rotation);
        foreach (var i in colliders)
        {
            if (i.gameObject == PlayerManager.LocalPlayer)
            {
                Debug.Log("HitBeam");
                PlayerRespawner.GetInstance().RespawnLocalPlayer();
            }
        }
        Destroy(gameObject);
    }
}
