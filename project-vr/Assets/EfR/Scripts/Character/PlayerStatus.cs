using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

// "ゲーム世界にいる"プレイヤーの情報
public class PlayerStatus : NetworkBehaviour {

    [SerializeField]
    GameObject m_prefVRHand;
	[SerializeField]
	float m_fFadeHeight = 1.5f;

    bool initialized = false;
    public bool Initialized
    {
        get { return initialized; }
    }
    public PlayerNumber Number { get; private set; }

    bool active=true;
    public bool Active
    {
        get { return active;}
    }

	public bool IsPerforming = false;

    //Stageが変わるとStageInit()が呼び出されるまでfalse
    public bool isReady = false;

    [ClientRpc]
    public void RpcInit(int number)
    {
        this.Number = (PlayerNumber)number;
        initialized = true;
        PlayerManager.Players[number] = gameObject;
        SetPlayerMaterial(number,gameObject);
    }
    void SetPlayerMaterial(int number,GameObject playerObject)
    {
        var mat=Resources.Load<Material>("Material/"+(number+1)+"playerMat");
        var mesh=playerObject.GetComponentInChildren<SkinnedMeshRenderer>();
        mesh.material = mat;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!isLocalPlayer&&!PlayerManager.Players[0])
        {
            PlayerManager.Players[0] = gameObject;
            var stageSettingObj = GameObject.Find("StageSettings");
            bool settingActive= stageSettingObj.GetComponent<StageSettings>().playerActiveOnStart[0];
            SetActiveOnLocal(settingActive);
        }
    }
    public override void OnStartLocalPlayer()
    {
        if ( isLocalPlayer )
        {
            Debug.Log("Set LocalPlayer");
            PlayerManager.SetLocalPlayer(this.gameObject);
		}
	}

    private void Start()
    {
        Debug.Log("preConID"+playerControllerId);
		gameObject.name = PlayerManager.GetPlayerName(playerControllerId);
        //StageInit();
    }

    public void StageInit()
    {
        SetPlayerPopPosition();
		GetComponent<PlayerMove>().SetFixedPosition(transform.position);
    }

    public void RendererSwitchForPlayerMoveType(PlayerMove.MoveType moveType)
    {
        var renderers = GetComponentsInChildren<Renderer>();

        switch ( moveType )
        {
            case PlayerMove.MoveType.FPS:
                foreach ( var ren in renderers )
                {
                    ren.enabled = false;
                }
                break;

            case PlayerMove.MoveType.TPS:
            case PlayerMove.MoveType._2D:
            case PlayerMove.MoveType.FIXED:
                foreach ( var ren in renderers )
                {
                    ren.enabled = true;
                }
                break;
        }
    }

    void SetPlayerPopPosition()
    {
        if ( !isLocalPlayer ) return;

		int posNum = playerControllerId + 1;

		var trPopPos = GameObject.Find("PlayerPopPos" + posNum.ToString());

        if ( trPopPos ) {
            Debug.Log(trPopPos.transform.position);
            transform.position = trPopPos.transform.position;
            transform.rotation = trPopPos.transform.rotation;
        }
        else
        {
            Debug.LogWarning("PlayerPopPos" + posNum.ToString() + "が見つかりません");
        }

        PlayerRespawner.GetInstance().SaveLocalPlayerRespawnPoint(transform.position);
    }

    [Command]
    public void CmdSetActive(bool isActive)
    {
        RpcSetActive(isActive);
    }
    [ClientRpc]
    public void RpcSetActive(bool isActive)
    {
        SetActiveOnLocal(isActive);
        isReady = true;
    } 
    void SetActiveOnLocal(bool isActive)
    {
        Debug.Log(gameObject.name + "のアクティブを" + isActive);
        var children = GetComponentsInChildren<Renderer>();
        foreach (var c in children)
        {
            c.enabled = isActive;
        }
        active = isActive;
        
    }

    [Command]
    public void CmdTransWorld(PlayerMove.MoveType transMoveTypeTo)
    {
        RpcTransWorld(transMoveTypeTo);
    }

	// 他端末のプレイヤーに影響する場合は[ClientRpc]を使う
	// [ClientRpc]：すべてのクライアントの関数をリモートで実行
	[ClientRpc]
	public void RpcTransWorld(PlayerMove.MoveType transMoveTypeTo)
	{
        StartCoroutine(VRCharaHoloController.GetInstance().VRChatCharaFade((int)Number,
            transMoveTypeTo == PlayerMove.MoveType._2D));
        StageMaterialChanger.GetInstance().ChangeMaterial((int)Number,transMoveTypeTo);
		if ( !hasAuthority ) return;
        TransWorld(transMoveTypeTo);
		
	}

    public void TransWorld(PlayerMove.MoveType transMoveTypeTo)
    {
        var pm = GetComponent<PlayerMove>();
        Debug.Log(pm.gameObject.name + " : " + pm.moveType + " → " + transMoveTypeTo);

        //既に移行したいMoveTypeだった場合か移行中の場合return
        if (transMoveTypeTo == pm.moveType || ViewSwitchPerformer.GetInstance().IsTranslation) return;
        Debug.Log("移行!!");
        pm.SwitchMoveType(transMoveTypeTo);
        pm.canMove = false;
        ViewSwitchPerformer.GetInstance().SwitchView(transMoveTypeTo, () =>
		{
			pm.canMove = true;
			pm.SetFixedPosition(pm.transform.position);
		});
	}

	[Command]
	public void CmdHoloFade(bool fadeIn)
	{
		RpcHoloFade(fadeIn);
	}

	[ClientRpc]
	public void RpcHoloFade(bool fadeIn)
	{
		StartCoroutine(HoloFadeCoroutine(fadeIn));
	}

	IEnumerator HoloFadeCoroutine(bool fadeIn)
	{
		var renderer = GetComponentInChildren<SkinnedMeshRenderer>();
		var wPosY = renderer.transform.position.y;
		var holoMat = renderer.material;
		var speed = 1.0f;
		holoMat.SetFloat("_App", 1);

		for ( float t = 0; t <= m_fFadeHeight; t += Time.deltaTime * speed )
		{
			holoMat.SetFloat("_Pos", wPosY + ( fadeIn ? t : m_fFadeHeight - t ));
			yield return null;
		}
		if(fadeIn) holoMat.SetFloat("_App", 0);

		holoMat.SetFloat("_Pos", wPosY + ( fadeIn ? 1000 : -1000 ));
		IsPerforming = false;
	}
}
