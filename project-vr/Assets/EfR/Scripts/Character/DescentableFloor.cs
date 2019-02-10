using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DescentableFloor : MonoBehaviour {

	[SerializeField]
	float judgeColliderCenterY = 1f;
	[SerializeField]
	Vector3 colliderHalfExtents = new Vector3(0.5f, 0.2f, 0.5f);
	[SerializeField]
	float maxDistance = 0.1f;

	float waitTimer = 0f;
	float colliderOnInterval = 0.5f;

	[SerializeField]
	Collider m_Collider;
	[SerializeField]
	Renderer m_Renderer;

	[SerializeField]
	LayerMask rideLayerMask;

	[SerializeField]
	Color gizmoColor = new Color(1f,0.4f,0.4f,0.3f);

	[SerializeField]
	bool debugDraw = false;

	// Use this for initialization
	void Start () {
		m_Collider = GetComponent<Collider>();
	}

	private void OnDrawGizmos()
	{
		if ( debugDraw )
		{
			Gizmos.color = gizmoColor;
			if ( m_Collider.enabled )
			{
				m_Renderer.enabled = true;
			}
			else
			{
				m_Renderer.enabled = false;
				Gizmos.DrawCube(
					transform.position + new Vector3(0, judgeColliderCenterY * transform.localScale.y, 0),
					Vector3.Scale(transform.localScale, colliderHalfExtents) * 2);
			}
		}
	}

	// Update is called once per frame
	void Update () {

		RaycastHit hit;
		bool isPlayerRide = Physics.BoxCast(
			transform.position + new Vector3(0, judgeColliderCenterY * transform.localScale.y, 0),
			Vector3.Scale(transform.localScale, colliderHalfExtents),
			Vector3.up,
			out hit,
			Quaternion.identity,
			maxDistance,
			rideLayerMask);

		if ( waitTimer >= 0 )
		{
			waitTimer -= Time.deltaTime;
		}
		// 乗っている
		if (isPlayerRide && waitTimer < 0 )
		{
			m_Collider.enabled = true;

			var pm = hit.collider.gameObject.GetComponent<PlayerMove>();
			if ( pm )
			{
				if ( pm.IsInputDescentFloor() )
				{
					m_Collider.enabled = false;
					waitTimer = colliderOnInterval;
				}

			}
		}
		// 乗ってない
		if ( !isPlayerRide )
			m_Collider.enabled = false;

		//m_Collider.enabled = isPlayerRide && isPlayerInputDescent;
	}

}
