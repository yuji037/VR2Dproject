using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCoordinator : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(GameStartCoroutine());
    }

    IEnumerator GameStartCoroutine()
    {
        StartCoroutine(SceneLoader.IELoadScene("Root_Stage"));
        StartCoroutine(SceneLoader.IELoadScene("Root_Frame3D"));
        StartCoroutine(SceneLoader.IELoadScene("Root_UI"));
    }
}
