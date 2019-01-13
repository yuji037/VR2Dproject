using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwitchActionBase :MonoBehaviour {
  
   

    public virtual void OnAction() { }

    public virtual void OffAction() { }

}
