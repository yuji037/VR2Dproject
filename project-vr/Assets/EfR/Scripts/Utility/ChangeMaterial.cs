using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChangeMaterial : NetworkBehaviour {

    private enum Elements
    {
        _VR,
        _2D,
    }

    private new MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        if (renderer == null) { Debug.LogError("Can't get MeshRenderer component."); }

        var thisObject = renderer.gameObject;
        if( !MaterialsManager.GetInstance().Add(thisObject) ) { Debug.LogError("Can't add list this object"); }
    }

    public void Change(PlayerMove.MoveType _moveType)
    {
        if (renderer.materials.Length == 1) { return; }

        switch ( _moveType )
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
                renderer.material = renderer.materials[(int)Elements._VR];
                break;

            case PlayerMove.MoveType._2D:
                renderer.material = renderer.materials[(int)Elements._2D];
                break;
        }
    }
}
