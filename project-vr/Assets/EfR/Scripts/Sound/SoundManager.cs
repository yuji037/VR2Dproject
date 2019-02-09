using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager> {

	Dictionary<string, AudioClip> m_AudioClips = new Dictionary<string, AudioClip>();

	[SerializeField, Header("音を持たせるオブジェクト")]
	GameObject m_prefSound;

	[SerializeField]
	BGMSetting m_BGMSetting;

	SoundPlayIns m_cStageSingleBGM = null;

	protected override void Awake()
	{
		base.Awake();

		var audioClips = Resources.LoadAll<AudioClip>( "Sound" );
		foreach(var ac in audioClips )
		{
			m_AudioClips[ac.name] = ac;
			Debug.Log("Sound ロード：" + ac.name);
		}
	}

	public SoundPlayIns Play(string name, Vector3? position = null, bool isLoop = false, bool playIn3DVolume = true)
	{
		var obj = Instantiate( m_prefSound, position??Vector3.zero, Quaternion.identity, transform );
		var soundPlayIns = obj.GetComponent<SoundPlayIns>();
		soundPlayIns.Init();

		var audioSource = soundPlayIns.m_AudioSource;
		audioSource.clip = m_AudioClips[name];
		audioSource.loop = isLoop;

		// ループしないなら自前で停止
		if ( !isLoop )
		{
			StartCoroutine( DestroyOnClipEndCoroutine( soundPlayIns ) );
		}

		audioSource.spatialBlend = playIn3DVolume ? 1f: 0f;

		audioSource.Play();
		Debug.Log("Play : " + audioSource.clip.name);

		return soundPlayIns;
	}

	// オブジェクトに追従して再生する
	public SoundPlayIns PlayOnAttachedTransform(string name, Transform attachPoint, bool isLoop = false)
	{
		var obj = Instantiate( m_prefSound, attachPoint.position, attachPoint.rotation, attachPoint );
		var soundPlayIns = obj.GetComponent<SoundPlayIns>();
		soundPlayIns.Init();
		soundPlayIns.AttachTarget( attachPoint );

		var audioSource = soundPlayIns.m_AudioSource;
		audioSource.clip = m_AudioClips[name];
		audioSource.loop = isLoop;

		// ループしないなら自前で停止
		if ( !isLoop )
		{
			StartCoroutine( DestroyOnClipEndCoroutine( soundPlayIns ) );
		}

		audioSource.Play();

		return soundPlayIns;
	}

	public void PlayStageBGM(string stageName)
	{
		var list = m_BGMSetting.stageBGMNameList;

		foreach(var pair in list )
		{
			if(pair.StageName == stageName )
			{
				if ( m_cStageSingleBGM )
					FadeoutStageBGM();

				m_cStageSingleBGM = Play(pair.BGMName, null, true);
				SetBGMSpeakerPosition();
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
				speaker = GameObject.Find("BGM2DSpeakerPos").transform;
				break;
			case PlayerMove.MoveType.FIXED:
				speaker = GameObject.Find("BGMVRSpeakerPos").transform;
				break;
			default:
				break;
		}
		if ( speaker )
		{
			m_cStageSingleBGM.transform.position = speaker.position;
			m_cStageSingleBGM.transform.rotation = speaker.rotation;
		}
	}

	public void FadeoutStageBGM(float duration = 1f)
	{
		m_cStageSingleBGM.FadeoutAndDestroy(duration);
		m_cStageSingleBGM = null;
	}

	public void StopStageBGM()
	{
		m_cStageSingleBGM.StopAndDestroy();
		m_cStageSingleBGM = null;
	}

	IEnumerator DestroyOnClipEndCoroutine(SoundPlayIns soundPlayIns)
	{
		float clipLength = soundPlayIns.m_AudioSource.clip.length;

		yield return new WaitForSeconds( clipLength + 0.5f );

		Destroy( soundPlayIns.gameObject );
	}

}

