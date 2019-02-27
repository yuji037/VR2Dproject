using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1つしか存在してはいけない&&MonoBehaviourを継承。

public class SingletonMonoBehaviour<T> : MonoBehaviour
where T : SingletonMonoBehaviour<T>
{

    static T instance;
    //debug用
    static T Instance
    {
        set { instance = value;}
        get { return instance; }
    }
    public static T GetInstance()
    {
        if (!Instance)
        {
            var obj = new GameObject("SingletonEmpty");
            Instance = obj.AddComponent<T>();
        }
        return Instance;
    }

    public static bool CheckInstance { get { return instance;} }

    // ※継承先クラスでAwakeを使う場合はbase.Awake()を呼ぶ。
    protected virtual void Awake()
    {
        // staticのinstanceがnullの場合自分を入れる。
        if (Instance == null)
        {
            Instance = (T)this;
        }

        // staticのinstanceが自分じゃない場合エラー
        if (Instance != this)
        {
            Debug.LogError(typeof(T) + "はSingletonなのに、2つ目が存在している。2つ目：" + gameObject);
            //Debug.Log(
            //typeof(T) +
            //   " は既に" + Instance.gameObject.name + "にアタッチされているため" +
            //   "このinstanceを破棄しました。");
            //Destroy(this);
        }
    }
    protected void OnDestroy()
    {
        instance = null;
    }
}

