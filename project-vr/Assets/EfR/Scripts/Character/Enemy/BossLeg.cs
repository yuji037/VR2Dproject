using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLeg : MonoBehaviour {

    [SerializeField]
    GameObject shockWavePrefab;

    [SerializeField]
    ParticleSystem damageSmoke;

    public System.Action deathAction;

    [SerializeField]
    Animator animator;

    Transform shockWavePoint;
    private void Start()
    {
       shockWavePoint=transform.Find("ShockWavePoint");    
    }
    public void GenerateShowkWave()
    {
        StartCoroutine(ShockWaveRoutine());
    }
    IEnumerator ShockWaveRoutine()
    {
        animator.CrossFade("Stamp", 0f);
        yield return new WaitForSeconds(1.1f);
        var obj=Instantiate(shockWavePrefab);
        obj.transform.position = shockWavePoint.position;
        obj.transform.rotation = Quaternion.identity;
    }
    public void Death()
    {
        damageSmoke.Play();
        if (deathAction != null) deathAction();
    }
}
