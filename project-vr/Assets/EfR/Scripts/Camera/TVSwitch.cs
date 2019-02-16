using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TVSwitch
{

    static Camera _2DCamera;
    public static bool IsOn
    {
        get
        {
            if (!_2DCamera)
            {
                _2DCamera = GameObject.Find(CameraUtility.Camera2DName).GetComponent<Camera>();
                if (!_2DCamera) return false;
            }
            return _2DCamera.enabled;
        }
        set
        {
            if (!_2DCamera)
            {
                _2DCamera = GameObject.Find(CameraUtility.Camera2DName).GetComponent<Camera>();
            }
            if(_2DCamera)_2DCamera.enabled = value;
        }
    }
}
