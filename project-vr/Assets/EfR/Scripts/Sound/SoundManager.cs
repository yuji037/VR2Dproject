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
	int m_iChannelMax = 40;

    int m_iChannelHalf = 0;
	int m_iPlayChannelCounter = 0;

	[SerializeField, Header("音を持たせるオブジェクト")]
	GameObject[] m_prefSound;

	[SerializeField]
	BGMSetting m_BGMSetting;

	[SerializeField]
	SoundSetting m_SoundSetting;

	PlayerMove m_cLocalPlayerMove = null;

	Transform m_trTVSpeakerPos = null;
    public Transform TVSpeakerPos { get { return m_trTVSpeakerPos; } }
	Transform m_trVRSpeakerPos = null;
    public Transform VRSpeakerPos { get { return m_trVRSpeakerPos; } }

    int m_iBGMChannel = -1;

    public override void OnStartLocalPlayer()
	{
        if (!isLocalPlayer) return;

		instance = this;
        m_iChannelHalf = (int)(m_iChannelMax / 2);

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
        string attachTargetName = null,
        int soundSettingID = 0,
		int specificPlayPlayerNumber = -1)
	{

		int channel = FindPlayableChannel();
        if(channel == -1)
        {
            Debug.LogWarning("再生に失敗");
            return -1;
        }

		if ( specificPlayPlayerNumber != -1 )
		{
			
		}
		else specificPlayPlayerNumber = PlayerManager.GetPlayerNumber();


		CmdPlay(channel, name, position ?? Vector3.zero, isLoop, playIn3DVolume, attachTargetName,
               specificPlayPlayerNumber, playInAllClients, soundSettingID);
		
		return channel;
	}

	[Command]
	public void CmdPlay(int channel, string name, Vector3 position, bool isLoop, bool playIn3DVolume, string attachTargetName,
        int specificPlayPlayerNumber, bool playInAllClients, int soundSettingID)
	{
		RpcPlay(channel, name, position, isLoop, playIn3DVolume, attachTargetName, specificPlayPlayerNumber, playInAllClients, soundSettingID);
	}

	[ClientRpc]
	public void RpcPlay(int channel, string name, Vector3 position, bool isLoop, bool playIn3DVolume, string attachTargetName,
        int specificPlayPlayerNumber, bool playInAllClients, int soundSettingID)
	{
		instance.PlayLocal(channel, name, position, isLoop, playIn3DVolume, attachTargetName, specificPlayPlayerNumber, playInAllClients, soundSettingID);
	}
	
	private SoundPlayIns PlayLocal(
		int channel,
		string name,
		Vector3 position,
		bool isLoop,
		bool playIn3DVolume,
		string attachTargetName,
        int specificPlayPlayerNumber, 
        bool playInAllClients,
        int soundSettingID = 0)
	{
		if(m_cLocalPlayerMove == null )
		{
			m_cLocalPlayerMove = PlayerManager.LocalPlayer.GetComponent<PlayerMove>();
		}

		if(m_oPlayingSounds[channel])
		{
			Debug.Log("サウンドの" + channel + "チャネルが使用中。再生をやめます");

			//         int retryChannel = FindPlayableChannel();
			//         if(retryChannel != -1)
			//         {
			//             CmdPlay(retryChannel, name, position, isLoop, playIn3DVolume, attachTargetName, triggeredPlayerNumber, playInAllClients, soundSettingID);
			//             return null;
			//         }
			return null;
        }

		if(m_cLocalPlayerMove.moveType == PlayerMove.MoveType._2D )
		{
			position = m_trTVSpeakerPos.position;
		}

		var obj = Instantiate(m_prefSound[soundSettingID], position, Quaternion.identity, m_oChannelParents[channel].transform);
		var soundPlayIns = obj.GetComponent<SoundPlayIns>();
		m_oPlayingSounds[channel] = soundPlayIns;
		soundPlayIns.Init();
        // このクライアントで再生するかどうか
		bool isEmptySound = !playInAllClients && PlayerManager.GetPlayerNumber() != specificPlayPlayerNumber;

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

		audioSource.spatialBlend = playIn3DVolume ? 1f : 0f;

		// 再生サウンドの指定
		if ( !isEmptySound )
		{
			audioSource.clip = m_AudioClips[name];
			audioSource.Play();

			//DebugTools.DisplayText("SE" + soundPlayIns.GetInstanceID(), "SE(" + name + ") " + soundPlayIns.GetInstanceID());
			//Debug.Log("Play : " + name);
		}
		else
        {
            audioSource.clip = m_AudioClips[name];
            // ローカルのみのサウンドの場合は他クライアントで空のサウンドオブジェクトにする
            obj.name = "empty_sound";
			//Debug.Log("Play : empty_sound");
		}

		// ループしないなら自前で停止
		if (!isLoop)
        {
            StartCoroutine(DestroyOnClipEndCoroutine(soundPlayIns, channel));
        }


		return soundPlayIns;
	}
	
	public void PlayStageBGM(string stageName)
	{
		var list = m_BGMSetting.stageBGMNameList;

		foreach(var pair in list )
		{
			if(pair.StageName == stageName )
			{
                var bgmName = pair.BGMName;

				return;
			}
		}

		Debug.Log("Stage[" + stageName + "]に対応するBGMを探しましたがありませんでした");
	}

    public void PlayBGM(string bgmName)
    {
        if (m_iBGMChannel != -1)
            FadeoutBGM();

        Vector3 bgmPosition = Vector3.zero;
        int soundSettingID = 0;
        GetBGMSettingParam(out bgmPosition, out soundSettingID);

        m_iBGMChannel = Play(bgmName, bgmPosition, false, true, true, null, soundSettingID);
    }

    public void UpdateBGMParam()
    {
        if (m_iBGMChannel == -1)
            return;

        Vector3 bgmPosition = Vector3.zero;
        int soundSettingID = 0;
        GetBGMSettingParam(out bgmPosition, out soundSettingID);

        // 途中で停止して途中から再生
        var currentTimeInBgm = m_oPlayingSounds[m_iBGMChannel].m_AudioSource.time;
        var bgmName = m_oPlayingSounds[m_iBGMChannel].m_AudioSource.clip.name;
        Debug.Log("BGM再生位置：" + currentTimeInBgm);
        Stop(m_iBGMChannel);
        m_iBGMChannel = Play(bgmName, bgmPosition, false, true, true, null, soundSettingID);
        SetSoundTime(m_iBGMChannel, currentTimeInBgm);
    }

    void GetBGMSettingParam(out Vector3 position, out int soundSettingID)
    {
        position = Vector3.zero;
        soundSettingID = 0;

        var playerMoveType = PlayerManager.LocalPlayer.GetComponent<PlayerMove>().moveType;
        switch (playerMoveType)
        {
            case PlayerMove.MoveType._2D:
                position = m_trTVSpeakerPos.position;
                soundSettingID = 1;
                break;
            case PlayerMove.MoveType.FIXED:
                position = m_trVRSpeakerPos.position;
                soundSettingID = 2;
                break;
        }

    }

	public void ChangeBGMVolume(float afterVolume, float duration = 1f)
	{
		ChangeVolume(m_iBGMChannel, afterVolume, duration);
	}

	public void FadeoutBGM(float duration = 1f)
	{
		Fadeout(m_iBGMChannel, duration);
		m_iBGMChannel = -1;
	}

	public void StopBGM()
	{
		Stop(m_iBGMChannel);
		m_iBGMChannel = -1;
	}

	IEnumerator DestroyOnClipEndCoroutine(SoundPlayIns soundPlayIns, int channel)
	{
        float clipLength = soundPlayIns.m_AudioSource.clip.length;

		yield return new WaitForSeconds( clipLength + 1.0f );

		Stop(channel);
	}

	public void ChangeVolume(int channel, float afterVoulume, float fadeDuration)
	{
        if (channel != -1)
            CmdChangeVolumeOrStop(channel, afterVoulume, fadeDuration, false);
    }

	public void Fadeout(int channel, float duration = 1f)
	{
        if (channel != -1)
            CmdChangeVolumeOrStop(channel, 0f, duration, true);
    }

	public void Stop(int channel)
	{
        if (channel != -1)
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
        instance.ChangeVolumeOrStopLocal(channel, afterVolume, fadeDuration, destroyAfterFade);
	}

    public void ChangeVolumeOrStopLocal(int channel, float afterVolume, float fadeDuration, bool destroyAfterFade)
    {
        if (m_oPlayingSounds[channel])
        {
            if (m_oPlayingSounds[channel].m_AudioSource.isPlaying)
            {
                if (destroyAfterFade)
                {
                    m_oPlayingSounds[channel].FadeoutAndDestroy(fadeDuration);
                }
                else
                    m_oPlayingSounds[channel].ChangeVolume(afterVolume, fadeDuration);
            }

            if (destroyAfterFade)
            {
				m_oPlayingSounds[channel].StopAndDestroy();
                m_oPlayingSounds[channel] = null;
            }
        }
    }

    private int FindPlayableChannel()
	{
        int channelMin = PlayerManager.GetPlayerNumber() == 0 ? 0 : m_iChannelHalf;
        int channelMax = channelMin + m_iChannelHalf;
        for ( int i = m_iPlayChannelCounter; i < m_iPlayChannelCounter + m_iChannelHalf; ++i )
		{
			int index = i % m_iChannelHalf + channelMin;
			if ( m_oPlayingSounds[index] == null )
			{
				m_iPlayChannelCounter = index + 1;
				return index;
			}
		}
		Debug.LogWarning("サウンド再生チャネルに空きがありません");
		return -1;
	}

    public void SetSoundTime(int channel, float time)
    {
        if (channel != -1)
            CmdSetSoundTime(channel, time);
    }

    [Command]
    void CmdSetSoundTime(int channel, float time)
    {
        RpcSetSoundTime(channel, time);
    }

    
    [ClientRpc]
    void RpcSetSoundTime(int channel, float time)
    {
        instance.SetSoundTimeLocal(channel, time);
    }

    void SetSoundTimeLocal(int channel, float time)
    {
        if (m_oPlayingSounds[channel])
        {
            if (m_oPlayingSounds[channel].m_AudioSource.isPlaying)
            {
                m_oPlayingSounds[channel].m_AudioSource.time = time;
            }
        }
    }

}

