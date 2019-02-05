using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    ParticleSystem particleSystem;
    // Use this for initialization
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
    }

    //public void GenerateShowkWave()
    //{
    //    particleSystem.Play();
    //    //StartCoroutine(ShockWaveCoroutine());
    //}

    //IEnumerator ShockWaveCoroutine()
    //{
    //    particleSystem.Play();
    //    //particleSystem.GetComponent<Renderer>().enabled = false;
    //    //　MaxParticlesを超えるパーティクルを生成するまでシミュレーションスピードを上げる
    //    var main = particleSystem.main;
    //    main.simulationSpeed = 10f;
    //    var a = particleSystem.limitVelocityOverLifetime;
    //    a.space = 0f;
    //    while (true)
    //    {
    //        //Debug.Log(particleSystem.particleCount+"/"+particleSystem.main.maxParticles);
    //        if (particleSystem.particleCount>=particleSystem.main.maxParticles)
    //        {
    //            Debug.Log("発射");
    //            main.simulationSpeed = 1f;
    //            //particleSystem.GetComponent<Renderer>().enabled = true;
    //            break;
    //        }
    //        yield return null;
    //    }
    //}
    //IEnumerator ShockWaveCoroutine()
    //{
    //    var main = particleSystem.main;
    //    main.simulationSpeed = 10f;
    //    particleSystem.GetComponent<Renderer>().enabled = false;
    //    main.startSpeed = 0f;
    //    while (true)
    //    {
    //        if (particleSystem.particleCount >= particleSystem.main.maxParticles)
    //        {
    //            main.simulationSpeed = 1f;
    //            particleSystem.GetComponent<Renderer>().enabled = true;
    //            main.startSpeed = 1.0f;
    //        }
    //        yield return null;
    //    }
    //}

    //private void onparticletrigger()
    //{
    //    list<particlesystem.particle> enters = new list<particlesystem.particle>();
    //    int numenter = particlesystem.gettriggerparticles(particlesystemtriggereventtype.enter, enters);
    //    if (numenter != 0)
    //    {
    //        playerrespawner.getinstance().respawnlocalplayer();
    //    }
    //}
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit" + other);
        if (other == PlayerManager.LocalPlayer)
        {
            PlayerRespawner.GetInstance().RespawnLocalPlayer();
        }
    }
}
