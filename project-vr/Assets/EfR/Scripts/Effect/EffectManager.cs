﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EffectManager : NetworkBehaviour {

	Dictionary<string, GameObject> m_Effects = new Dictionary<string, GameObject>();
	GameObject[] m_oChannelParents;
	EffectBehaviour[] m_oPlayingEffects;

	[SerializeField]
	int m_iChannelMax = 20;
    [SerializeField]
    GameObject m_prefEmptyEffect;

	static EffectManager instance = null;

	private void Awake()
	{
		instance = this;

		// プレハブのロード
		var effObjects = Resources.LoadAll<GameObject>("Effect");
		foreach ( var eff in effObjects )
		{
			m_Effects[eff.name] = eff;
			Debug.Log("Effect ロード：" + eff.name);
		}

		// チャネルの確保
		m_oChannelParents = new GameObject[m_iChannelMax];
		for(int i = 0; i < m_iChannelMax; ++i )
		{
			var channel = new GameObject("channel"+ i);
			channel.transform.parent = this.transform;
			m_oChannelParents[i] = channel;
		}
        m_oPlayingEffects = new EffectBehaviour[m_iChannelMax];
    }

	public static EffectManager GetInstance()
	{
		if(instance == null )
		{
			Debug.LogError("未接続");
		}
		return instance;
	}

    public int Play(string name, Vector3 position, bool playInAllClients = true,
        string attachTargetName1 = null, string attachTargetName2 = null, Vector3? endPosition = null)
    {
        int channel = FindPlayableChannel();
        if (channel == -1)
        {
            // 再生失敗
            return -1;
        }

        CmdPlay(channel, name, position, attachTargetName1, attachTargetName2, endPosition ?? new Vector3(-1, -1, -1),
            playInAllClients, PlayerManager.GetPlayerNumber());

        return channel;
    }

	public int FindPlayableChannel()
	{
		for ( int i = 0; i < m_iChannelMax; ++i )
		{
			if ( m_oPlayingEffects[i] == null )
			{
				return i;
			}
		}
		Debug.LogWarning("エフェクト再生チャネルに空きがありません");
		return -1;
	}

	[Command]
	public void CmdPlay(int channel, string name, Vector3 position, 
        string attachTargetName1, string attachTargetName2, Vector3 endPosition,
        bool playInAllClients, int triggeredPlayerNumber)
	{
		RpcPlay(channel, name, position, attachTargetName1, attachTargetName2, endPosition, playInAllClients, triggeredPlayerNumber);
	}

	[ClientRpc]
	public void RpcPlay(int channel, string name, Vector3 position,
        string attachTargetName1, string attachTargetName2, Vector3 endPosition,
        bool playInAllClients, int triggeredPlayerNumber)
	{
		var eff = InstantiateEffect(channel, name, position, endPosition, playInAllClients, triggeredPlayerNumber);
		eff = AttachTarget(eff, attachTargetName1, attachTargetName2);
    }


	GameObject InstantiateEffect(int channel, string name, Vector3 position, Vector3 endPosition,
        bool playInAllClients, int triggeredPlayerNumber)
	{
        GameObject obj = null;
        bool isEmptyEffect = !playInAllClients && PlayerManager.GetPlayerNumber() != triggeredPlayerNumber;
        if (!isEmptyEffect)
        {
            obj = Instantiate(m_Effects[name]);
        }
        else
        {
            obj = Instantiate(m_prefEmptyEffect);
        }

        var eff = obj.GetComponent<EffectBehaviour>();
		obj.transform.parent = m_oChannelParents[channel].transform;
		obj.transform.position = position;
		if(endPosition != new Vector3(-1, -1, -1) )
		{
			obj.GetComponentInChildren<EffectBehaviour>().SetEndPosition(endPosition);
		}
        m_oPlayingEffects[channel] = eff;
        Debug.Log("Effect Play: " + name);
		return obj;
	}

	GameObject AttachTarget(GameObject eff, string attachTargetName1, string attachTargetName2)
	{
		if ( string.IsNullOrEmpty(attachTargetName1) == false )
		{
			eff.GetComponentInChildren<EffectBehaviour>().SetAttachTarget(1, attachTargetName1);
		}
		if ( string.IsNullOrEmpty(attachTargetName2) == false )
		{
			eff.GetComponentInChildren<EffectBehaviour>().SetAttachTarget(2, attachTargetName2);
		}
		return eff;
	}

	public void Stop(int channel, float durationUntilDestroy = 2f)
	{
        if (channel != -1)
            CmdStop(channel, durationUntilDestroy);
	}

    [Command]
    void CmdStop(int channel, float durationUntilDestroy)
    {
        RpcStop(channel, durationUntilDestroy);
    }

    [ClientRpc]
    void RpcStop(int channel, float durationUntilDestroy)
    {
        if (m_oPlayingEffects[channel] == null)
        {
            Debug.LogError("エフェクトもうありません : チャネル" + channel);
            return;
        }

        var eff = m_oPlayingEffects[channel];
        m_oPlayingEffects[channel] = null;
        StartCoroutine(StopEffectCoroutine(eff, durationUntilDestroy));
    }

    IEnumerator StopEffectCoroutine(EffectBehaviour oEffect, float duration)
	{
		var particles = oEffect.GetComponentsInChildren<ParticleSystem>();
		foreach ( var par in particles )
		{
			par.Stop();
		}
		yield return new WaitForSeconds(duration);

		Destroy(oEffect.gameObject);
	}
}
