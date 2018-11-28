using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IOPoint:MonoBehaviour
{
    private void Awake()
    {
        if (!this.ownerLine)
        {
            var owner = transform.parent.GetComponent<TransmissionLine>();
            if (owner) this.ownerLine = owner;
            else
            {
                Debug.LogError("ポイントの親にラインが存在しません");
            }
        }
    }
    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
    }
    //自分を持っているライン
    TransmissionLine ownerLine;
    public TransmissionLine OwnerLine
    {
        get { return ownerLine; }
    }
    public bool IsPowerOn
    {
        get
        {
            return ownerLine.IsPowerOn;
        }
        set
        {
            ownerLine.IsPowerOn = value;
        }
    }
    public IOPoint(TransmissionLine ownerLine)
    {
        this.ownerLine = ownerLine;
    }
}