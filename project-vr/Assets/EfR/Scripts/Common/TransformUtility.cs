using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformUtility : SingletonMonoBehaviour<TransformUtility>
{
    static public void TransPosAndRotToEqualize(Transform from, Transform to,float duration=1.0f)
    {
        GetInstance().StartCoroutine(GetInstance().TransPosAndRotToEqualizeCoroutine(from, to,duration));
    }
    IEnumerator TransPosAndRotToEqualizeCoroutine(Transform from, Transform to,float duration)
    {
        Vector3 defPos = from.position;
        Quaternion defRot = from.rotation;
        float diffAngle = Vector3.Angle(from.forward, to.forward);
        Debug.Log(diffAngle);
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            from.position = defPos * (duration - t) / duration + to.position * t / duration;
            from.rotation = Quaternion.RotateTowards(defRot, to.rotation, diffAngle * t / duration);
            yield return null;
        }
    }
}
