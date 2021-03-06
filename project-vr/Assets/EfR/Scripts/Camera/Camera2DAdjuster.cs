﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera2DAdjuster : MonoBehaviour
{
    Camera videoGameCamera;

    Vector3 pointLeftCenter;
    Vector3 pointCenter;

    [SerializeField]
    float depth = 1f;

    [SerializeField]
    RenderTexture targetTexture;

    Camera VRCamera;

    Camera2DController camera2DController;

    // Use this for initialization
    void Start()
    {
        videoGameCamera = GetComponent<Camera>();
        camera2DController = GetComponent<Camera2DController>();
    }

    public void SetVRCamera(Camera vrCamera)
    {
        VRCamera = vrCamera;
    }

    public void Trans2DPerspective()
    {
        StartCoroutine(Trans2DPerspectiveCoroutine());
    }

    public Vector3 Get2DCameraPos()
    {
        return transform.position;
    }

    IEnumerator Trans2DPerspectiveCoroutine()
    {
        yield return AdjustPerspective(true);
        videoGameCamera.transform.position = Get2DCameraPos();

    }

    public void Trans3DPerspective()
    {
        StartCoroutine(AdjustPerspective(false));
    }

    public void Set2DPosition()
    {
        transform.position = Get2DCameraPos();
    }

    Vector3 GetLeftCenterPoint()
    {
        return videoGameCamera.ScreenToWorldPoint(new Vector3(0, targetTexture.height * 0.5f, depth));
    }

    Vector3 GetCenterPoint()
    {
        return videoGameCamera.ScreenToWorldPoint(new Vector3(targetTexture.width * 0.5f, targetTexture.height * 0.5f, depth));
    }

    //Fovがデフォルト値になった時のCameraPosition
    //public Vector3 GetPositionForDefaultFOV()
    //{
    //    pointLeftCenter = GetLeftCenterPoint();
    //    pointCenter = GetCenterPoint();
    //    var z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * defaultFOV);
    //    return Get2DCameraPos() + new Vector3(0, 0, depth) - new Vector3(0, 0, z);
    //}
    //パースを調節する
    IEnumerator AdjustPerspective(bool toReal)
    {
        pointLeftCenter = GetLeftCenterPoint();
        pointCenter = GetCenterPoint();
        videoGameCamera.orthographic = false;
        var startCameraPos = PlayerManager.LocalPlayer.transform.position;
        var vrFov = VRCamera.fieldOfView;
        var startFOV = videoGameCamera.fieldOfView;

        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            float fovCoeff = t;
            if (toReal)
            {
                fovCoeff = 1.0f - t;
            }
            
            float fieldOfView = startFOV + (vrFov - 1) * fovCoeff;

            videoGameCamera.fieldOfView = fieldOfView;
            var z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * fieldOfView);
            var moveVec = -(new Vector3(0, 0, depth+z));
            moveVec = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveVec;
            //カメラの手前のオブジェクトを見えないようにする
            videoGameCamera.nearClipPlane = Mathf.Abs(moveVec.z)-5.0f;
            transform.position = startCameraPos + moveVec;


            yield return null;
        }
        videoGameCamera.nearClipPlane = 0.1f;
    }

}
