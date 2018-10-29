using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickActivate : GimmickBase {

    [SerializeField]
    bool isDefaultActive = false;

    private void Start()
    {
        gameObject.SetActive(isDefaultActive);
    }
}
