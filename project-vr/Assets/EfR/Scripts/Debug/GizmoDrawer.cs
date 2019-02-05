using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class GizmoDrawer : MonoBehaviour {
#if UNITY_EDITOR
    [MenuItem("デバッグツール/SwitchDrawGizmo")]
    public static void SwitchDrawGizmo()
    {
        isDrawing = !isDrawing;
    }
#endif
    static bool isDrawing=false;
    [System.Serializable]
    enum DrawType
    {
        Cube,
        Sphere
    }
    [SerializeField]
    Color GizmoColor;

    [SerializeField]
    DrawType drawType;

    private void OnDrawGizmos()
    {
        if (!isDrawing) return;
        Gizmos.color = GizmoColor;
        switch (drawType)
        {
            case DrawType.Cube:
                Gizmos.DrawCube(transform.position, transform.lossyScale);
                break;
            case DrawType.Sphere:
                Gizmos.DrawSphere(transform.position, transform.lossyScale.x);
                break;
        }
    }
}
