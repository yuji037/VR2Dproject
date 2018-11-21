using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

// "ゲーム世界にいる"プレイヤーの情報
public class PlayerStatus : NetworkBehaviour {

	[SerializeField]
	GameObject m_prefVRHand;


	public override void OnStartLocalPlayer()
    {
        if ( isLocalPlayer )
        {
            Debug.Log("Set LocalPlayer");
            PlayerManager.SetLocalPlayer(this.gameObject);
			//GameObject.Find("Server").GetComponent<EFRNetworkServer>().RegisterPlayer(this.gameObject);
			//VRObjectManager.GetInstance().OnNetworkConnected();
			//CmdSpawnHand();
		}
	}

	[Command]
	public void CmdSpawnHand()
	{
		var VRCamObject = VRObjectManager.GetInstance().VRCamObject;
		// 2Pからも1Pの手が見えるように、ネットワーク対応
		var handObjTransforms = VRCamObject.GetComponentsInChildren<Transform>()
								.Where(tr => tr.gameObject.name.Contains("HandRig")).ToArray();

		foreach ( var obj in handObjTransforms )
		{
			//var _parent = obj.transform.parent;
			//obj.transform.parent = null;
			//NetworkServer.Spawn(obj.gameObject);
			//obj.transform.parent = _parent;

			// ※サーバーでスポーンするオブジェクトの親が非アクティブだと
			// 上手くスポーンしないらしい？
			var trackHand = Instantiate(m_prefVRHand);
			//trackHand.transform.parent = GameObject.Find("VRCamParent").transform;
			trackHand.transform.position = obj.transform.position;
			trackHand.transform.rotation = obj.transform.rotation;
			trackHand.GetComponent<TrackingTransform>().trackTransform = obj.transform;
			Debug.Log("hand spon");
			NetworkServer.SpawnWithClientAuthority(trackHand, connectionToClient);
			//NetworkServer.Spawn(trackHand);
		}
	}

    private void Start()
    {
        gameObject.name = "Player" + playerControllerId.ToString();
        Debug.Log("MonoBehaviour Start function");
        StageInit();
    }

    public void StageInit()
    {
        SetPlayerPopPosition();
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

        var trPopPos = GameObject.Find("PlayerPopPos" + playerControllerId.ToString());

        if ( trPopPos ) {
            transform.position = trPopPos.transform.position;
            transform.rotation = trPopPos.transform.rotation;
        }
        else
        {
            Debug.LogWarning("PlayerPopPos" + playerControllerId.ToString() + "が見つかりません");
        }
    }
}
