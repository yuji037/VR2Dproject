using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DController : MonoBehaviour {

    GameObject _targetObj;
    GameObject targetObject
    {
        get
        {
            if ( !_targetObj )
                _targetObj = PlayerManager.LocalPlayer;
            return _targetObj;
        }
    }

	[SerializeField]
	Vector3 offset;

    // Update is called once per frame
    void Update () {
        if ( !targetObject ) return;


        Vector3 move = targetObject.transform.position - transform.position + offset;
        move.z = 0;

        transform.position += move;
	}
}
