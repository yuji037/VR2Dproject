using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Networking;

public class DollyMoveObject : NetworkBehaviour{
    [SerializeField]
    public CinemachineSmoothPath path;

    [SerializeField]
    float moveSpeed;

    public float currentPathValue;

    [SerializeField]
    public bool autoMove;

    [SerializeField]
    bool selfTurn;

    bool isLoop;

    [SerializeField]
    PlayerNumber hasAuthorityPlayerNumber;

    int moveDirection = 1;

    bool initialized;

    void Start()
    {
        currentPathValue = path.FindClosestPoint(transform.position, Mathf.FloorToInt(0), 2, 5);
        currentPathValue= path.GetPathDistanceFromPosition(currentPathValue);
        transform.position = path.EvaluatePositionAtUnit(currentPathValue, CinemachinePathBase.PositionUnits.Distance);
        isLoop = path.Looped;
    }
    bool IsReady()
    {
        return PlayerManager.LocalPlayer &&PlayerManager.playerStatus;
    }
    private void Update()
    {
        if (autoMove)
        {
            Move(1.0f);
        }
    }
    [Command]
    void CmdSyncDollyPos(Vector3 next)
    {
        var target = PlayerManager.Players[((int)hasAuthorityPlayerNumber==0)?1:0].GetComponent<NetworkIdentity>().connectionToClient;
        TargetSyncNextPos(target, next);
    }

    [TargetRpc]
    void TargetSyncNextPos(NetworkConnection target, Vector3 next)
    {
        transform.position=next;
    }

    void Initialize()
    {
        if (initialized) return;
        if (hasAuthorityPlayerNumber != (PlayerNumber)PlayerManager.GetPlayerNumber()) return;
        var netId = GetComponent<NetworkIdentity>();
        if (netId.localPlayerAuthority) PlayerManager.playerStatus.SetAuth(netId);
        initialized = true;
    }

    public void Move(float multiPlySpeed)
    {
        if (!IsReady()) return;
        Initialize();
        if (hasAuthority)
        {
            //セルフターンが有効な場合向きを逆転させる
            if (selfTurn)
            {
                if (path.MaxUnit(CinemachinePathBase.PositionUnits.Distance) <= currentPathValue)
                {
                    moveDirection = -1;
                }
                else if (currentPathValue <= 0)
                {
                    moveDirection = 1;
                }
            }
            currentPathValue += moveSpeed * multiPlySpeed * Time.deltaTime*moveDirection;
            if (!isLoop)
            {
                currentPathValue =Mathf.Clamp(currentPathValue,0f, path.MaxUnit(CinemachinePathBase.PositionUnits.Distance));
            }
            var next = path.EvaluatePositionAtUnit(currentPathValue,CinemachinePathBase.PositionUnits.Distance);
            transform.position = next;

            if(PlayerManager.OtherPlayer)CmdSyncDollyPos(next);
        }
    }


}
