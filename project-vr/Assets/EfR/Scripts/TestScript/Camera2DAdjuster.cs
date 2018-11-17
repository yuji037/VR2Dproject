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

    // Use this for initialization
    void Start()
    {
        DefaultRot = transform.eulerAngles;
    }
    private void Update()
    {
        if (Target == null)
        {
            Target = PlayerManager.LocalPlayer.transform;
            targetOffset = transform.position - Target.transform.position;
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
        StartCoroutine(AdjustPosition(target, Get2DCameraPos(), DefaultRot));
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
        StartCoroutine(AdjustPosition(transform, endTransform.position, endTransform.eulerAngles));
    }

    public void Set2DPosition()
    {
        transform.position = Get2DCameraPos();
        transform.eulerAngles = DefaultRot;
    }

    //パースを調節する
    IEnumerator AdjustPerspective(bool toReal)
    {
        pointLeftCenter = videoGameCamera.ScreenToWorldPoint(new Vector3(0/*Screen.width * 0.25f*/, Screen.height * 0.5f, depth));
        pointCenter = videoGameCamera.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, depth));
        videoGameCamera.orthographic = false;
        videoGameCamera.fieldOfView = 60f;
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

            videoGameCamera.fieldOfView = fieldOfView;

            z = Vector3.Magnitude(pointLeftCenter - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * fieldOfView);

            transform.position = Get2DCameraPos() + new Vector3(0, 0, depth) - new Vector3(0, 0, z);

            yield return null;
        }
    }

    IEnumerator AdjustPosition(Transform adjustTarget, Vector3 endPos, Vector3 endRot)
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
