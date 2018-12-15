using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FPS時、2D時など視点によってプレイヤーの動き方を決めるもの。
/// </summary>
[CreateAssetMenu(fileName = "PlayerMoveSettings", menuName = "Create PlayerMoveSettings", order = 0)]
public class PlayerMoveSettings : ScriptableObject
{

    public bool canJump;
    public float jumpPower;
    public float limitSpeed;
    public float groundMoveAccel;
    public float airMoveAccel;
    public float gravity;
    //public float airVerticalResistance;
    public float airResistance;
    public float groundRegistance;
    public float distanceToGround;

	public float jumpingDuration = 0.5f;
}
