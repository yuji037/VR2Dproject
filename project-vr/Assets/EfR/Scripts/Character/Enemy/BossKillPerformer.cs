using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKillPerformer : MonoBehaviour {
    [SerializeField]
    ParticleSystem killEffect;

    [SerializeField]
    BossBehavior bossBehavior;

    [SerializeField]
    BlockTransitioner blockTransitioner;

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
        PlayerManager.playerStatus.RpcTransWorld(PlayerMove.MoveType.FIXED);
        StartCoroutine(DelayTransBlock());
    }
    IEnumerator DelayTransBlock()
    {
        yield return new WaitForSeconds(2.0f);
        blockTransitioner.StartTransBlocks();
    }
}
