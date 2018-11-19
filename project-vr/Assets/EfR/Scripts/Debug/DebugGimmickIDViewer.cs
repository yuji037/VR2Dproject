using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DebugGimmickIDViewer : MonoBehaviour
{
    [SerializeField]
    GimmickBase gimmickBase;
    [SerializeField]
    TextMesh textMesh;
    public void Init(GimmickBase gimmick,Color textColor)
    {
        gimmickBase = gimmick;
        transform.parent = gimmick.transform;
        transform.localPosition = Vector3.zero;
        textMesh.color = textColor;
    }
    bool isGame=false;
    private void Awake()
    {
        textMesh.text = "";
        isGame = true;
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!gimmickBase||isGame) return;
        textMesh.text = gimmickBase.GimmickID.ToString();
        if (Camera.current)
        {
            transform.LookAt(Camera.current.transform);
        }
    }
#endif

}
