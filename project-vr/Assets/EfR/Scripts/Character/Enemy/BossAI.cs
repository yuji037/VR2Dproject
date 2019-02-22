using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour{
    public BossBehaviorBase bossBehavior;
    bool isStarted;
    public void StartAI()
    {
        if (isStarted) return;
        bossBehavior.StartBehavior();
        
        isStarted = true;
    }
}
