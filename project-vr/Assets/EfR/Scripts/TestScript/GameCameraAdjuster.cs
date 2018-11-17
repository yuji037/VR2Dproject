using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCameraAdjuster : MonoBehaviour
{

    [SerializeField]
    Camera cam;

    [SerializeField]
    Rect screenRect;

    Vector3 pointLeftCenter;
    Vector3 pointCenter;

    [SerializeField]
    float depth = 1f;

    Vector3 defaultRot;

    GameObject rc;
    GameObject RealCamera
    {
        get
        {
            if (!rc) rc = VRObjectManager.GetInstance().VRCamObject;
            return rc;
        }
    }

    [SerializeField]
    Transform target;

    [SerializeField]
    Vector3 targetOffset2D;
    [SerializeField]
    Vector3 Rot2D;

    [SerializeField]
    Vector3 targetOffsetTPS;
    [SerializeField]
    Vector3 RotTPS;


    // Use this for initialization
    void Start()
    {
        defaultRot = transform.eulerAngles;
    }

    public void VirtualToReal()
    {
        StartCoroutine(VirtualToRealC());
    }
    //
    public Vector3 Get2DCameraPos()
    {
        return target.transform.position + targetOffset2D;
    }

    IEnumerator VirtualToRealC()
    {
        var endPos = Get2DCameraPos();
        StartCoroutine(AdjustPosition(transform, endPos, Rot2D));
        yield return AdjustPosition(RealCamera.transform, endPos, Rot2D);
        yield return AdjustPerse(true);
        cam.transform.position = endPos;
        cam.orthographic = true;

    }

    public void RealToVirtual()
    {
        StartCoroutine(RealToVirtualC());

    }
    IEnumerator RealToVirtualC()
    {
        yield return AdjustPerse(false);
        var endPos = target.transform.position + targetOffsetTPS;
        yield return AdjustPosition(transform, endPos, RotTPS);
    }
    //パースを調節する
    IEnumerator AdjustPerse(bool toReal)
    {
        pointLeftCenter = cam.ScreenToWorldPoint(new Vector3(0/*Screen.width * 0.25f*/, Screen.height * 0.5f, depth));
        pointCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, depth));
        cam.orthographic = false;
        cam.fieldOfView = 60f;
        Debug.Log("Screen width : " + Screen.width);
        float z = depth;

        Debug.Log("pointLeftTop : " + pointLeftCenter);
        Debug.Log("pointCenter : " + pointCenter);
        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            float fovCoeff = t;
            if (toReal)
            {
                fovCoeff = 1.0f - t;
            }
          
            float fieldOfView = 1f + 59f * fovCoeff;

            cam.fieldOfView = fieldOfView;

            z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * fieldOfView);

            transform.position = Get2DCameraPos() + new Vector3(0, 0, depth) - new Vector3(0, 0, z);

            yield return null;
        }
    }

    IEnumerator AdjustPosition(Transform adjustTarget,Vector3 endPos,Vector3 endRot)
    {
        Vector3 defPos = adjustTarget.position;
        Vector3 defRot = adjustTarget.eulerAngles;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            adjustTarget.position = defPos * (1f - t) / 1f + endPos * t / 1f;
            adjustTarget.eulerAngles = defRot * (1f - t) / 1f + endRot * t / 1f;
            yield return null;
        }
    }
}
