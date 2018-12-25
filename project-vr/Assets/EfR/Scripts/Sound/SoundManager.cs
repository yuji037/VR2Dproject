using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager> {

	Dictionary<string, AudioClip> m_AudioClips = new Dictionary<string, AudioClip>();

	[SerializeField, Header("音を持たせるオブジェクト")]
	GameObject m_prefSound;


	protected override void Awake()
	{
		base.Awake();

		var audioClips = Resources.LoadAll<AudioClip>( "BGM" );
		foreach(var ac in audioClips )
		{
			m_AudioClips[ac.name] = ac;
			Debug.Log(ac.name);
		}
	}

	public void Play(string name, Vector3? position = null, bool isLoop = false, bool playIn3DVolume = true)
	{
		if( position == null )
		{
			position = Vector3.zero;
		}
		var obj = Instantiate( m_prefSound, transform.position, Quaternion.identity, transform );
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

		if ( !playIn3DVolume )
		{
			audioSource.spatialBlend = 0f;
		}

		audioSource.Play();
	}

	public void PlayOnAttachedTransform(string name, Transform attachPoint, bool isLoop = false)
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
	}

	IEnumerator DestroyOnClipEndCoroutine(SoundPlayIns soundPlayIns)
	{
		float clipLength = soundPlayIns.m_AudioSource.clip.length;

		yield return new WaitForSeconds( clipLength + 0.5f );

		Destroy( soundPlayIns.gameObject );
	}

	public void StopAndDestroy(SoundPlayIns soundPlayIns)
	{

	}

	public void FadeOut()
	{
		//StartCoroutine(FadeOutCoroutine());
	}

}

