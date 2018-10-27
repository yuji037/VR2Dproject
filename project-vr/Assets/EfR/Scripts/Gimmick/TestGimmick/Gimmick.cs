using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gimmick : MonoBehaviour {

    [SerializeField]
    public int ID;

    [System.Serializable]
    public class SendEvent {
        public int gimmickID;
        public string sendMessage;
    }

    [SerializeField]
    protected SendEvent[] sendEventList;

    // ※継承先でAwakeを使いたい場合はbase.Awake()を呼ぶ。
    protected virtual void Awake()
    {
        GimmickManager.GetInstance().Register(this);
    }
}
