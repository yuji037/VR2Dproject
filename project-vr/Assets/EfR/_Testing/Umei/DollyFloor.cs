using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//プレイヤーが同じ床に乗ることは無いはずなので、位置同期は行わない。
public class DollyFloor : MonoBehaviour{
    [SerializeField]
    public CinemachineSmoothPath path;

    [SerializeField]
    float defaultPathValue;

    [SerializeField]
    float moveSpeed;

    Rigidbody rigidbody;

    float currentPathValue;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentPathValue = defaultPathValue;
    }
    private void FixedUpdate()
    {
         var next= path.EvaluatePosition(currentPathValue);
        rigidbody.MovePosition(next);
        currentPathValue += moveSpeed*Time.fixedDeltaTime;
    }
}
