using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walls : MonoBehaviour {

    [SerializeField]
    GameObject wall;

    [SerializeField]
    GameObject gimickWall;

    // Use this for initialization
    void Start () {
        for (int i = -30; i < 30; i++)
        {
            Instantiate(wall, new Vector3(0, 0, i*3), new Quaternion(0, 0, 0, 0));
        }
        int rand = Random.Range(0, 3);
        Debug.Log("rand : " + rand);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
