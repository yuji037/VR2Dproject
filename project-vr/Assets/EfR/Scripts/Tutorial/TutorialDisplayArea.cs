using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDisplayArea : MonoBehaviour {

	LayerMask playerLayer;

	[SerializeField]
	TutorialObject tutorialObject;

	TutorialObject tutorialObjectIns;

	// Use this for initialization
	void Start () {
		playerLayer = LayerMask.NameToLayer("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == PlayerManager.LocalPlayer )
		{
			var obj = Instantiate(tutorialObject.gameObject);
			tutorialObjectIns = obj.GetComponent<TutorialObject>();
			tutorialObjectIns.SetParent();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ( other.gameObject == PlayerManager.LocalPlayer )
		{
			Destroy(tutorialObjectIns.gameObject);
		}
	}
}
