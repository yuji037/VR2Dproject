using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoorLinkPathViewer : MonoBehaviour {
    ParticleSystem particle;

    // Use this for initialization
	void Start () {

        var switchActionBase = GetComponent<SwitchActionBase>();
        var gimmickSwitch = GetComponent<GimmickSwitch>();
        var particleAttractor = GetComponentInChildren<particleAttractorSpherical>();
        if (!(gimmickSwitch && switchActionBase && particleAttractor))
        {
            Debug.Log("指定コンポーネントが無いので自殺");
            Destroy(this);
            return;
        }
        particle = particleAttractor.transform.parent.GetComponent<ParticleSystem>();

        gimmickSwitch.m_aPointerHitAction += (x) => PlayLinkEffect();
        gimmickSwitch.m_aPointerExitAction+= (x) => StopLinkEffect();

        var particleTargets = switchActionBase.ActorObjects;
        if (particleTargets!=null)
        {
            particleAttractor.endPoint= particleTargets[0];
        }
    }

    void PlayLinkEffect()
    {
        Debug.Log("再生");
        particle.Play();
    }

    void StopLinkEffect()
    {
        Debug.Log("停止");
        particle.Stop();
    }
}
