using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Networkmanager : MonoBehaviour
{

    public GameObject objectPrefab;
    string ip = "127.0.0.1";
    string port = "1192";
    bool connected = false;

    private void CreatePlayer()
    {
        connected = true;
        Network.Instantiate(objectPrefab, objectPrefab.transform.position, objectPrefab.transform.rotation, 1);
    }

    public void OnConnectedToServer()
    {
        connected = true;
    }

    public void OnServerInitialized()
    {
        CreatePlayer();
    }


    public void OnGUI()
    {
        if (!connected)
        {
            if (GUI.Button(new Rect(10, 10, 90, 90), "Client"))
            {
                Network.Connect(ip, int.Parse(port));
            }
            if (GUI.Button(new Rect(10, 110, 90, 90), "Master"))
            {
                Network.InitializeServer(10, int.Parse(port), false);
            }
        }
    }
    
}
