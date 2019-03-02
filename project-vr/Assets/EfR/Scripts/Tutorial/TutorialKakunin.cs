using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialKakunin : MonoBehaviour {

	private void Start()
	{
		foreach(var col in GetComponentsInChildren<Collider>() )
		{
			col.enabled = false;
		}
	}

	private void Update()
	{
		if( Input.GetKeyDown(KeyCode.M) )
		{
			foreach ( var col in GetComponentsInChildren<Collider>() )
			{
				col.enabled = true;
			}
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
