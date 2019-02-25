using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchLinkPathViewer : MonoBehaviour {
    ParticleSystem particle;
    particleAttractorSpherical particleAttractor;
    GimmickSwitch gimmickSwitch;

    bool initilaized = false;
    private void Start()
    {
        gimmickSwitch = GetComponent<GimmickSwitch>();
        particleAttractor = GetComponentInChildren<particleAttractorSpherical>();
        if (!(gimmickSwitch && particleAttractor))
        {
            Debug.Log("指定コンポーネントが無いので自殺");
            Destroy(this);
            return;
        }
        particle = particleAttractor.transform.parent.GetComponent<ParticleSystem>();

        gimmickSwitch.m_aPointerHitAction += (x) => PlayLinkEffect();
        gimmickSwitch.m_aPointerExitAction += (x) => StopLinkEffect();

    }
    void Initlaize()
    {
        if (initilaized) return;

        var particleTargets = gimmickSwitch.ActorSwitchableObjects;
        Debug.Log(particleTargets[0]);
        if (particleTargets != null)
        {
            particleAttractor.endPoint = (particleTargets[0] as MonoBehaviour).transform;
        }
        initilaized = true;
    }

    void PlayLinkEffect()
    {
        Debug.Log("再生");
        Initlaize();
        particle.Play();
    }

    void StopLinkEffect()
    {
        Debug.Log("停止");
        Initlaize();
        particle.Stop();
    }
}
