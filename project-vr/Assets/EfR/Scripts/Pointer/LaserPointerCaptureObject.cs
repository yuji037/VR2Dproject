using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointerCaptureObject : LaserPointerBase {
    Rigidbody captureObject;
    Vector3 capturedPoint;
    [SerializeField]
    float capturePower=10.0f;
    protected override void HitAction(RaycastHit hit, Vector3 origin, Vector3 direction)
    {
        if (!isLocalPlayer) return;
        var wallBreaker=hit.transform.GetComponent<GimmickWallBreaker>();
        if (Input.GetKeyDown(KeyCode.Mouse0)&& wallBreaker)
        {
            Debug.Log("キャプチャー"+hit.transform);
            captureObject=wallBreaker.GetComponent<Rigidbody>();
            capturedPoint = origin;
        }
        FollowOrigin(origin);
    }
    protected override void NoHitAction(Vector3 origin, Vector3 direction)
    {
        base.NoHitAction(origin, direction);
        FollowOrigin(origin);
    }
    void FollowOrigin(Vector3 currentOrigin)
    {
        if (Input.GetKey(KeyCode.Mouse0)&&captureObject)
        {
            var moveVec = currentOrigin - capturedPoint;
            captureObject.AddForce(moveVec*capturePower,ForceMode.Impulse);
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            Debug.Log("キャプチャーオフ"+captureObject);
            captureObject = null;
        }
    }
}
