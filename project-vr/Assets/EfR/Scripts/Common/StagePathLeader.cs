using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagePathLeader : MonoBehaviour {
    Cinemachine.CinemachineSmoothPath path;
    [SerializeField]
    ParticleSystem leadParticle;

    [SerializeField]
    float speed;

    float prevPathPos;

    bool isLeading = false;
    // Use this for initialization
	void Start () {
        path = GetComponent<Cinemachine.CinemachineSmoothPath>();
        DebugTools.RegisterDebugAction(KeyCode.U, CreateLeadEffect, "導蟲");
    }
    public void CreateLeadEffect(Vector3 originPos)
    {
        if (isLeading) return;
        StartCoroutine(LeadRoutine(PlayerManager.LocalPlayer.transform.position));
    }
    public void CreateLeadEffect()
    {
        if (isLeading) return;
        StartCoroutine(LeadRoutine(PlayerManager.LocalPlayer.transform.position));
    }

    IEnumerator LeadRoutine(Vector3 originPos)
    {
        leadParticle.Play();
        isLeading = true;
        float t = 0f;
        var startPos= path.FindClosestPoint(originPos, Mathf.FloorToInt(prevPathPos), 2, 5);
        var endPos = path.MaxPos;
        var leadingPathPos = 0f;
        while (endPos > leadingPathPos)
        {
            t += Time.deltaTime*speed;
            leadingPathPos = startPos + t;

            var pos=path.EvaluatePosition(leadingPathPos);
            leadParticle.transform.position = pos;
            yield return null;
        }
        isLeading = false;
        leadParticle.Stop();
    }
}
