using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingAct : MonoBehaviour {

  [SerializeField]
  float timeRotx;
  [SerializeField]
  float timeRoty;
  [SerializeField]
  float timeRotz;

  // Use this for initialization
  void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.Rotate(timeRotx, timeRoty, timeRotz);
	}
}
