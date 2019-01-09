using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GimmickRollableBall : GimmickBase{

    Vector3 rollVector;

    Rigidbody rigidbody;

    [SerializeField]
    float rollPower;

    [SerializeField]
    int triggerID=1;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rollVector.Normalize();
        m_acTriggerEnterAction += StartRoll;
        GetComponent<SavableObject>().respawnAction += () => { rigidbody.velocity = Vector3.zero; rigidbody.angularVelocity = Vector3.zero; };
    }
    void StartRoll(Collider collider)
    {
        var gimmick = collider.GetComponent<GimmickBase>();
        if (gimmick&&gimmick.GimmickID==triggerID)
        {
            var gimPos = gimmick.transform.position;
            gimPos.y = 0f;
            var myPos = transform.position;
            myPos.y = 0f;
            rollVector = (myPos - gimPos).normalized;
            rigidbody.AddForce(rollVector*rollPower);
        }
    }

}
