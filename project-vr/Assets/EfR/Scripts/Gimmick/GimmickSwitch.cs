using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickSwitch : GimmickBase {


    [SerializeField]
    SwitchActionBase switchAction;

    [SerializeField, Header("押せる物体のギミックID（プレイヤーの手、ブロックなど）")]

    int[]		m_iTriggerGimmickIDs;


	[SerializeField, Header("光線ポインターで反応するかどうか")]

	bool		m_IsActPointerHit;

	[SerializeField, Header("スイッチを離したときにもアクションするか")]

	bool		m_IsActOnRelease;


	[SerializeField, Header("押したときの位置変化ベクトル")]

	Vector3		m_vPushedVector	= new Vector3(0, -0.2f, 0);

	[SerializeField, Header("押せるスピード")]

	float		m_fPerformSpeed		= 1.0f;

	[SerializeField, Header("トグルスイッチかどうか（1回押すとON、もう1回押すとOFF）")]

	bool		m_IsToggle			= false;



	#region 内部変数

	[SerializeField]
	bool		m_IsPushing			= false;
	bool		m_IsPressed			= false;
	bool		m_IsToggleOn		= false;

	Rigidbody	m_rRigidbody;

	Vector3		m_vReleasedPosition;
	Vector3		m_vPressedPosition;
	float		m_fPressedDistanceSqr;

	#endregion


	// Use this for initialization
	void Start()
	{

		m_aTriggerEnterAction		+= PushJudge;
		// 光線でONになるかどうか
		if ( m_IsActPointerHit )
			m_aPointerHitAction		+= PushJudge;

		m_aTriggerExitAction		+= ReleaseJudge;

		// 内部変数への入れ込み
		m_rRigidbody = GetComponent<Rigidbody>();

		m_vReleasedPosition			= transform.localPosition;
		m_vPressedPosition			= transform.localPosition + m_vPushedVector;
		m_fPressedDistanceSqr		= ( m_vPressedPosition - m_vReleasedPosition ).sqrMagnitude;
	}

	private void Update()
	{
		if ( m_IsPushing )
		{
			OnPushing();
		}
		else
		{
			OnReleasing();
		}
	}

	// スイッチが沈み切るとアクションが起こる
	void OnPushing()
	{
		Vector3		nowPressedVector		=	transform.localPosition - m_vReleasedPosition;
		float		nowPressedDistanceSqr	=	nowPressedVector.sqrMagnitude;


		if (		nowPressedDistanceSqr < m_fPressedDistanceSqr )
		{
			// スイッチが沈んでいる途中
			PressThisFrame();
		}
		else
		{
			// スイッチが沈み切った
			if ( m_IsPressed ) return;
			
			m_IsPressed		= true;

			if ( m_IsToggle )
			{
				// トグルスイッチの場合
				if ( m_IsToggleOn == false )
                    switchAction.OnAction();
				else
                    switchAction.OffAction();

				m_IsToggleOn = !m_IsToggleOn;
			}
			else
			{
                // トグルスイッチじゃない場合
                switchAction.OnAction();
			}
			transform.localPosition = m_vPressedPosition;
		}
	}

	void OnReleasing()
	{
		Vector3		nowReleasedVector		= transform.localPosition - m_vPressedPosition;
		float		nowReleasedDistanceSqr	= nowReleasedVector.sqrMagnitude;

		if (		nowReleasedDistanceSqr	< m_fPressedDistanceSqr )
		{
			// スイッチが浮かんでいる途中
			// 離してアクションするタイプならここでアクション
			if ( m_IsPressed && m_IsActOnRelease)
                switchAction.OffAction();

			ReleaseThisFrame();
			m_IsPressed = false;
		}
		else
		{
			// スイッチが浮かび切った
			// スイッチに何も触れてない定常時

			transform.localPosition = m_vReleasedPosition;
		}
	}

	void PressThisFrame()
	{
		var moveVec =	m_vPushedVector * m_fPerformSpeed * Time.deltaTime;
		m_rRigidbody.MovePosition(transform.position + moveVec);
	}

	void ReleaseThisFrame()
	{
		var moveVec = -	m_vPushedVector * m_fPerformSpeed * Time.deltaTime;
		m_rRigidbody.MovePosition(transform.position + moveVec);
	}

	protected virtual void PushJudge(int otherGimmickID)
	{
		Debug.Log("Push");
		foreach ( var triggerID in m_iTriggerGimmickIDs )
		{
			if ( otherGimmickID == triggerID )
			{
				m_IsPushing = true;
			}
		}
	}

	protected virtual void ReleaseJudge(int otherGimmickID)
	{
		Debug.Log("Release");
		foreach ( var triggerID in m_iTriggerGimmickIDs )
		{
			if ( otherGimmickID == triggerID )
			{
				m_IsPushing = false;
			}
		}
	}

}
