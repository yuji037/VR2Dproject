﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCharaHoloController : SingletonMonoBehaviour<VRCharaHoloController> {

	public GameObject[] chatCharas { get; private set; }
	Material[] charaMaterials;

	[SerializeField]
	float fadeLength = 1.5f;

	private void Start()
	{
		int maxPlayer = 2;
		chatCharas = new GameObject[maxPlayer];
		charaMaterials = new Material[maxPlayer];
		for(int i = 0; i < maxPlayer; ++i )
		{
			chatCharas[i] = GameObject.Find("VRChatCharaPos" + ( i + 1 ));
			var ren = chatCharas[i].GetComponentInChildren<SkinnedMeshRenderer>();
			charaMaterials[i] = ren.material;
			charaMaterials[i].SetFloat("_Pos", transform.position.y + 1.5f);
		}
	}

	public IEnumerator VRChatCharaFade(int playerNum, bool fadeIn)
	{
		if ( playerNum != 0 && playerNum != 1 )
		{
			Debug.LogError("指定が不正");
			yield break;
		}

		Debug.Log("Holo Start");

		var renderer = GameObject.Find("VRChatCharaPos" + ( playerNum + 1 )).GetComponentInChildren<SkinnedMeshRenderer>();
		var wPosY = renderer.transform.position.y;
		var holoMat = renderer.material;
		var speed = 1.0f;

		yield return StartCoroutine(MaterialsManager.HoloFadeCoroutine(
			holoMat, fadeIn, renderer.transform.position, null, fadeLength, speed));

		//holoMat.SetFloat("_App", 1);

		//for ( float t = 0; t <= fadeLength; t += Time.deltaTime * speed )
		//{
		//	float setPosY = wPosY + ( fadeLength - t );
		//	holoMat.SetFloat("_Pos", setPosY);
		//	yield return null;
		//}
	}

	public float GetCharaBorderPos(int playerNum)
	{
		return charaMaterials[playerNum].GetFloat("_Pos");
	}
}
