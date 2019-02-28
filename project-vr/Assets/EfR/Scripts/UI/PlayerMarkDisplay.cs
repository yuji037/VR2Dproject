using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkDisplay : MonoBehaviour
{
    [SerializeField]
    Camera UICamera;

    [SerializeField]
    GameObject mark;

    [SerializeField]
    GameObject yazirusi;

    [SerializeField]
    Vector2 correctVec=new Vector2(0.05f,0.95f);

    Camera targetCamera;

    Rect rect = new Rect(0, 0, 1, 1);

    Vector2 preViewPort;
    private void Start()
    {
        mark.SetActive(false);
        this.GetGameObjectWithCoroutine(CameraUtility.CameraVRName,
            (GameObject go) => {
                targetCamera = go.GetComponent<CameraVRController>().CenterCam;
                UICamera.transform.parent = targetCamera.transform;
            });

    }
    // Update is called once per frame
    void Update()
    {
        DisplayMark();
    }
    void DisplayMark()
    {
        if (targetCamera && PlayerManager.LocalPlayer &&
            PlayerManager.playerMove.moveType==PlayerMove.MoveType.FIXED&&
            ViewSwitchPerformer .CheckInstance&&
            PlayerManager.playerMove.canMove)
        {
            var pPos = PlayerManager.LocalPlayer.transform.position;
            var viewPos = targetCamera.WorldToViewportPoint(pPos);
            if ((!rect.Contains(viewPos)|| viewPos.z < 0)&&
                !ViewSwitchPerformer.GetInstance().IsTranslation)
            {
                if (viewPos.z < 0)
                {
                    var sub = targetCamera.transform.position - pPos;
                    pPos += targetCamera.transform.forward * (Vector3.Dot(targetCamera.transform.forward, sub) + 5);
                }
                mark.SetActive(true);
                yazirusi.SetActive(true);
                var viewPort = targetCamera.WorldToViewportPoint(pPos);
                if (rect.Contains(viewPort))
                {
                    viewPort.x = (viewPort.x>0.5)?1.0f:0f;
                }
                var correctedViewPos = new Vector2(Mathf.Clamp(viewPort.x, correctVec.x, correctVec.y), Mathf.Clamp(viewPort.y, correctVec.x, correctVec.y));
                Vector2 markPos = targetCamera.ViewportToScreenPoint(correctedViewPos);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(),markPos,UICamera,out markPos);
                if ((preViewPort - correctedViewPos).magnitude > 0.5f)
                {
                    mark.transform.localPosition = markPos;
                }
                else
                {
                    mark.transform.localPosition = Vector3.Lerp(mark.transform.localPosition,markPos,0.1f);
                }
                Vector2 pScreenPos=targetCamera.ViewportToScreenPoint(viewPort);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), pScreenPos, UICamera, out pScreenPos);
                Debug.Log("ppos:"+pScreenPos+"correctedPos:"+markPos);
                yazirusi.transform.localRotation=Quaternion.FromToRotation(markPos,pScreenPos);
                preViewPort = correctedViewPos;
            }
            else
            {
                mark.SetActive(false);
                yazirusi.SetActive(false);
            }
        }
    }

}
