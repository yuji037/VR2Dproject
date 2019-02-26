using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour {

	[SerializeField]
	string parentName;

    [SerializeField]
    GameObject inactiveObjInPlayMode;

	[SerializeField]
	bool isLocalPlayerCanMove = true;

	LineRenderer lineRenderer;
	GameObject lineTargetObj = null;
	[SerializeField]
	float lineOffsetMoveSpeed = 1.0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if ( lineTargetObj )
		{
			//var pos = lineTargetObj.transform.position - transform.position;
			lineRenderer.SetPosition(0, transform.position);
			lineRenderer.SetPosition(1, lineTargetObj.transform.position);
			lineRenderer.material.mainTextureOffset += new Vector2(Time.deltaTime * lineOffsetMoveSpeed, 0f);
		}
	}

	public void Init()
	{
        // コントローラの試作モデルは非表示
        if (inactiveObjInPlayMode)
            inactiveObjInPlayMode.SetActive(false);

		// プレイヤーが操作できるかの設定
		PlayerManager.LocalPlayer.GetComponent<PlayerMove>().canMove = isLocalPlayerCanMove;

		lineRenderer = GetComponentInChildren<LineRenderer>();
		if ( lineRenderer )
			lineRenderer.SetPosition(1, Vector3.zero);
	}

	public void SetTarget(GameObject[] targetObjects)
	{
		lineTargetObj = targetObjects[0];
	}

	public void SetParent()
	{
		var parentObj = GameObject.Find(parentName);
		if ( !parentObj )
		{
			Debug.LogWarning("親オブジェクト見つからず");
			return;
		}
		transform.parent = parentObj.transform;
	}
}
