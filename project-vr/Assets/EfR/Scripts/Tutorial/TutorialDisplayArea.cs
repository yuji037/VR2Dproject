using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplayArea : MonoBehaviour {

	LayerMask playerLayer;

	[Header("チュートリアルを表示させたいプレイヤー番号"), SerializeField]
	PlayerNumber displayPlayerNumber;

	[SerializeField]
	TutorialObject tutorialObject;

	static Dictionary<string, TutorialObject> tutorialObjInsList = new Dictionary<string, TutorialObject>();

	[Header("ラインなどのターゲット達"), SerializeField]
	GameObject[] targetObjects;

    //[Header("表示秒数（0の場合持続表示)"), SerializeField]
    //float displayTime = 0f;
    //float timer = 0f;

	// Use this for initialization
	void Start () {
		playerLayer = LayerMask.NameToLayer("Player");
	}

    private void OnTriggerEnter(Collider other)
	{
        Debug.Log(this + " OnTriggerEnter");

		if(tutorialObject == null )
		{
			Debug.LogWarning("チュートリアル設定されていません。");
			return;
		}

		// 両クライアントで起こる
		if (!tutorialObjInsList.ContainsKey(tutorialObject.parentName) && other.gameObject.layer == playerLayer )
		{
			if ( PlayerManager.GetPlayerNumber() == (int)displayPlayerNumber )
			{
                bool isCorrectPlayerMoveType = false;
				PlayerMove localPlayerMove = PlayerManager.playerMove;
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
		if ( tutorialObjInsList.ContainsKey(tutorialObject.parentName) && other.gameObject.layer == playerLayer )
		{
            DestroyTutorial();
		}
	}

    void CreateTutorial()
    {
        var obj = Instantiate(tutorialObject.gameObject);
        var tutorialObjectIns = obj.GetComponent<TutorialObject>();
		var parentName = tutorialObjectIns.parentName;
        tutorialObjectIns.SetParent();
        tutorialObjectIns.Init();
        if (targetObjects != null && targetObjects.Length != 0)
            tutorialObjectIns.SetTarget(targetObjects);
        tutorialObjectIns.transform.localPosition = Vector3.zero;
        tutorialObjectIns.transform.localRotation = Quaternion.identity;
        SoundManager.GetInstance().Play(
            "pipi2", tutorialObjectIns.transform.position, false, false, true, null, 0, (int)displayPlayerNumber);

		tutorialObjInsList[parentName] = tutorialObjectIns;
    }

    void DestroyTutorial()
    {
		var parentName = tutorialObject.parentName;
		var tutorialObjectIns = tutorialObjInsList[parentName];
		if ( tutorialObjectIns && tutorialObjectIns.gameObject)
        {
            Destroy(tutorialObjectIns.gameObject);
			tutorialObjInsList.Remove(parentName);
        }
    }

	public static void DestroyAllTutorial()
	{
		foreach(var ins in tutorialObjInsList.Values )
		{
			//Debug.Log(ins.gameObject.name + "チュートリアル削除");
			Destroy(ins.gameObject);
		}
		tutorialObjInsList.Clear();
	}

	public static IEnumerator DestroyAllTutorialCoroutine()
	{
		yield return new WaitForSeconds(1f);
		DestroyAllTutorial();
	}
}
