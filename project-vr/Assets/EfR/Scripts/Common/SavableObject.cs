using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavableObject : MonoBehaviour {

    private void Start()
    {
        StageSaver.GetInstance().RegisterSavableObject(this);
    }
}
