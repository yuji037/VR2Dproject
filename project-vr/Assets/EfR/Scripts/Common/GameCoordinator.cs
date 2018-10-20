using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : MonoBehaviour {

    [SerializeField]
    string firstLoadScene;

	// Use this for initialization
	void Start () {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
            firstLoadScene
            , UnityEngine.SceneManagement.LoadSceneMode.Additive);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
