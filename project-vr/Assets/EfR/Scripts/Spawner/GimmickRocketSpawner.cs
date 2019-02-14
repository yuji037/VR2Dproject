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

    [SerializeField]
    float rocketDeathTime;

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
        var obj = Instantiate(rocketPrefab);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        var rocket = obj.GetComponent<GimmickRocket>();
        rocket.moveSpeed = rocketMoveSpeed;
        rocket.deathTime = rocketDeathTime;
        NetworkServer.Spawn(obj);
    }
}
