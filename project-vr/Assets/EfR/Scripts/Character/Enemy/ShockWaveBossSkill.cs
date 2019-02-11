using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShockWaveBossSkill : BossSkillBase
{
    [SerializeField]
    public BossLeg[] legs;

    int currentLegNum=0;
    public override void InvokeSkill()
    {
        RecursiveStartStamp(1,legs.Length);
    }

    void UpdateLegNum()
    {
        currentLegNum++;
        if (currentLegNum>=legs.Length)
        {
            currentLegNum = 0;
        }
    }

    bool RecursiveStartStamp(int currentRecursive,int maxRecursive)
    {
        if (currentRecursive > maxRecursive)return false;

        UpdateLegNum();

        if (legs[currentLegNum].IsBreak)
        {
            Debug.Log(currentLegNum);

            return RecursiveStartStamp(currentRecursive+1,maxRecursive);
        }
        else
        {
            legs[currentLegNum].RpcStartStamp();
            return true;
        }
    }
}
