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
}
