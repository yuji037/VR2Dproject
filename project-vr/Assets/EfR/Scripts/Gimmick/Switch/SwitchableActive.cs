using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchableActive : MonoBehaviour,ISwitchableObject {

    [SerializeField]
    bool defaultActive;

    [SerializeField]
    bool isNetworkObject;

    public void Start()
    {
        SetActive(defaultActive);
    }

    public void OnAction()
    {
        SetActive(!defaultActive);
    }

    public void OffAction()
    {
        SetActive(defaultActive);
    }

    void SetActive(bool active)
    {
        if (isNetworkObject)
        {
            SetNetworkActive(active);
        }
        else
        {
            gameObject.SetActive(active);
        }
    }

    void SetNetworkActive(bool active)
    {
        foreach (var c in gameObject.GetComponentsInChildren<Collider>())
        {
            c.enabled = active;
        }
        foreach (var c in gameObject.GetComponentsInChildren<Renderer>())
        {
            c.enabled = active;
        }
    }

}
