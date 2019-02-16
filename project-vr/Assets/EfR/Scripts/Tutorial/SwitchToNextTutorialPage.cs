using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToNextTutorialPage : MonoBehaviour {

	[SerializeField]
	TutorialObject nextTutorialObject;

	// Update is called once per frame
	void Update () {
		if( OVRInput.GetDown(OVRInput.Button.One) )
		{
			Destroy(gameObject);
			if ( nextTutorialObject )
			{
				var obj = Instantiate(nextTutorialObject.gameObject);
				var tutorialObjectIns = obj.GetComponent<TutorialObject>();
				tutorialObjectIns.SetParent();
				tutorialObjectIns.Init();
				tutorialObjectIns.transform.localPosition = Vector3.zero;
				tutorialObjectIns.transform.localRotation = Quaternion.identity;
			}
			else
				PlayerManager.LocalPlayer.GetComponent<PlayerMove>().canMove = true;
		}
	}
}
