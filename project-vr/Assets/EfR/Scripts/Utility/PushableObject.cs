using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour {

    [SerializeField]
    LayerMask triggerLayerMask = -1;
    [SerializeField]
    float pushStrength = 0.5f;

    private void OnTriggerStay(Collider other)
    {
        var otherLayerMask = 1 << other.gameObject.layer;
        // 選択したレイヤー以外のオブジェクトに当たっても押されない
        if ( (otherLayerMask & triggerLayerMask.value) == 0 ) return;

        Vector3 distance = transform.position - other.transform.position;
        float x = Mathf.Abs(distance.x);
        float z = Mathf.Abs(distance.z);

        if ( x > z ) { distance.z = 0; }
        else { distance.x = 0; }
        distance.y = 0;

        distance /= distance.magnitude;

        //Debug.DrawLine

        // 押される方向に当たり判定がある物体があったら押されない
        if ( Physics.BoxCast(transform.position + distance, Vector3.one * 0.4f, distance, Quaternion.identity, 0.1f) )
            return;

        transform.Translate(distance * pushStrength * Time.deltaTime);
    }
}
