using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DollyFloor : GimmickBase{
    [SerializeField]
    CinemachineSmoothPath path;

    [SerializeField]
    float defaultPos;

    [SerializeField]
    float moveSpeed;
    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(MoveCoroutine());
    }
    IEnumerator MoveCoroutine()
    {
        float currentValue =defaultPos;
        while(true)
        {
            transform.position=path.EvaluatePosition(currentValue);
            currentValue += Time.deltaTime*moveSpeed;
            yield return null;
        }
    }
}
