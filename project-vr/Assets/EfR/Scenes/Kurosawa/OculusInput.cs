using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusInput : MonoBehaviour {
    [SerializeField]
    GameObject oculus;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawTouch.LIndexTrigger))
        {
            Debug.Log("OVRInput.RawTouch.LIndexTrigger");
        }
    }
}
