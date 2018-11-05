using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// テスト用ギミック
// 作り方の参考にしてください
public class GimmickChangeColorRed : GimmickBase {

    private void Start()
    {
        m_aTriggerEnterAction += ChangeColorRed;
    }

    public void ChangeColorRed(Collider other, int otherGimmickID)
    {
        var ren = GetComponent<Renderer>();
        if ( ren ) ren.material.color = Color.red;
    }
}
