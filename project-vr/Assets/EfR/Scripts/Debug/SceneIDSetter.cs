using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SceneIDSetter : MonoBehaviour{
    //IDは被らないようにする
    [SerializeField]
    int sceneID;
    // Use this for initialization
    public void SetForceSceneID() {
        var netIdentity = GetComponent<NetworkIdentity>();
        Debug.Log(this.name + "シーンID変更: " + netIdentity.sceneId + " To " + sceneID);
        netIdentity.ForceSceneId(sceneID);
    }


}
