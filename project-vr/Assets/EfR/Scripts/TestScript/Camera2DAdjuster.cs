using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera2DAdjuster : MonoBehaviour
{
    Camera videoGameCamera;

    [SerializeField]
    Rect screenRect;

    Vector3 pointLeftCenter;
    Vector3 pointCenter;

    [SerializeField]
    float depth = 1f;

    Transform Target;

    Vector3 targetOffset;

    public Vector3 DefaultRot { private set; get; }

    float defaultFOV = 90.0f;
    // Use this for initialization
    void Start()
    {
        DefaultRot = transform.eulerAngles;
        videoGameCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (Target == null)
        {
            var localPlayer = PlayerManager.LocalPlayer;
            if (!localPlayer) return;
            Target = localPlayer.transform;
            var offset = transform.position - Target.transform.position;
            targetOffset = new Vector3(0, offset.y, -10.0f);
        }
    }
    public void Trans2DPerspective()
    {
        StartCoroutine(Trans2DPerspectiveCoroutine());
    }


    public Vector3 Get2DCameraPos()
    {
        return Target.transform.position + targetOffset;
    }
    public void Move2DPosition(Transform target)
    {
        StartCoroutine(AdjustPosition(target, Get2DCameraPos()));
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

    public void TransPosition(Transform endTransform, float duration)
    {
        StartCoroutine(AdjustPosition(transform, endTransform.position, duration));
    }

    public void Set2DPosition()
    {
        transform.position = Get2DCameraPos();
        transform.eulerAngles = DefaultRot;
    }
    //Fovがデフォルト値になった時のCameraPosition
    public Vector3 GetPositionForDefaultFOV()
    {
        pointLeftCenter = videoGameCamera.ScreenToWorldPoint(new Vector3(0/*Screen.width * 0.25f*/, Screen.height * 0.5f, depth));
        pointCenter = videoGameCamera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, depth));
        var z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * defaultFOV);
        return Get2DCameraPos() + new Vector3(0, 0, depth) - new Vector3(0, 0, z);
    }
    //パースを調節する
    IEnumerator AdjustPerspective(bool toReal)
    {
        pointLeftCenter = videoGameCamera.ScreenToWorldPoint(new Vector3(0/*Screen.width * 0.25f*/, Screen.height * 0.5f, depth));
        pointCenter = videoGameCamera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, depth));
        videoGameCamera.orthographic = false;
        videoGameCamera.fieldOfView = defaultFOV;

        //Debug.Log("Screen width : " + Screen.width);
        //Debug.Log("pointLeftTop : " + pointLeftCenter);
        //Debug.Log("pointCenter : " + pointCenter);
        for (float t = 0; t < 1.0f; t += Time.deltaTime)
        {
            float fovCoeff = t;
            if (toReal)
            {
                fovCoeff = 1.0f - t;
            }

            float fieldOfView = 1f + (defaultFOV - 1) * fovCoeff;

            videoGameCamera.fieldOfView = fieldOfView;

            var z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * fieldOfView);

            transform.position = Get2DCameraPos() + new Vector3(0, 0, depth) - new Vector3(0, 0, z);


            yield return null;
        }
    }

    IEnumerator AdjustPosition(Transform adjustTarget, Vector3 endPos, float duration = 1.0f)
    {
        Vector3 defPos = adjustTarget.position;
        //Vector3 defRot = adjustTarget.eulerAngles;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            adjustTarget.position = defPos * (duration - t) / duration + endPos * t / duration;
            //adjustTarget.eulerAngles = defRot * (duration - t) / duration + endRot * t / duration;
            yield return null;
        }
    }
}
