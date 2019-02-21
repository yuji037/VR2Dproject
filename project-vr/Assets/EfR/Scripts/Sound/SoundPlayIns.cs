using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundPlayIns : MonoBehaviour{

	public AudioSource m_AudioSource;

	Transform m_trAttachedTarget = null;

	public void Init()
	{
		m_AudioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		if ( m_trAttachedTarget )
		{
			transform.position = m_trAttachedTarget.position;
		}
	}

	public void ChangeVolume(float afterVolume, float changeDuration = 1.0f)
	{
		if ( afterVolume == 0f )
		{
			Debug.LogWarning( "SoundOneIns.ChangeVolumeによってVolume→0になりますが破棄されません" );
		}
		StartCoroutine( ChangeVolumeCoroutine( afterVolume, changeDuration ) );
	}

	public void FadeoutAndDestroy(float duration = 1.0f)
	{
		StartCoroutine( ChangeVolumeCoroutine( 0f, duration, DestroyIfExists) );
	}

	public void Fadeout(float duration = 1.0f)
	{
		StartCoroutine(ChangeVolumeCoroutine(0f, duration));
	}

	public void StopAndDestroy()
	{
		Stop();
		DestroyIfExists();
	}

	public void Stop()
	{
		m_AudioSource.Stop();
	}

	IEnumerator ChangeVolumeCoroutine(float afterVolume, float duration, Action onEnd = null)
	{
		float beforeVolume = m_AudioSource.volume;

		for ( float t = 0; t < duration; t += Time.unscaledDeltaTime )
		{
			float transRate = t / duration;

			m_AudioSource.volume =
				beforeVolume * ( 1 - transRate ) +
				afterVolume * transRate;

			yield return null;
		}

		m_AudioSource.volume = afterVolume;
		DebugTools.Log("Sound(" + m_AudioSource.clip.name + ")ボリューム調節" + this.GetInstanceID());

		if ( onEnd != null)
		{
			onEnd();
		}
	}

	// 音の再生位置をオブジェクトに追従させる
	public void AttachTarget(Transform targetTransform)
	{
		if(targetTransform == null )
		{
			Debug.LogError("attachTargetがnull : " + targetTransform);
			return;
		}
		m_trAttachedTarget = targetTransform;

		UpdatePosition();
	}

	void DestroyIfExists()
	{
		if ( gameObject )
		{
			DebugTools.UnregisterDisplayText("SE" + this.GetInstanceID());
			Destroy(gameObject);
		}
	}
}