using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DebugGimmickIDViewer : MonoBehaviour
{
    [SerializeField]
    IActor actor;
    [SerializeField]
    TextMesh textMesh;
    public void Init(IActor actor,Color textColor)
    {
        this.actor = actor;
        transform.parent = (actor as MonoBehaviour).transform;
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
        if (actor==null||isGame) return;
        textMesh.text = actor.GetID().ToString();
        if (Camera.current)
        {
            transform.LookAt(Camera.current.transform);
        }
    }
#endif

}
