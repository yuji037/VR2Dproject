using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLeg : MonoBehaviour {

    [SerializeField]
    ShockWave shockWave;

    [SerializeField]
    ParticleSystem damageSmoke;

    public System.Action deathAction;

    [SerializeField]
    Animator animator;

    public void GenerateShowkWave()
    {
        StartCoroutine(ShockWaveRoutine());
        }
    IEnumerator ShockWaveRoutine()
    {
        animator.CrossFade("Stamp", 0f);
        yield return new WaitForSeconds(1.0f);
        shockWave.GenerateShowkWave();
    }
    public void Death()
    {
        damageSmoke.Play();
        if (deathAction != null) deathAction();
    }
}
