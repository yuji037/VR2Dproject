using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKillPerformer : MonoBehaviour {
    [SerializeField]
    ParticleSystem killEffect;

    [SerializeField]
    BossBehavior bossBehavior; 

	public void Kill()
    {
        killEffect.Play();
        bossBehavior.StopBehavior();
        bossBehavior.gameObject.SetActive(false);
        var meshRenderers=GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshRenderers)
        {
            mesh.enabled = false;
        }

        var skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (var mesh in skinnedMeshRenderers)
        {
            mesh.enabled = false;
        }
    }
}
