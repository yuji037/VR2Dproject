using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialObject : MonoBehaviour {

	[SerializeField]
	string parentName;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init()
	{
		gameObject.FindFirstChildByName("ControllerLeftModel");
	}

	public void SetParent()
	{
		transform.parent = GameObject.Find(parentName).transform;
	}
}
