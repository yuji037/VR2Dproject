using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFallableObject : GimmickBase {

	[SerializeField]
	private GameObject m_oParent;

	private Rigidbody m_rRigidbody;
	// 当たり判定などに使う、Box形状の大きさ
	private Vector3 m_vHalfExtents;
	private Vector3 m_vDefaultLocalPosition;

	// Use this for initialization
	void Start()
	{
		m_rRigidbody = GetComponent<Rigidbody>();
		m_vHalfExtents = transform.lossyScale * 0.49f;
		m_vDefaultLocalPosition = transform.localPosition;
	}

	void FixedUpdate()
	{
		if ( m_oParent )
			FixParentPosition();


		if ( !isServer ) return;

		// 落下判定
		if ( Physics.BoxCast(transform.position, m_vHalfExtents, Vector3.down, transform.rotation, 0.1f) )
		{
			// 落下しない
			m_rRigidbody.isKinematic = true;
		}
		else
		{
			// 落下する
			m_rRigidbody.isKinematic = false;
		}

	}

	// NetworkTransformを付けていない親オブジェクト（スイッチの土台など）も位置同期させる場合に使う
	void FixParentPosition()
	{
		var deltaPos = transform.localPosition - m_vDefaultLocalPosition;
		m_oParent.transform.position += deltaPos;

		transform.localPosition = m_vDefaultLocalPosition;
	}
}