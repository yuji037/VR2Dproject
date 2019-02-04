using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
enum CameraType
{
    Camera2D,
    CameraVR,
}

public static class CameraUtility {
	public static string Camera2DName
    {
        get
        {
            return "Camera2D";
        }
    }

    public static string CameraVRName
    {
        get
        {
            return "OculusCameraRig(Clone)";
        }
    }
}
