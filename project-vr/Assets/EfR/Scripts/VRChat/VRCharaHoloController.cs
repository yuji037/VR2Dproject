using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCharaHoloController : SingletonMonoBehaviour<VRCharaHoloController> {

	[SerializeField]
	GameObject[] chatCharas;
	Material[] charaMaterials;

	[SerializeField]
	float fadeLength = 1.5f;

	[SerializeField]
	float fadeTime = 2.0f;


	public GameObject GetChatCharaObject(int playerNum)
	{
		return chatCharas[playerNum];
	}

	public float GetCharaBorderPos(int playerNum)
	{
		return charaMaterials[playerNum].GetFloat("_Pos");
	}

	private void Start()
	{
		int maxPlayer = 2;
		//chatCharas = new GameObject[maxPlayer];
		charaMaterials = new Material[maxPlayer];
		for(int i = 0; i < maxPlayer; ++i )
		{
			//chatCharas[i] = GameObject.Find("VRChatCharaPos" + ( i + 1 ));
			var ren = chatCharas[i].GetComponentInChildren<SkinnedMeshRenderer>();
			charaMaterials[i] = ren.material;
			//charaMaterials[i].SetFloat("_Pos", transform.position.y + 1.5f);
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

		var renderer = chatCharas[playerNum].GetComponentInChildren<SkinnedMeshRenderer>();
		//var wPosY = renderer.transform.position.y;
		var holoMat = renderer.material;
		//var length = fadeLength * chatCharas[playerNum]
		var length = fadeLength * chatCharas[playerNum].transform.lossyScale.y;
		var speed = length / fadeTime;

		yield return StartCoroutine(MaterialsManager.HoloFadeCoroutine(
			holoMat, fadeIn, chatCharas[playerNum].transform.position, chatCharas[playerNum].transform, length, speed));

		//holoMat.SetFloat("_App", 1);

		//for ( float t = 0; t <= fadeLength; t += Time.deltaTime * speed )
		//{
		//	float setPosY = wPosY + ( fadeLength - t );
		//	holoMat.SetFloat("_Pos", setPosY);
		//	yield return null;
		//}
	}

	private void OnDrawGizmos()
	{
		if ( chatCharas == null ) return;

		Gizmos.color = Color.yellow;

		for(int i = 0; i < chatCharas.Length; ++i )
		{
			if ( chatCharas[i] == null ) continue;

			var length = fadeLength * chatCharas[i].transform.lossyScale.y;
			Gizmos.DrawLine(chatCharas[i].transform.position, chatCharas[i].transform.position + new Vector3(0, length,0));
		}
	}
}
