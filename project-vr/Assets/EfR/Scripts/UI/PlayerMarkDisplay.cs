using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkDisplay : MonoBehaviour
{

    [SerializeField]
    GameObject mark;

    [SerializeField]
    Vector2 correctVec=new Vector2(0.05f,0.95f);

    Camera targetCamera;
    Rect rect = new Rect(0, 0, 1, 1);
    private void Start()
    {
        this.GetGameObjectWithCoroutine(CameraUtility.CameraVRName,
            (GameObject go) => targetCamera = go.GetComponent<CameraVRController>().CenterCam);

    }
    // Update is called once per frame
    void Update()
    {
        DisplayMark();
    }
    void DisplayMark()
    {
        if (targetCamera && PlayerManager.LocalPlayer &&PlayerManager.playerMove.moveType==PlayerMove.MoveType.FIXED)
        {
            var pPos = PlayerManager.LocalPlayer.transform.position;
            var viewPos = targetCamera.WorldToViewportPoint(pPos);
            if (!rect.Contains(viewPos)|| viewPos.z < 0)
            {
                if (viewPos.z < 0)
                {
                    var sub = targetCamera.transform.position - pPos;
                    pPos += targetCamera.transform.forward * (Vector3.Dot(targetCamera.transform.forward, sub) + 5);
                }
                mark.SetActive(true);
                var viewPort = targetCamera.WorldToViewportPoint(pPos);
                if (rect.Contains(viewPort))
                {
                    viewPort.x = (viewPort.x>0.5)?1.0f:0f;
                }
                var correctedPos = new Vector2(Mathf.Clamp(viewPort.x, correctVec.x, correctVec.y), Mathf.Clamp(viewPort.y, correctVec.x, correctVec.y));
                mark.transform.position = targetCamera.ViewportToScreenPoint(correctedPos);
            }
            else
            {
                mark.SetActive(false);
            }
        }
    }

}
