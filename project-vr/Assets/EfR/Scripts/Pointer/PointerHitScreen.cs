using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHitScreen : MonoBehaviour
{
    [SerializeField]
    FloorForm floorForm;
    [SerializeField]
    public PlayerNumber canCreatePlayerNumber;


    public FloorForm GetFloorForm
    {
        get { return floorForm; }
    }
}
