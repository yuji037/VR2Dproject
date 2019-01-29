using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransformUtility : SingletonMonoBehaviour<TransformUtility>
{
    static public void TransSamePosRot(Transform from, Transform to, float duration = 1.0f)
    {
        GetInstance().StartCoroutine(GetInstance().TransSamePosRotCoroutine(from, to.position, to.rotation, duration));
    }

    static public void TransSamePosRot(Transform from, Vector3 toPos, Quaternion toRot, float duration = 1.0f)
    {
        GetInstance().StartCoroutine(GetInstance().TransSamePosRotCoroutine(from, toPos, toRot, duration));
    }

    static public void TransSamePosRot(Transform from, Func<Vector3> getPos, Func<Quaternion> getRot, float duration=1.0f)
    {
        GetInstance().StartCoroutine(GetInstance().TransSamePosRotCoroutine(from,getPos,getRot,duration));
    }

    IEnumerator TransSamePosRotCoroutine(Transform from, Vector3 toPos, Quaternion toRot, float duration)
    {
        Vector3 defPos = from.position;
        Quaternion defRot = from.rotation;
        float diffAngle = Vector3.Angle(from.forward, toRot * Vector3.forward);
        Debug.Log(diffAngle);
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            from.position = defPos * (duration - t) / duration + toPos * t / duration;
            from.rotation = Quaternion.RotateTowards(defRot, toRot, diffAngle * t / duration);
            yield return null;
        }
        from.position = toPos;
        from.rotation = toRot;
    }
    //動的にposとrotが変わる場合こっち使う
    IEnumerator TransSamePosRotCoroutine(Transform from, Func<Vector3> getPos, Func<Quaternion> getRot, float duration)
    {
        Vector3 defPos = from.position;
        Quaternion defRot = from.rotation;
        Vector3 defForward = from.forward;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float diffAngle = Vector3.Angle(defForward, getRot() * Vector3.forward);
            from.position = defPos * (duration - t) / duration + getPos() * t / duration;
            from.rotation = Quaternion.RotateTowards(defRot, getRot(), diffAngle * t / duration);
            yield return null;
        }
    }
}
