using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickChangeColorRed : Gimmick {
    
    public void ChangeColorRed()
    {
        var ren = GetComponent<Renderer>();
        if ( ren ) ren.material.color = Color.red;
    }
}
