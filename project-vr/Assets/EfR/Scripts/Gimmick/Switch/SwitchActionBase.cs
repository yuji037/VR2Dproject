using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class SwitchActionBase :NetworkBehaviour{
 

    public virtual void OnAction() { }

    public virtual void OffAction() { }

}
