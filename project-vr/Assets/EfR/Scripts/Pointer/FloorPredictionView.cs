using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPredictionView : MonoBehaviour {
    BoxInfo m_boxColliderInfo;
    SpriteRenderer m_spriteRenderer;

    [SerializeField]
    GameObject cantCreateFloorPredictionObject;

    // Use this for initialization
	void Start () {
        m_boxColliderInfo = GetComponent<BoxInfo>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ActiveView(Vector3 hitPos,PointerHitScreen pointerHitScreen,bool canCreate)
    {
        transform.position = hitPos;
        transform.rotation = pointerHitScreen.transform.rotation;
        if (!canCreate)
        {
            cantCreateFloorPredictionObject.SetActive(true);
            m_spriteRenderer.enabled = false;
            return;
        }
        m_spriteRenderer.enabled = true;
        cantCreateFloorPredictionObject.SetActive(false);

        Vector2 boxSize = new Vector2(1,1);
        Vector3 boxPos = Vector3.zero;
        //screenBox
        var sBox = pointerHitScreen.GetComponent<BoxInfo>();
        var fBox = m_boxColliderInfo;

        var pivot = pointerHitScreen.transform.position;

        var srDis = (sBox.RightPoint - pivot).magnitude;
        var srVec = (sBox.RightPoint - pivot).normalized;
        var frDis = Vector3.Dot(srVec, (fBox.RightPoint - pivot));
        var rSub = (frDis - srDis) / transform.lossyScale.x;
        if (rSub > 0)
        {
            boxSize.x = 1 - rSub;
            boxPos = new Vector3((-rSub / 2)*transform.lossyScale.x, boxPos.y, boxPos.z);
        }


        var slDis = (sBox.LeftPoint - pivot).magnitude;
        var slVec = (sBox.LeftPoint - pivot).normalized;
        var flDis = Vector3.Dot(slVec, (fBox.LeftPoint - pivot));
        var lSub = (flDis - slDis) / transform.lossyScale.x;
        if (lSub > 0)
        {
            boxSize.x = 1 - lSub;
            boxPos = new Vector3((lSub / 2) * transform.lossyScale.x, boxPos.y, boxPos.z);
        }

        var stDis = (sBox.TopPoint - pivot).magnitude;
        var stVec = (sBox.TopPoint - pivot).normalized;
        var ftDis = Vector3.Dot(stVec, (fBox.TopPoint - pivot));
        var tSub = (ftDis - stDis) / transform.lossyScale.y;
        if (tSub > 0)
        {
            boxSize.y = 1- tSub;
            boxPos = new Vector3(boxPos.x,( -tSub / 2) * transform.lossyScale.y, boxPos.z);
        }

        var sbDis = (sBox.BottomPoint - pivot).magnitude;
        var sbVec = (sBox.BottomPoint - pivot).normalized;
        var fbDis = Vector3.Dot(sbVec, (fBox.BottomPoint - pivot));
        var bSub = (fbDis - sbDis) / transform.lossyScale.y;
        if (bSub > 0)
        {
            boxSize.y = 1 - bSub;
            boxPos = new Vector3(boxPos.x, (bSub / 2) * transform.lossyScale.y, boxPos.z);
        }
        m_spriteRenderer.size= boxSize;
        transform.position += boxPos;
        return;
    }
}
