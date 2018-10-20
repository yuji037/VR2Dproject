using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickManager : SingletonMonoBehaviour<GimmickManager> {

    Dictionary<int, Gimmick> gimmickDictionary = new Dictionary<int, Gimmick>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Register(Gimmick gimmick)
    {
        gimmickDictionary.Add(gimmick.ID, gimmick);
    }

    public void ReceiveMessage(int id, string message)
    {
        gimmickDictionary[id].SendMessage(message);
    }
}
