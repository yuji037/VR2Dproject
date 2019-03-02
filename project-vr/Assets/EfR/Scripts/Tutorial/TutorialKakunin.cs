using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKakunin : MonoBehaviour {

	static TutorialKakunin instance = null;

	public static TutorialKakunin GetInstance()
	{
		return instance;
	}

	private void Start()
	{
		instance = this;
		SetActiveThisObj(false);
	}

	private void Update()
	{
		if( Input.GetKeyDown(KeyCode.M) )
		{
			SetActiveThisObj(true);
		}
	}

	public void SetActiveThisObj(bool isActive)
    {
        SetActiveObj(gameObject, isActive);
    }

    void SetActiveObj(GameObject obj, bool isActive)
    {
        var rens = obj.GetComponentsInChildren<Renderer>();
        foreach (var ren in rens)
            ren.enabled = isActive;

        var cols = obj.GetComponentsInChildren<Collider>();
        foreach (var col in cols)
            col.enabled = isActive;
    }
}
