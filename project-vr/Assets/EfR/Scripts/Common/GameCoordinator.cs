using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCoordinator : MonoBehaviour {

    [SerializeField]
    string firstLoadScene;

	// Use this for initialization
	void Start () {
        StartCoroutine(SceneLoader.LoadScene(firstLoadScene));
	}
}
