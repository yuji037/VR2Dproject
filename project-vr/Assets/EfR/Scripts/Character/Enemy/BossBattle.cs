using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : SingletonMonoBehaviour<BossBattle> {
    [SerializeField]
    List<BossLeg> legs;

    float attackInterval=3.0f;

    bool wasStart;

    private void Start()
    {
        foreach(var leg in legs)
        {
            leg.deathAction += () => LegOnDied(leg);
        }
    }

    public void StartBossBattle()
    {
        if (wasStart) return;
        wasStart = true;
        StartCoroutine(BehaviourRoutine());
    }

    void LegOnDied(BossLeg leg)
    {
        legs.Remove(leg);
        attackInterval -= 0.5f;
    }
    // Update is called once per frame
	IEnumerator BehaviourRoutine()
    {

        while (legs.Count >0)
        {
            var num=Random.Range(0,legs.Count);
            legs[num].GenerateShowkWave();
            yield return new WaitForSeconds(attackInterval);
        }
        
        BossBattleEnd();
    }
    [SerializeField]
    ParticleSystem breakParticle;

    [SerializeField]
    BlockTransitioner blockTransitioner;

    void BossBattleEnd()
    {
        breakParticle.Play();
        gameObject.SetActive(false);
        blockTransitioner.StartTransBlocks();
        Debug.Log("BOSSBATTLEEND");
    }

}
