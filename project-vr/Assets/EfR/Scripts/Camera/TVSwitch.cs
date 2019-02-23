using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TVSwitch
{

    static GameObject TVMask;
    public static bool IsOn
    {
        get
        {
            if (!TVMask)
            {
                TVMask = GameObject.Find("TVMask");
                if (!TVMask) return false;
            }
            return !TVMask.activeSelf;
        }
        set
        {
            if (!TVMask)
            {
                TVMask = GameObject.Find("TVMask");
            }
            TVMask.SetActive(!value);
        }
    }
}
