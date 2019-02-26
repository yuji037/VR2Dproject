using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplayArea : MonoBehaviour {

	LayerMask playerLayer;

	[Header("チュートリアルを表示させたいプレイヤー番号"), SerializeField]
	PlayerNumber displayPlayerNumber;

	[SerializeField]
	TutorialObject tutorialObject;

	TutorialObject tutorialObjectIns = null;

	[Header("ラインなどのターゲット達"), SerializeField]
	GameObject[] targetObjects;

	// Use this for initialization
	void Start () {
		playerLayer = LayerMask.NameToLayer("Player");
	}
	
	private void OnTriggerEnter(Collider other)
	{
		// 両クライアントで起こる
		if(!tutorialObjectIns && other.gameObject.layer == playerLayer )
		{
			if ( PlayerManager.GetPlayerNumber() == (int)displayPlayerNumber )
			{
				var obj = Instantiate(tutorialObject.gameObject);
				tutorialObjectIns = obj.GetComponent<TutorialObject>();
				tutorialObjectIns.SetParent();
				tutorialObjectIns.Init();
				if ( targetObjects != null && targetObjects[0] != null )
					tutorialObjectIns.SetTarget(targetObjects);
				tutorialObjectIns.transform.localPosition = Vector3.zero;
				tutorialObjectIns.transform.localRotation = Quaternion.identity;
			}
        }
	}

	private void OnTriggerExit(Collider other)
	{
		if ( other.gameObject == PlayerManager.LocalPlayer )
		{
            if (tutorialObjectIns && tutorialObjectIns.gameObject)
                Destroy(tutorialObjectIns.gameObject);
		}
	}
}
