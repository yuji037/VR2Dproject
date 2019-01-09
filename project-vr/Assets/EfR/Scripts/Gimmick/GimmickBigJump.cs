using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBigJump : GimmickBase {
    [SerializeField]
    float jumpPower=1.0f;

    // Use this for initialization
	void Start () {
        isCallingWithServer = false;
        m_acCollisionEnterAction += BigJumpPlayer;
        m_acTriggerEnterAction += BigJumpPlayer;
	}

    void BigJumpPlayer(Collider collision)
    {
        Debug.Log("BigJump");
        if (collision.gameObject == PlayerManager.LocalPlayer)
        {
            Debug.Log("JumpLocalPlayer");
            var pm = collision.gameObject.GetComponent<PlayerMove>();
            pm.Jump(jumpPower);
        }
    }
    void BigJumpPlayer(Collision collision)
    {
        Debug.Log("BigJump");
        if (collision.gameObject==PlayerManager.LocalPlayer)
        {
            Debug.Log("JumpLocalPlayer");
            var pm =collision.gameObject.GetComponent<PlayerMove>();
            pm.Jump(jumpPower);
        }
    }
}
