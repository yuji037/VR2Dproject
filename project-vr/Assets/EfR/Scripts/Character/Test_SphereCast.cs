using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_SphereCast : MonoBehaviour {

    private RaycastHit hit;
    private bool isHit = false;

    [SerializeField]
    LayerMask layerMask;

    [SerializeField]
    float radius;


    private void OnTriggerStay(Collider other)
    {
        Vector3 distance = other.transform.position - transform.position;
        float x = Mathf.Abs(distance.x);
        float z = Mathf.Abs(distance.z);

        if (x > z) { distance.z = 0; }
        else { distance.x = 0; }
        distance.y = 0;

        Vector3.Normalize(distance);
        
        // 押した先に当たり判定がある物体が存在するか確認しなければ押せるようにする


        other.transform.Translate(distance * Time.deltaTime);
    }
}
