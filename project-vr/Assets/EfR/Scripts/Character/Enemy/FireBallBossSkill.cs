using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireBallBossSkill : BossSkillBase
{
    [SerializeField]
    GameObject fireBallPrefab;

    [SerializeField]
    Cinemachine.CinemachineSmoothPath[] path;

    public override void InvokeSkill()
    {
        var num = Random.Range(0,path.Length);
        var obj = Instantiate(fireBallPrefab);
        obj.GetComponent<DollyMoveObject>().path = path[num];
        NetworkServer.Spawn(obj);
    }
}
