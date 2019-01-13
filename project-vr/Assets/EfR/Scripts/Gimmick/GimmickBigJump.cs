using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBigJump : GimmickBase {
    [SerializeField]
    float jumpPower=1.0f;

    // Use this for initialization
	void Start () {
        isCallingWithServer = false;
        m_acTriggerEnterAction += BigJumpPlayer;
	}

    void BigJumpPlayer(Collider collision)
    {
        if (collision.gameObject == PlayerManager.LocalPlayer)
        {
            var pm = collision.gameObject.GetComponent<PlayerMove>();
            pm.Jump(jumpPower);
        }
    }
}
