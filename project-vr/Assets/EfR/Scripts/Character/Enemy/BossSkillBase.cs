using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class BossSkillBase :  NetworkBehaviour{
    [System.Serializable]
    public struct SkillParam
    {
        public float coolTime;
        public bool isRandom;
        public float randomMinCoolTime;
        public float randomMaxCoolTime;
    }
    public SkillParam param;
    public abstract void InvokeSkill();
}
