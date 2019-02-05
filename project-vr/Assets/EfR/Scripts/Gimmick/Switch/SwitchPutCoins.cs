using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SwitchPutCoins : SwitchActionBase {
    [SerializeField]
    Transform[] putPositions;

    [SerializeField]
    GameObject coinPrefab;

    int playerGotCoinCount;

    List<GameObject> coins=new List<GameObject>();

    [SerializeField]
    float coinDuration = 20.0f;

    [SerializeField]
    TextMesh timerText;

    float timer = 0f;

    [SyncVar]
    bool isOn;

    bool isGotCoins;

    private void Update()
    {
        if (isGotCoins) return;
        if (isOn)
        {
            timer += Time.deltaTime;
            timerText.text = (int)(coinDuration - timer)+"";
            if (isServer&&coinDuration<=timer)
            {
                GetComponent<GimmickButton>().UnPush();
            }
        }
    }

    public override void OnAction()
    {
        if (isGotCoins) return;
        isOn = true;
        playerGotCoinCount = 0;
        foreach (var i in putPositions)
        {
            var coinObj = SpawnCoin();
            coinObj.transform.position = i.transform.position;
            coins.Add(coinObj);
        }
    }
   
    public override void OffAction()
    {
        if (isGotCoins) return;
        isOn = false;
        timer = 0f;
        DestroyAllCoin();
    }

    void DestroyAllCoin()
    {
        foreach (var c in coins)
        {
            if(c)c.GetComponent<GimmickCoin>().DestroyThisObject();
        }
        coins.Clear();
    }

    GameObject SpawnCoin()
    {
        var obj = Instantiate(coinPrefab);
        obj.transform.position = new Vector3(0, -1000.0f, 0);
        NetworkServer.Spawn(obj);
        obj.GetComponent<GimmickCoin>().SetTriggerEnterAction(CountUp);
        return obj;
    }
    void CountUp()
    {
        if (isGotCoins) return;
        playerGotCoinCount++;
        if (playerGotCoinCount>=putPositions.Length)
        {
            CmdGotCoins();
        }
    }
    [Command]
    void CmdGotCoins()
    {
        RpcGotCoins();
    }

    [SerializeField]
    GameObject screen;
    [ClientRpc]
    void RpcGotCoins()
    {
        isGotCoins = true;
        timerText.text = "";
        screen.SetActive(false);
        if (PlayerManager.playerMove.moveType == PlayerMove.MoveType._2D)
        {
            PlayerManager.playerStatus.TransWorld(PlayerMove.MoveType.FIXED);
        }
    }
}
