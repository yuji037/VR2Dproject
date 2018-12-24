using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtention
{

    /// <summary>
	/// 指定した名前でGameObjectを取得出来るまで、毎フレームGameObject.Find()を繰り返す。
	/// </summary>
	/// <param name="callBack">取得に成功したときに行う、GameObjectが引数のAction</param>
    public static void GetGameObjectWithCoroutine(this MonoBehaviour mono, string gameObjectName, System.Action<GameObject> callBack)
    {
        mono.StartCoroutine(FindCoroutine(gameObjectName, callBack));
    }
    static IEnumerator FindCoroutine(string gameObjectName, System.Action<GameObject> callBack)
    {
        GameObject obj = null;
        while (!(obj = GameObject.Find(gameObjectName)))
        {
            Debug.Log(gameObjectName + "探し中");
            yield return null;
        }
        Debug.Log(gameObjectName + "取得完了");
        callBack(obj);
    }
}
