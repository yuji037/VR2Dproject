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

	[SerializeField]
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null) { Debug.LogError("Can't get MeshRenderer component."); }

        var thisObject = meshRenderer.gameObject;
        if( !MaterialsManager.GetInstance().Add(thisObject) ) { Debug.LogError("Can't add list this object"); }
    }

    public void Change(PlayerMove.MoveType _moveType)
    {
        if (meshRenderer.materials.Length == 1) { return; }

        switch ( _moveType )
        {
            case PlayerMove.MoveType.FPS:
            case PlayerMove.MoveType.TPS:
                meshRenderer.material = meshRenderer.materials[(int)Elements._VR];
                break;

            case PlayerMove.MoveType._2D:
                meshRenderer.material = meshRenderer.materials[(int)Elements._2D];
                break;
        }
    }
}
