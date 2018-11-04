using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2Dto3Dtest : MonoBehaviour {

    [SerializeField]
    Camera cam;

    [SerializeField]
    Rect screenRect;

    Vector3 pointLeftTop;
    Vector3 pointCenter;

    [SerializeField]
    float depth = 1f;

    Vector3 defaultPos;

    [SerializeField]
    Vector3 cam3Dpos;
    [SerializeField]
    Vector3 cam3Drot;

    // Use this for initialization
    void Start () {
        //Application.targetFrameRate = 10;
        //defaultPos = transform.position;
        //defaultPos += new Vector3(0, 0, depth);
        //pointLeftTop = cam.ScreenToWorldPoint(new Vector3(0/*Screen.width * 0.25f*/, Screen.height * 0.5f, depth));
        //pointCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, depth));
        //StartCoroutine(coroutine());
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.G) )
        {
            defaultPos = transform.position;
            defaultPos += new Vector3(0, 0, depth);
            pointLeftTop = cam.ScreenToWorldPoint(new Vector3(0/*Screen.width * 0.25f*/, Screen.height * 0.5f, depth));
            pointCenter = cam.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, depth));
            StartCoroutine(coroutine());
        }
    }

    IEnumerator coroutine()
    {
        yield return new WaitForSeconds(1f);

        cam.orthographic = false;
        cam.fieldOfView = 60f;
        Debug.Log("Screen width : " + Screen.width);
        float z = depth;

        Debug.Log("pointLeftTop : " + pointLeftTop);
        Debug.Log("pointCenter : " + pointCenter);

        for ( float t = 0; t < 1f; t += Time.deltaTime )
        {
            // 1f → 60f
            float fieldOfView = 
                1f * ( 1f - t ) / 1f 
                + 60f * t / 1f;
            cam.fieldOfView = fieldOfView;

            z = Vector3.Magnitude(pointLeftTop - pointCenter) / Mathf.Tan(Mathf.Deg2Rad * fieldOfView);

            transform.position = defaultPos - new Vector3(0, 0, z);

            yield return null;
        }

        //yield return new WaitForSeconds(1f);

        //cam.orthographic = true;
        StartCoroutine(posCoroutine());
    }

    IEnumerator posCoroutine()
    {
        Vector3 defPos = transform.position;
        Vector3 defRot = transform.eulerAngles;
        for(float t = 0; t < 1; t += Time.deltaTime )
        {
            transform.position = defPos * ( 1f - t ) / 1f + cam3Dpos * t / 1f;
            transform.eulerAngles = defRot * ( 1f - t ) / 1f + cam3Drot * t / 1f;

            yield return null;
        }
        GetComponent<Camera2Dto3Dtest>().enabled = true;
    }
}
