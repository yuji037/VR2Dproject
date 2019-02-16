using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDoorLinkPathViewer : MonoBehaviour {
    SwitchDoorOpen switchDoorOpen;
    ParticleSystem particle;

    Transform door;
    // Use this for initialization
	void Start () {

        switchDoorOpen = GetComponent<SwitchDoorOpen>();
        var gimmickSwitch = GetComponent<GimmickSwitch>();
        var particleAttractor = GetComponentInChildren<particleAttractorSpherical>();
        if (!(gimmickSwitch && switchDoorOpen && particleAttractor))
        {
            Debug.Log("指定コンポーネントが無いので自殺");
            Destroy(this);
            return;
        }
        particle = particleAttractor.transform.parent.GetComponent<ParticleSystem>();

        gimmickSwitch.m_aPointerHitAction += (x) => PlayLinkEffect();
        gimmickSwitch.m_aPointerExitAction+= (x) => StopLinkEffect();

        var gimmick = GimmickManager.GetGimmick(switchDoorOpen.IActorGimmickID);
        if (gimmick)
        {
            door = gimmick.transform;
        }
        particleAttractor.endPoint=door;
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
