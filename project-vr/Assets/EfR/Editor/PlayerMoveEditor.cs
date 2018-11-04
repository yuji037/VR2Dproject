using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerMove))]
public class PlayerMoveEditor : Editor {

    PlayerMove playerMove;

    private void OnEnable()
    {
        playerMove = (PlayerMove)target;
    }

    public override void OnInspectorGUI()
    {
        if ( GUILayout.Button("Show Settings") )
        {
            playerMove.LoadSettings();
            Selection.activeObject = playerMove.Pms;
        }

        base.OnInspectorGUI();
    }
}
