using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SoundSetting", menuName = "SoundSetting", order = 0)]
public class SoundSetting : ScriptableObject {

	public SoundSettingModel[] soundSettings;
}

[System.Serializable]
public class SoundSettingModel {

	[Header("0番目がVR視点時、1番目が2D視点時に適用される")]
	public AudioCustomizeModel[] audioCusmizeSettings = new AudioCustomizeModel[2];
}

[System.Serializable]
public class AudioCustomizeModel {

	[Header("適用したい音減衰方式")]
	public AudioRolloffMode rollOffMode = AudioRolloffMode.Logarithmic;

	public float minDistance = 1f;
	public float maxDistance = 500f;
}
