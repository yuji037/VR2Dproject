using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SwitchableActive : MonoBehaviour,ISwitchableObject {

    [SerializeField]
    bool defaultActive;

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
        if (GetComponent<NetworkIdentity>())
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
