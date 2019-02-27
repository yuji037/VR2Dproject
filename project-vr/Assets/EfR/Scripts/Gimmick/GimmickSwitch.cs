using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GimmickSwitch : GimmickBase {


    [SerializeField, Header("押せる物体のギミックID（プレイヤーの手、ブロックなど）")]

    int[]		m_iTriggerGimmickIDs;

    [SerializeField, Header("反応する対象のID")]
    int[]       m_iActorGimmickIDs;

    [SerializeField,Header("物理的に押されるオブジェクト")]
    Transform   m_PushObject;

	[SerializeField, Header("光線ポインターで反応するかどうか")]

	bool		m_IsActPointerHit;

	[SerializeField, Header("スイッチを離したときにもアクションするか")]

	bool		m_IsActOnRelease;


	[SerializeField, Header("押したときの位置変化ベクトル")]

	Vector3		m_vPushedVector	= new Vector3(0, -0.2f, 0);

	[SerializeField, Header("押せるスピード")]

	float		m_fPerformSpeed		= 1.0f;

	[SerializeField, Header("トグルスイッチかどうか（1回押すとON、もう1回押すとOFF）")]

	bool		m_IsToggle			= false;

    [SerializeField, Header("クライアントにもOnOffを通知する")]
    bool        m_IsSwitchingNotifyToClient   = false;

    [SerializeField, Header("当たった判定を行うクライアントのプレイヤー番号")]
    PlayerNumber pushJudgePlayerNumber;

    public ISwitchableObject[] ActorSwitchableObjects
    {
        get
        {
            if (actorSwitchableObjects == null)
            {
                actorSwitchableObjects = new ISwitchableObject[m_iActorGimmickIDs.Length];
                int idx = 0;
                foreach (var i in m_iActorGimmickIDs)
                {
                    var gimmick = GimmickManager.GetActor(i);
                    var switchable = (gimmick as MonoBehaviour).GetComponent<ISwitchableObject>();
                    actorSwitchableObjects[idx] = switchable;
                    idx++;
                }
            }
            return actorSwitchableObjects;
        }
    }


    #region 内部変数

    [SerializeField]
	bool		m_IsPushing			= false;
	bool		m_IsPressed			= false;
	bool		m_IsToggleOn		= false;

	Vector3		m_vReleasedPosition;
	Vector3		m_vPressedPosition;
	float		m_fPressedDistanceSqr;
    bool        m_Initialized;

    ISwitchableObject[] actorSwitchableObjects;
	#endregion


	// Use this for initialization
	void Start()
	{
        if (!m_PushObject) m_PushObject = transform;

        isCallOnlyServer = false;

        m_aTriggerEnterAction += PushJudge;
		// 光線でONになるかどうか
		if ( m_IsActPointerHit )
			m_aPointerHitAction		+= PushJudge;

		m_aTriggerExitAction		+= ReleaseJudge;

		m_vReleasedPosition			= m_PushObject.localPosition;
		m_vPressedPosition			= m_PushObject.localPosition + m_vPushedVector;
		m_fPressedDistanceSqr		= ( m_vPressedPosition - m_vReleasedPosition ).sqrMagnitude;

    }
    private  void Initialize()
    {
        if (m_Initialized) return;
        var netId = GetComponent<NetworkIdentity>();
        if(netId.localPlayerAuthority)PlayerManager.playerStatus.SetAuth(netId);
        
        m_Initialized = true;
    }

    bool IsReady()
    {
        return PlayerManager.LocalPlayer &&
            PlayerManager.playerStatus &&
            ((PlayerNumber)PlayerManager.GetPlayerNumber() == pushJudgePlayerNumber);
    }

    private void Update()
	{
        if(!IsReady())return;
        Initialize();

        if ( m_IsPushing )
		{
			OnPushing();
		}
		else
		{
			OnReleasing();
		}
	}

	// スイッチが沈み切るとアクションが起こる
	void OnPushing()
	{
		Vector3		nowPressedVector		=	m_PushObject.localPosition - m_vReleasedPosition;
		float		nowPressedDistanceSqr	=	nowPressedVector.sqrMagnitude;


		if (		nowPressedDistanceSqr < m_fPressedDistanceSqr )
		{
			// スイッチが沈んでいる途中
			MoveThisFrame(true);
		}
		else
		{
			// スイッチが沈み切った
			if ( m_IsPressed ) return;
			
			m_IsPressed		= true;

			if ( m_IsToggle )
			{
				// トグルスイッチの場合
                NotifySwitching(!m_IsToggleOn);

				m_IsToggleOn = !m_IsToggleOn;
			}
			else
			{
                // トグルスイッチじゃない場合
                NotifySwitching(true);
			}
			m_PushObject.localPosition = m_vPressedPosition;
		}
	}

	void OnReleasing()
	{
		Vector3		nowReleasedVector		= m_PushObject.localPosition - m_vPressedPosition;
		float		nowReleasedDistanceSqr	= nowReleasedVector.sqrMagnitude;

		if (		nowReleasedDistanceSqr	< m_fPressedDistanceSqr )
		{
			// スイッチが浮かんでいる途中
			// 離してアクションするタイプならここでアクション
			if ( m_IsPressed && m_IsActOnRelease)
                NotifySwitching(false);

			MoveThisFrame(false);
			m_IsPressed = false;
		}
		else
		{
			// スイッチが浮かび切った
			// スイッチに何も触れてない定常時

			m_PushObject.localPosition = m_vReleasedPosition;
		}
	}

    [Command]
    void CmdSyncButtonPos(Vector3 moveVec)
    {
        var targetPlayer = PlayerManager.Players[((int)pushJudgePlayerNumber == 0) ? 1 : 0];
        if (!targetPlayer) return;
        var target = targetPlayer.GetComponent<NetworkIdentity>().connectionToClient;
        TargetSyncButtonPos(target,moveVec);
    }

    [TargetRpc]
    void TargetSyncButtonPos(NetworkConnection target,Vector3 moveVec)
    {
        m_PushObject.Translate(moveVec,Space.Self);
    }

    void MoveThisFrame(bool isPress)
	{
		var moveVec =	m_vPushedVector * m_fPerformSpeed*Time.deltaTime*((isPress)?1.0f:-1.0f);
		m_PushObject.Translate(moveVec, Space.Self);
        if (hasAuthority) CmdSyncButtonPos( moveVec);
	}

    protected virtual void PushJudge(int otherGimmickID)
	{
		Debug.Log("Push"+otherGimmickID);
		foreach ( var triggerID in m_iTriggerGimmickIDs )
		{
			if ( otherGimmickID == triggerID )
			{
                m_IsPushing=true;
			}
		}
	}

	protected virtual void ReleaseJudge(int otherGimmickID)
	{
		Debug.Log("Release"+otherGimmickID);
		foreach ( var triggerID in m_iTriggerGimmickIDs )
		{
			if ( otherGimmickID == triggerID )
			{
				m_IsPushing= false;
			}
		}
	}

    void NotifySwitching(bool isOn)
    {
        Debug.Log("通知:"+isOn);
        if (m_IsSwitchingNotifyToClient)
        {
            CmdTurnSwitchableObject(isOn);
        }
        else
        {
            TurnSwitchableObject(isOn);
        }
    }

    [Command]
    void CmdTurnSwitchableObject(bool isOn)
    {
        RpcTurnSwitchableObject(isOn);
		SoundManager.GetInstance().Play("buttonpush", transform.position);
	}

	[ClientRpc]
    void RpcTurnSwitchableObject(bool isOn)
    {
        TurnSwitchableObject(isOn);
    }

    void TurnSwitchableObject(bool isOn)
    {
        Debug.Log("Act Switch"+ isOn);
        foreach (var i in ActorSwitchableObjects)
        {
            if (isOn)
            {
                i.OnAction();
            }
            else
            {
                i.OffAction();
            };
        }
    }
}
