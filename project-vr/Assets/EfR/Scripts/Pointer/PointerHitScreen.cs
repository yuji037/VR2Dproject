using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHitScreen : MonoBehaviour
{
    public enum PlayerNumber{
        Player1=0,
        Player2=1,
    }
    [SerializeField]
    FloorForm floorForm;
    [SerializeField]
    public PlayerNumber canCreatePlayerNumber;


    public FloorForm GetFloorForm
    {
        get { return floorForm; }
    }
}
