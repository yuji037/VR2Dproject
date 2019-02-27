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

    [Header("表示秒数（0の場合持続表示"), SerializeField]
    float displayTime = 0f;
    float timer = 0f;

	// Use this for initialization
	void Start () {
		playerLayer = LayerMask.NameToLayer("Player");
	}

    private void Update()
    {
        if (displayTime == 0f) return;
        if (tutorialObjectIns) return;

        timer += Time.deltaTime;
        if(timer > displayTime)
        {
            DestroyTutorial();
        }
    }

    private void OnTriggerEnter(Collider other)
	{
		if(tutorialObject == null )
		{
			Debug.LogWarning("チュートリアル設定されていません。");
			return;
		}

		// 両クライアントで起こる
		if (!tutorialObjectIns && other.gameObject.layer == playerLayer )
		{
			if ( PlayerManager.GetPlayerNumber() == (int)displayPlayerNumber )
			{
                bool isCorrectPlayerMoveType = false;
                PlayerMove localPlayerMove = PlayerManager.LocalPlayer.GetComponent<PlayerMove>();
                if (tutorialObject.name.Contains("2D") &&
                    localPlayerMove.moveType == PlayerMove.MoveType._2D)
                    isCorrectPlayerMoveType = true;

                if (tutorialObject.name.Contains("VR") &&
                    localPlayerMove.moveType == PlayerMove.MoveType.FIXED)
                    isCorrectPlayerMoveType = true;

                if (!isCorrectPlayerMoveType) return;

                CreateTutorial();
			}
        }
	}

	private void OnTriggerExit(Collider other)
	{
		if ( other.gameObject.layer == playerLayer )
		{
            DestroyTutorial();
		}
	}

    void CreateTutorial()
    {
        var obj = Instantiate(tutorialObject.gameObject);
        tutorialObjectIns = obj.GetComponent<TutorialObject>();
        tutorialObjectIns.SetParent();
        tutorialObjectIns.Init();
        if (targetObjects != null && targetObjects.Length != 0)
            tutorialObjectIns.SetTarget(targetObjects);
        tutorialObjectIns.transform.localPosition = Vector3.zero;
        tutorialObjectIns.transform.localRotation = Quaternion.identity;
        SoundManager.GetInstance().Play(
            "pipi2", tutorialObjectIns.transform.position, false, false, true, null, 0, (int)displayPlayerNumber);

        timer = 0f;
    }

    void DestroyTutorial()
    {
        if (tutorialObjectIns && tutorialObjectIns.gameObject)
        {
            Destroy(tutorialObjectIns.gameObject);
            tutorialObjectIns = null;
        }
    }
}
