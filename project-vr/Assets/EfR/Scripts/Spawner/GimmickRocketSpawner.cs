using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickRocketSpawner : NetworkBehaviour{
    [SerializeField]
    GameObject rocketPrefab;

    [SerializeField]
    float spawnCoolTime=3.0f;

    [SerializeField]
    float rocketMoveSpeed = 5.0f;

    public override void OnStartServer() {
        StartCoroutine(SpawnCoroutine());
	}
	IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Spawn();    
            yield return new WaitForSeconds(spawnCoolTime);
        }
    }
    void Spawn()
    {
        Debug.Log("spawn");
        var obj = Instantiate(rocketPrefab);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.GetComponent<GimmickRocket>().moveSpeed = rocketMoveSpeed;
        NetworkServer.Spawn(obj);
    }
}
