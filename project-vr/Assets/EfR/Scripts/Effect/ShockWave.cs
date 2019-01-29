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
    }
    public void GenerateShowkWave()
    {
        particleSystem.Play();
    }
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

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Hit" + other);
        if (other == PlayerManager.LocalPlayer)
        {
            PlayerRespawner.GetInstance().RespawnLocalPlayer();
        }
    }
}
