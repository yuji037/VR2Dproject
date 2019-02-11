using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BossBehavior :  NetworkBehaviour{

    [SerializeField]
    ShockWaveBossSkill shockWave;

    [SerializeField]
    MeteoBossSkill meteo;

    [SerializeField]
    BeamBossSkill beem;

    bool isPlaying;

    public void StopBehavior()
    {
        isPlaying = false;
    }

    public override void OnStartServer()
    {
        StartBossBehaviorOnServer();
    }

    public void StartBossBehaviorOnServer()
    {
        if (!isServer) return;
        isPlaying = true;
        StartCoroutine(BehaviorRoutine());
    }
    IEnumerator BehaviorRoutine()
    {
        StartCoroutine(SkillRoutine(shockWave));

        //片足が壊れた時
        yield return new WaitUntil(()=>
            {
                foreach (var i in shockWave.legs)
                {
                    if (i.IsBreak)
                    {
                        return true;
                    }
                }
                return false;
            }
        );

        StartCoroutine(SkillRoutine(meteo));
        StartCoroutine(SkillRoutine(beem));

    }

    IEnumerator SkillRoutine(BossSkillBase skillBase) {
        while (true)
        {
            var coolTime = (skillBase.param.isRandom)? Random.Range(skillBase.param.randomMinCoolTime,skillBase.param.randomMaxCoolTime):skillBase.param.coolTime;
            yield return new WaitForSeconds(coolTime);
            if(!isPlaying)yield break;
            skillBase.InvokeSkill();
        }
	}
}
