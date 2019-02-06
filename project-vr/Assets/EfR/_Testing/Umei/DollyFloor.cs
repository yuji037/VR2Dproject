﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Networking;

public class DollyFloor : NetworkBehaviour{
    [SerializeField]
    public CinemachineSmoothPath path;

    [SerializeField]
    float defaultPathValue;

    [SerializeField]
    float moveSpeed;

    float currentPathValue;

    [SerializeField]
    bool autoMove;

    public bool StartedServer { get; private set; }

    public override void OnStartServer()
    {
        currentPathValue = defaultPathValue;
        StartedServer = true;
        Move(1.0f);
    }

    private void Update()
    {
        if (autoMove)
        {
            Move(1.0f);
        }
    }
    public void Move(float multiPlySpeed)
    {
        if (StartedServer && isServer)
        {
            currentPathValue += moveSpeed * multiPlySpeed * Time.deltaTime;
            var next = path.EvaluatePosition(currentPathValue);
            transform.position = next;
        }
    }
}
