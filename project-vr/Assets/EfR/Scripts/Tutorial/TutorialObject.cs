using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour {

	[SerializeField]
	public string parentName;

    [SerializeField]
    GameObject inactiveObjInPlayMode;

	[SerializeField]
	bool isLocalPlayerCanMove = true;

	LineRenderer lineRenderer;
	[SerializeField]
	ParticleSystem leadParticle;
	GameObject lineTargetObj = null;
	GameObject particleTargetObj = null;
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
		if( particleTargetObj && leadParticle )
		{
			leadParticle.transform.position = particleTargetObj.transform.position;
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
		if ( targetObjects.Length > 0 )
			lineTargetObj = targetObjects[0];
		if ( targetObjects.Length > 1 )
		{
			particleTargetObj = targetObjects[1];
			if ( leadParticle )
				leadParticle.gameObject.SetActive(true);
		}
	}

	public void SetParent()
	{
		var parentObj = GameObject.Find(parentName);
		if ( !parentObj )
		{
			Debug.LogWarning(parentName + "見つからず");
			return;
		}
		transform.parent = parentObj.transform;
	}
}
