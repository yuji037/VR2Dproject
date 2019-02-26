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
		lineRenderer = GetComponent<LineRenderer>();
		if ( lineRenderer )
			lineRenderer.SetPosition(1, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
		if ( lineTargetObj )
		{
			var pos = lineTargetObj.transform.position - transform.position;
			lineRenderer.SetPosition(1, pos);
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
	}

	public void SetTarget(GameObject[] targetObjects)
	{
		lineTargetObj = targetObjects[0];
	}

	public void SetParent()
	{
		transform.parent = GameObject.Find(parentName).transform;
	}
}
