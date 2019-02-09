﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EffectManager : NetworkBehaviour {

	Dictionary<string, GameObject> m_Effects = new Dictionary<string, GameObject>();
	GameObject[] m_oChannelParents;
	GameObject[] m_oPlayingEffects;

	[SerializeField]
	int m_iChannelMax = 20;

	static EffectManager instance = null;

	private void Awake()
	{
		var effObjects = Resources.LoadAll<GameObject>("Effect");
		foreach ( var eff in effObjects )
		{
			m_Effects[eff.name] = eff;
			Debug.Log("Effect ロード：" + eff.name);
		}

		m_oChannelParents = new GameObject[m_iChannelMax];
		for(int i = 0; i < m_iChannelMax; ++i )
		{
			var channel = new GameObject("channel"+ i);
			channel.transform.parent = this.transform;
			m_oChannelParents[i] = channel;
		}
		m_oPlayingEffects = new GameObject[m_iChannelMax];
	}



	public override void OnStartClient()
	{
		if(instance != null )
		{
			Debug.LogError(GetType() + "が２つ作られた");
		}
		instance = this;
	}

	public static EffectManager GetInstance()
	{
		if(instance == null )
		{
			Debug.LogError("未接続");
		}
		return instance;
	}

	public int Play(string name, Vector3 position, bool playInAllClient = true, string attachTargetName1 = null, string attachTargetName2 = null)
	{
		int channel = FindPlayableChannel();
		if(channel == -1 )
		{
			// 再生失敗
			return -1;
		}

		if ( attachTargetName1 != null || attachTargetName2 != null )
		{
			//if ( attachTargetName1 == null ) attachTargetName1 = "";
			//if ( attachTargetName2 == null ) attachTargetName2 = "";
			if ( playInAllClient )
				CmdPlayAndAttachTarget(channel, name, position, attachTargetName1, attachTargetName2);
			else
			{

			}

		}
		else
		{
			if ( playInAllClient )
			{
				CmdPlay(channel, name, position);
			}
		}
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
	public void CmdPlay(int channel, string name, Vector3 position)
	{
		RpcPlay(channel, name, position);
	}

	[Command]
	public void CmdPlayAndAttachTarget(int channel, string name, Vector3 position, string attachTargetName1, string attachTargetName2)
	{
		RpcPlayAndAttachTarget(channel, name, position, attachTargetName1, attachTargetName2);
	}

	[ClientRpc]
	public void RpcPlay(int channel, string name, Vector3 position)
	{
		var eff = InstantiateEffect(channel, name, position);
	}

	[ClientRpc]
	public void RpcPlayAndAttachTarget(int channel, string name, Vector3 position, string attachTargetName1, string attachTargetName2)
	{
		var eff = InstantiateEffect(channel, name, position);
		eff = AttachTarget(eff, attachTargetName1, attachTargetName2);
	}


	GameObject InstantiateEffect(int channel, string name, Vector3 position)
	{
		var eff = Instantiate(m_Effects[name]);
		eff.transform.parent = m_oChannelParents[channel].transform;
		eff.transform.position = position;
		m_oPlayingEffects[channel] = eff;
		Debug.Log("Effect Play: " + name);
		return eff;
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
}