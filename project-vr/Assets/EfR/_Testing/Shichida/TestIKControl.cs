using UnityEngine;
using System;
using System.Collections;

[RequireComponent( typeof( Animator ) )]

public class TestIKControl : MonoBehaviour {

	protected Animator animator;

	public bool ikActive = false;
	public Transform HeadJoint = null;
	public Transform rightHandObj = null;
	public Transform rightFootObj = null;
	public Transform leftFootObj = null;
	public Transform lookObj = null;

	public float lookAtWeight = 1.0f;

	void Start()
	{
		animator = GetComponent<Animator>();
	}

	// IK を計算するためのコールバック
	void OnAnimatorIK()
	{
		if ( animator )
		{

			// IK が有効ならば、位置と回転を直接設定します
			if ( ikActive )
			{

				// すでに指定されている場合は、視線のターゲット位置を設定します
				if ( lookObj != null )
				{
					animator.SetLookAtWeight(lookAtWeight );
					animator.SetLookAtPosition(lookObj.position
						/*HeadJoint.position + VRObjectManager.GetInstance().VRCamObject.transform.rotation * Vector3.forward*/ );
				}

				// 指定されている場合は、右手のターゲット位置と回転を設定します
				if ( rightHandObj != null )
				{
					animator.SetIKPositionWeight( AvatarIKGoal.RightHand, 1 );
					animator.SetIKRotationWeight( AvatarIKGoal.RightHand, 1 );
					animator.SetIKPosition( AvatarIKGoal.RightHand, rightHandObj.position );
					animator.SetIKRotation( AvatarIKGoal.RightHand, rightHandObj.rotation );
				}
				if ( rightFootObj != null )
				{
					animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
					animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1);
					animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootObj.position);
					animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootObj.rotation);
				}
				if ( leftFootObj != null )
				{
					animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
					animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
					animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootObj.position);
					animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootObj.rotation);
				}
			}

			//IK が有効でなければ、手と頭の位置と回転を元の位置に戻します
			else
			{
				animator.SetIKPositionWeight( AvatarIKGoal.RightHand, 0 );
				animator.SetIKRotationWeight( AvatarIKGoal.RightHand, 0 );
				animator.SetLookAtWeight( 0 );
			}
		}
	}
}