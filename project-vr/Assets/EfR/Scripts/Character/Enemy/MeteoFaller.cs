using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class MeteoFaller :  NetworkBehaviour{
    [System.Serializable]
    struct FallParam
    {
        public Vector2 fallRange;
        public Transform fallCenter;
        [Range(0,1.0f)]
        public float cubeRate;
    }
    [SerializeField]
    FallParam[] fallParams;

    [SerializeField]
    GameObject meteoPrefab;

    [SerializeField]
    GameObject meteoCubePrefab;

    private void OnDrawGizmos()
    {
        foreach (var f in fallParams)
        {
        Gizmos.DrawWireCube(f.fallCenter.position, new Vector3(f.fallRange.x, 0, f.fallRange.y));
        }
    }

    Vector3 GetRandomlyFallPos(FallParam fallParam)
    {
        return fallParam.fallCenter.position + new Vector3(Random.Range(-fallParam.fallRange.x * 0.5f, fallParam.fallRange.x * 0.5f), 0, Random.Range(-fallParam.fallRange.y * 0.5f, fallParam.fallRange.y * 0.5f));
    }

    public void FallMeteo()
    {
        var arrayNum = Random.Range(0, fallParams.Length);
        var pos =(GetRandomlyFallPos(fallParams[arrayNum]));
        var isCube = Random.Range(0, 1.0f)< fallParams[arrayNum].cubeRate;
        if (isCube)
        {
            var obj = Instantiate(meteoCubePrefab);
            //ちょっと浮かす
            pos.y += 3.0f;
            obj.transform.position = pos;
            NetworkServer.Spawn(obj);
        }
        else
        {
            RpcFallMeteo(pos);
        }

        Debug.Log("メテオ");
    }
    [ClientRpc]
    void RpcFallMeteo(Vector3 fallPos)
    {
        Instantiate(meteoPrefab);
        meteoPrefab.transform.position = fallPos;
    }
}
