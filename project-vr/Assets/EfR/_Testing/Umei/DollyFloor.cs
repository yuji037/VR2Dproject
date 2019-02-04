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

    [SerializeField]
    bool autoMove;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        currentPathValue = defaultPathValue;
        Move(1.0f);
    }
    private void FixedUpdate()
    {
        if (autoMove)
        {
            Move(1.0f);
        }
    }
    public void Move(float multiPlySpeed)
    {
        currentPathValue += moveSpeed *multiPlySpeed* Time.fixedDeltaTime;
        var next = path.EvaluatePosition(currentPathValue);
        rigidbody.MovePosition(next);
    }
}
