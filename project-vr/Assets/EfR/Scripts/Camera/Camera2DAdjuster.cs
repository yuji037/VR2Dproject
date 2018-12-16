using System.Collections;
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


    float defaultFOV = 90.0f;


    // Use this for initialization
    void Start()
    {
        videoGameCamera = GetComponent<Camera>();
    }
    public void SetDefaultFov(float fov)
    {
        defaultFOV = fov;
    }


    public void Trans2DPerspective()
    {
        StartCoroutine(Trans2DPerspectiveCoroutine());
    }


    public Vector3 Get2DCameraPos()
    {
        return transform.position;
    }
    public void Move2DPosition(Transform target)
    {
        StartCoroutine(AdjustPosition(target,transform));
    }


    IEnumerator Trans2DPerspectiveCoroutine()
    {
        yield return AdjustPerspective(true);
        videoGameCamera.transform.position = Get2DCameraPos();
        videoGameCamera.orthographic = true;

    }

    public void Trans3DPerspective()
    {
        StartCoroutine(AdjustPerspective(false));
    }

    public void TransPosition(Transform endTransform)
    {
        StartCoroutine(AdjustPosition(transform, endTransform));
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
    public Vector3 GetPositionForDefaultFOV()
    {
        pointLeftCenter = GetLeftCenterPoint();
        pointCenter = GetCenterPoint();
        var z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * defaultFOV);
        return Get2DCameraPos() + new Vector3(0, 0, depth) - new Vector3(0, 0, z);
    }
    //パースを調節する
    IEnumerator AdjustPerspective(bool toReal)
    {
        pointLeftCenter = GetLeftCenterPoint();
        pointCenter = GetCenterPoint();
        videoGameCamera.orthographic = false;
        videoGameCamera.fieldOfView = defaultFOV;
        var startCameraPos = PlayerManager.LocalPlayer.transform.position;
        startCameraPos.y = Get2DCameraPos().y;
        Debug.Log(defaultFOV + "FOVで遷移");
        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            float fovCoeff = t;
            if (toReal)
            {
                fovCoeff = 1.0f - t;
            }

            float fieldOfView = 1f + (defaultFOV - 1) * fovCoeff;

            videoGameCamera.fieldOfView = fieldOfView;
            Debug.Log(fieldOfView);
            var z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * fieldOfView);
            var moveVec = new Vector3(0, 0, depth) + new Vector3(0, 0, z);
            moveVec = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveVec;
            transform.position = startCameraPos - moveVec;


            yield return null;
        }
    }
    IEnumerator AdjustPosition(Transform adjustTarget, Transform to)
    {
        Vector3 defPos = adjustTarget.position;
        Vector3 defForward = adjustTarget.forward;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            adjustTarget.position = defPos * (1f - t) / 1f + to.position * t / 1f;
            adjustTarget.forward = defForward * (1f - t) / 1f + to.forward * t / 1f;
            yield return null;
        }
    }
}
