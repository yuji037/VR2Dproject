using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBase : MonoBehaviour {

    private void OnTriggerEnter(Collider collider)
    {
        //collider.
        Execute();
    }

    protected virtual void Execute()
    {

    }
}
