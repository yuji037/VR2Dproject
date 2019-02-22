using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickObjectScaler : GimmickBase{
    [SerializeField]
    int triggerID = 1;

    Vector3 defaultScale;

    [SerializeField]
    Vector3 toScale;

    [SerializeField]
    Transform scaleChangeTarget;

    [SerializeField]
    float changeDuration;

    Coroutine currentCoroutine;
	// Use this for initialization
	void Start () {
        isCallOnlyServer = false;
        defaultScale = scaleChangeTarget.localScale;

        m_aTriggerEnterAction += ChangeScale;
        m_aTriggerExitAction += ResetScale;
    }

    void ChangeScale(int id)
    {
        if (id==triggerID)
        {
            StartChangeScale(toScale);
        }
    }

    void ResetScale(int id)
    {
        if (id == triggerID)
        {
            StartChangeScale(defaultScale);
        }
    }

    void StartChangeScale(Vector3 toScale)
    {
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        currentCoroutine=StartCoroutine(ChangeScaleRoutine(toScale));
    }

	IEnumerator ChangeScaleRoutine(Vector3 toScale) {
        var fromScale = scaleChangeTarget.localScale;
        for (float t=0;t<changeDuration;t+=Time.deltaTime)
        {
            if (!gameObject.activeSelf) yield break;
            scaleChangeTarget.localScale=Vector3.Lerp(fromScale,toScale,t/changeDuration);
            yield return null;
        }
        //一応最後に代入
        scaleChangeTarget.localScale = toScale;
	}
}
