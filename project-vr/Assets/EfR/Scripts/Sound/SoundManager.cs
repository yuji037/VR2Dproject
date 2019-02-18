using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class SoundManager : NetworkBehaviour {

	static SoundManager instance = null;

	Dictionary<string, AudioClip> m_AudioClips = new Dictionary<string, AudioClip>();

	GameObject[] m_oChannelParents;
	SoundPlayIns[] m_oPlayingSounds;

	[SerializeField]
	int m_iChannelMax = 20;

	[SerializeField, Header("音を持たせるオブジェクト")]
	GameObject m_prefSound;

	[SerializeField]
	BGMSetting m_BGMSetting;

	[SerializeField]
	SoundSetting m_SoundSetting;

	PlayerMove m_cLocalPlayerMove = null;

	Transform m_trTVSpeakerPos = null;
	Transform m_trVRSpeakerPos = null;

	int m_iStageBGMChannel = -1;

	private void Awake()
	{
		instance = this;

		// プレハブのロード
		var audioClips = Resources.LoadAll<AudioClip>( "Sound" );
		foreach(var ac in audioClips )
		{
			m_AudioClips[ac.name] = ac;
			Debug.Log("Sound ロード：" + ac.name);
		}

		// チャネルの確保
		m_oChannelParents = new GameObject[m_iChannelMax];
		for ( int i = 0; i < m_iChannelMax; ++i )
		{
			var channel = new GameObject("channel" + i);
			channel.transform.parent = this.transform;
			m_oChannelParents[i] = channel;
		}
		m_oPlayingSounds = new SoundPlayIns[m_iChannelMax];
		this.GetGameObjectWithCoroutine("BGM2DSpeakerPos", 
			(obj) =>
		{
			m_trTVSpeakerPos = obj.transform;
		});
		this.GetGameObjectWithCoroutine("BGMVRSpeakerPos",
			(obj) =>
			{
				m_trVRSpeakerPos = obj.transform;
			});
	}

	//public override void OnStartClient()
	//{
	//	if ( instance != null )
	//	{
	//		Debug.LogError(GetType() + "が２つ作られた");
	//	}
	//	instance = this;
	//}

	public static SoundManager GetInstance()
	{
		if ( instance == null )
		{
			Debug.LogError("未接続");
		}
		return instance;
	}

	public int Play(
		string name,
		Vector3? position = null,
		bool playInAllClients = true,
		bool isLoop = false,
		bool playIn3DVolume = true,
		string attachTargetName = null)
	{

		int channel = FindPlayableChannel();

		if ( playInAllClients )
			CmdPlay(channel, name, position ?? Vector3.zero, isLoop, playIn3DVolume, attachTargetName);
		else
		{
			PlayLocal(channel, name, position ?? Vector3.zero, isLoop, playIn3DVolume, attachTargetName);
		}
		
		return channel;
	}

	[Command]
	public void CmdPlay(int channel, string name, Vector3 position, bool isLoop, bool playIn3DVolume, string attachTargetName)
	{
		RpcPlay(channel, name, position, isLoop, playIn3DVolume, attachTargetName);
	}

	[ClientRpc]
	public void RpcPlay(int channel, string name, Vector3 position, bool isLoop, bool playIn3DVolume, string attachTargetName)
	{
		PlayLocal(channel, name, position, isLoop, playIn3DVolume, attachTargetName);
	}
	
	private SoundPlayIns PlayLocal(
		int channel,
		string name,
		Vector3 position,
		bool isLoop = false,
		bool playIn3DVolume = true,
		string attachTargetName = null,
		int specialSoundSetting = -1)
	{
		if(m_cLocalPlayerMove == null )
		{
			m_cLocalPlayerMove = PlayerManager.LocalPlayer.GetComponent<PlayerMove>();
		}

		if(m_oPlayingSounds[channel])
		{
			Debug.LogError("サウンドの" + channel + "チャネルが使用中です");
		}

		var obj = Instantiate(m_prefSound, position, Quaternion.identity, m_oChannelParents[channel].transform);
		var soundPlayIns = obj.GetComponent<SoundPlayIns>();
		m_oPlayingSounds[channel] = soundPlayIns;
		soundPlayIns.Init();
		bool isEmptySound = string.IsNullOrEmpty(name);

		// オブジェクトへのアタッチ設定
		if ( !string.IsNullOrEmpty(attachTargetName))
		{
			var attachObj = GameObject.Find(attachTargetName);
			if ( attachObj )
				soundPlayIns.AttachTarget(attachObj.transform);
			else
				Debug.LogError(attachTargetName + "が見つかりませんでした");
		}

		var audioSource = soundPlayIns.m_AudioSource;
		audioSource.loop = isLoop;

		// ループしないなら自前で停止
		if ( !isLoop && !isEmptySound )
		{
			StartCoroutine(DestroyOnClipEndCoroutine(soundPlayIns, channel));
		}

		audioSource.spatialBlend = playIn3DVolume ? 1f : 0f;

		// 再生サウンドの指定
		if ( !isEmptySound )
		{
			audioSource.clip = m_AudioClips[name];
			audioSource.Play();
			
			if(specialSoundSetting != -1 )
			{
				AudioCustomizeModel audioCustomizeSetting = null;
				Debug.Log("m_cLocalPlayerMove.moveType : " + m_cLocalPlayerMove.moveType);
				switch ( m_cLocalPlayerMove.moveType )
				{
					case PlayerMove.MoveType.FIXED:
						audioCustomizeSetting = m_SoundSetting.soundSettings[specialSoundSetting].audioCusmizeSettings[0];
						break;
					case PlayerMove.MoveType._2D:
						audioCustomizeSetting = m_SoundSetting.soundSettings[specialSoundSetting].audioCusmizeSettings[1];
						break;
				}

				audioSource.rolloffMode = audioCustomizeSetting.rollOffMode;
				audioSource.maxDistance = audioCustomizeSetting.maxDistance;
				audioSource.minDistance = audioCustomizeSetting.minDistance;
			}

			if( m_cLocalPlayerMove.moveType == PlayerMove.MoveType._2D )
			{
				// TVの位置から再生するとデカすぎる音を弱める
				audioSource.volume *= 0.4f;
				Debug.Log("2Dのため音量調整");
			}
		}
		else
		{
			// ローカルのみのサウンドの場合は他クライアントで空のサウンドオブジェクトにする
			obj.name = "empty_sound";
		}

		Debug.Log("Play : " + name);

		return soundPlayIns;
	}
	
	public void PlayStageBGM(string stageName)
	{
		var list = m_BGMSetting.stageBGMNameList;

		foreach(var pair in list )
		{
			if(pair.StageName == stageName )
			{
				if ( m_iStageBGMChannel != -1 )
					FadeoutStageBGM();

				var playerMoveType = PlayerManager.LocalPlayer.GetComponent<PlayerMove>().moveType;
				Vector3 bgmPosition = Vector3.zero;
				switch ( playerMoveType )
				{
					case PlayerMove.MoveType._2D:
						bgmPosition = m_trTVSpeakerPos.position;
						break;
					case PlayerMove.MoveType.FIXED:
						bgmPosition = m_trVRSpeakerPos.position;
						break;
				}
				m_iStageBGMChannel = Play(pair.BGMName,
					bgmPosition, 
					true, true);
				//SetBGMSpeakerPosition();
				return;
			}
		}

		Debug.Log("Stage[" + stageName + "]に対応するBGMを探しましたがありませんでした");
	}

	public void SetBGMSpeakerPosition()
	{
		// スピーカー場所に移動
		var playerMoveType = PlayerManager.LocalPlayer.GetComponent<PlayerMove>().moveType;
		Transform speaker = null;
		switch ( playerMoveType )
		{
			case PlayerMove.MoveType._2D:
				speaker = m_trTVSpeakerPos;
				break;
			case PlayerMove.MoveType.FIXED:
				speaker = m_trVRSpeakerPos;
				break;
			default:
				break;
		}
		if ( speaker )
		{
			m_oPlayingSounds[m_iStageBGMChannel].transform.position = speaker.position;
			m_oPlayingSounds[m_iStageBGMChannel].transform.rotation = speaker.rotation;
		}
	}
	public void ChangeBGMVolume(float afterVolume, float duration = 1f)
	{
		ChangeVolume(m_iStageBGMChannel, afterVolume, duration);
	}

	public void FadeoutStageBGM(float duration = 1f)
	{
		Fadeout(m_iStageBGMChannel, duration);
		m_iStageBGMChannel = -1;
	}

	public void StopStageBGM()
	{
		Stop(m_iStageBGMChannel);
		m_iStageBGMChannel = -1;
	}

	IEnumerator DestroyOnClipEndCoroutine(SoundPlayIns soundPlayIns, int channel)
	{
        yield return new WaitUntil(() => soundPlayIns.m_AudioSource.clip);

        float clipLength = soundPlayIns.m_AudioSource.clip.length;

		yield return new WaitForSeconds( clipLength + 1.0f );

		Stop(channel);
	}

	public void ChangeVolume(int channel, float afterVoulume, float fadeDuration)
	{
		CmdChangeVolumeOrStop(channel, afterVoulume, fadeDuration, false);
	}

	public void Fadeout(int channel, float duration = 1f)
	{
		CmdChangeVolumeOrStop(channel, 0f, duration, true);
	}

	public void Stop(int channel)
	{
		CmdChangeVolumeOrStop(channel, 0f, 0f, true);
	}

	[Command]
	public void CmdChangeVolumeOrStop(int channel, float afterVolume, float fadeDuration, bool destroyAfterFade)
	{
		RpcChangeVolumeOrStop(channel, afterVolume, fadeDuration, destroyAfterFade);
	}

	[ClientRpc]
	public void RpcChangeVolumeOrStop(int channel, float afterVolume, float fadeDuration, bool destroyAfterFade)
	{
		if ( m_oPlayingSounds[channel] )
		{
			if ( m_oPlayingSounds[channel].m_AudioSource.isPlaying )
			{
				if ( destroyAfterFade )
				{
					m_oPlayingSounds[channel].FadeoutAndDestroy(fadeDuration);
				}
				else
					m_oPlayingSounds[channel].ChangeVolume(afterVolume, fadeDuration);
			}

			if ( destroyAfterFade )
				m_oPlayingSounds[channel] = null;
		}
	}

	private int FindPlayableChannel()
	{
		for ( int i = 0; i < m_iChannelMax; ++i )
		{
			if ( m_oPlayingSounds[i] == null )
			{
				return i;
			}
		}
		Debug.LogWarning("サウンド再生チャネルに空きがありません");
		return -1;
	}

}

