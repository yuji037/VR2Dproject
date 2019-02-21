using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour {

    [SerializeField]
    public float destroyTime = 3f;

    public virtual void SetAttachTarget(int num, string targetName)
	{

	}

	public virtual void SetEndPosition(Vector3 endPos)
	{

	}
}
