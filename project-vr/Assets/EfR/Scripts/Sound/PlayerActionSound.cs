using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionSound : MonoBehaviour {
    [SerializeField]
    PlayerMove playerMove;

    public void FootStep()
    {
        if (!playerMove.IsGrounded()) return;
        SoundManager.GetInstance().Play("run",transform.position,false);
    }

	//public void HitGround()
	//{
	//	//if ( !playerMove.IsGrounded() ) return;
	//	SoundManager.GetInstance().Play("landing", transform.position, false);
	//}

	public void PlaySE(string seName)
	{
		SoundManager.GetInstance().Play(seName, transform.position, false);
	}
}
