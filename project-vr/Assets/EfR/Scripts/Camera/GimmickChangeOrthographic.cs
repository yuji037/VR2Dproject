using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickChangeOrthographic : GimmickBase{

    [SerializeField]
    bool isOrthographic;

    Camera camera2D;
    private void Start()
    {
        m_acTriggerEnterAction+=ChangeOrthographic;
        isCallOnlyServer = false;
        this.GetGameObjectWithCoroutine(CameraUtility.Camera2DName, (x) => camera2D= x.GetComponent<Camera>());
    }

    void ChangeOrthographic(Collider collider)
    {
        if (!camera2D) return;

        if (collider.gameObject == PlayerManager.LocalPlayer)
        {
            camera2D.orthographic = isOrthographic;
        }
    }
}
