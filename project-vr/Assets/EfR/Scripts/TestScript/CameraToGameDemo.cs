using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToGameDemo : MonoBehaviour {

    [SerializeField]
    Vector3 startPos;

    [SerializeField]
    Vector3 endPos;

    [SerializeField]
    Camera realCam;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update () {
        if ( Input.GetKeyDown(KeyCode.G) )
        {
            StartCoroutine(Coroutine());
        }
	}

    IEnumerator Coroutine()
    {
        float dur = 2f;
        startPos = transform.position;
        var pp = realCam.GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        pp.enabled = true;
        for(float t = 0; t < dur; t += Time.deltaTime )
        {
            transform.position = startPos * ( dur - t ) / dur + endPos * t / dur;
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        var gameCamObj = GameObject.Find("GameCamera");
        transform.position = gameCamObj.transform.position;
        transform.eulerAngles = gameCamObj.transform.eulerAngles /*+ realCam.transform.eulerAngles*/;

        realCam.cullingMask = gameCamObj.GetComponent<Camera>().cullingMask;
        realCam.clearFlags = CameraClearFlags.Skybox;
        var skyCamObj = GameObject.Find("SkyboxCamera").GetComponent<Camera>();
        skyCamObj.targetTexture = null;
        pp.enabled = false;
        GetComponent<ControlCamera>().enabled = true;
    }
}
