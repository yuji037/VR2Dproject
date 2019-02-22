using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class BossBehaviorBase :  NetworkBehaviour{

    public abstract void StartBehavior();

    public abstract void StopBehavior();
}
