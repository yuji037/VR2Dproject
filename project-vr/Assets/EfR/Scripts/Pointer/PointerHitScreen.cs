using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerHitScreen : MonoBehaviour {
    [SerializeField]
    FloorForm floorForm;
	public FloorForm GetFloorForm
    {
        get { return floorForm; }
    }
}
