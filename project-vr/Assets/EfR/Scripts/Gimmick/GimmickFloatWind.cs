using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickFloatWind : MonoBehaviour {

    [SerializeField]
    float floatPower;

	// Update is called once per frame
	void Update () {
        var colliders = Physics.OverlapBox(transform.position,transform.lossyScale*0.5f);
        foreach (var collider in colliders)
        {
            if (collider.gameObject==PlayerManager.LocalPlayer)
            {
                PlayerManager.playerMove.FloatPlayer(floatPower, false);
            }
        }
	}
}
