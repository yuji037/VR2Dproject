﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class StoppingBossBehavior :  BossBehaviorBase{

    [SerializeField]
    ShockWaveBossSkill shockWave;

    //[SerializeField]
    //MeteoBossSkill meteo;

    [SerializeField]
    BeamBossSkill beem;

    [SerializeField]
    FireBallBossSkill fireBall;

    bool isPlaying;

    void StopBossBehavior()
    {
        isPlaying = false;
    }

    void StartBossBehavior()
    {
        isPlaying = true;
        StartCoroutine(BehaviorRoutine());
    }
    public void OnUpdate() { 
}

    IEnumerator BehaviorRoutine()
    {
        StartCoroutine(SkillRoutine(shockWave));
        StartCoroutine(SkillRoutine(fireBall));
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
        //StartCoroutine(SkillRoutine(meteo));
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

    public override void StartBehavior()
    {
        StartBossBehavior();
    }

    public override void StopBehavior()
    {
        StopBehavior();
    }
}
