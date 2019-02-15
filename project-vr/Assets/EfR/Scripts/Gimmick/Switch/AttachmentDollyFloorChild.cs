using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentDollyFloorChild : MonoBehaviour
{
    [SerializeField]
    DollyMoveObject target;

    Vector3 preTargetPos;
    private void Awake()
    {
        preTargetPos = target.transform.position;
    }
    private void Update()
    {
        if (target.isServer&&target.StartedServer)
        {
            transform.parent = target.transform;
            var sub=target.transform.position - preTargetPos;
            transform.Translate(sub);
            Debug.Log(sub);
            enabled = false;
        }
    }

}
