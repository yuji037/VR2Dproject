using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerMoveSettings", menuName = "Create PlayerMoveSettings", order = 0)]
public class PlayerMoveSettings : ScriptableObject
{

    public bool canJump;
    public float jumpPower;
    public float limitSpeed;
    public float groundMoveAccel;
    public float airMoveAccel;
    public float gravity;
    public float airVerticalResistance;
    public float airResistance;
    public float groundFriction;
    public float distanceToGround;
}
