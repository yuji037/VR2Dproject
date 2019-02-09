using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickRocketParts : MonoBehaviour{

    System.Action<Collider> hitAction;
    public void Initialize(System.Action<Collider> hitAction)
    {
        this.hitAction+= hitAction;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(hitAction!=null)hitAction(other);
    }
}
