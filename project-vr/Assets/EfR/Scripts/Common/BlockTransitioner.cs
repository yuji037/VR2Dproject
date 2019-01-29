using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTransitioner : MonoBehaviour {

    [SerializeField]
    GameObject fromParent;

    [SerializeField]
    GameObject toParent;

    [SerializeField]
    string fromObjectPrefix;

    [SerializeField]
    string toObjectPrefix;

    [SerializeField]
    float oneTransTime;


    public void StartTransBlocks()
    {
        StartCoroutine(TransRoutine());
    }
	IEnumerator TransRoutine()
    {

        var fromObjs = fromParent.GetTransformsOrderByNumber(fromObjectPrefix);
        var toObjs = toParent.GetTransformsOrderByNumber(toObjectPrefix);
        Debug.Log("FROM"+fromObjs.Length);
        Debug.Log("TO" + toObjs.Length);
        for (int i=0;i<fromObjs.Length;i++)
        {
            TransformUtility.TransSamePosRot(fromObjs[i], toObjs[i],oneTransTime);
            yield return new WaitForSeconds(oneTransTime*0.5f);
        }
        

    }
}
