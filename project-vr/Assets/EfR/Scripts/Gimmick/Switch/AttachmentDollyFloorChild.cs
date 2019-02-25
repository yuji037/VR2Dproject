using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentDollyFloorChild : MonoBehaviour
{
    [SerializeField]
    DollyMoveObject target;

    Vector3 diffToTarget;

    float delay = 0.5f;
    float timer = 0f;

    private void Awake()
    {
        diffToTarget= target.transform.position-transform.position;
        Debug.Log("差は"+diffToTarget+"です");
    }
    private void Update()
    {
        timer+= Time.deltaTime;
        if (delay <= timer)
        {
            transform.position=target.transform.position-diffToTarget;
            transform.parent = target.transform;
            enabled = false;
        }
    }

}
