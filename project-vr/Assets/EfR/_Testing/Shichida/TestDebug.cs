using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebug : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if ( Input.GetKeyDown(KeyCode.M) )
        {
            Collision c = new Collision();
            var gmk = GimmickManager.GetGimmick(1001);
            gmk.SendMessage("OnCollisionEnter", c);
        }
	}
}
