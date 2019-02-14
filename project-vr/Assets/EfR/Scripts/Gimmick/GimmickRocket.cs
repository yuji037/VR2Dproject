using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickRocket : GimmickBase {

    [SerializeField]
    float playerJumpPower;

    [SerializeField]
    public float moveSpeed;

    [SerializeField]
    HitNotifier top;

    [SerializeField]
    HitNotifier bot;

    [SerializeField]
    public float deathTime = 10.0f;

    float timer = 0f;
 
    // Use this for initialization
    void Start () {
        top.Initialize(Jump);
        bot.Initialize(PlayerRespawnAndSuicide);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        transform.Translate(transform.forward*moveSpeed*Time.deltaTime,Space.World);
        if (timer > deathTime)
        {
            DestroyThisObject();
        }
    }
    void Jump(Collider collider)
    {
        var pm = collider.GetComponent<PlayerMove>();
        if (pm)
        {
            Debug.Log("jump!");
            pm.FloatPlayer(playerJumpPower);
            DestroyThisObject();
        }
    }

    void PlayerRespawnAndSuicide(Collider collider)
    {
        var pm = collider.GetComponent<PlayerMove>();
        if (pm)
        {
            pm.RpcRespawn();
            DestroyThisObject();
        }
    }
	
}
