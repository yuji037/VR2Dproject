using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;

public class WorldCollapser : NetworkBehaviour
{
    [SerializeField]
    float[] limitTimerEachSecction;

    [SerializeField]
    class CollapseTargets
    {
        public Transform root;
        [HideInInspector] public List<Transform> children = new List<Transform>();
    }
    List<CollapseTargets>[] sectionObjects = new List<CollapseTargets>[2];
    [SerializeField]
    float oneCollapseDelay=0.4f;

    int currentSecction;
    float timer = 0f;
    private void Update()
    {
        if (currentSecction >= limitTimerEachSecction.Length) return;
        timer += Time.deltaTime;
        if (limitTimerEachSecction[currentSecction] <= timer)
        {
            Collapse(currentSecction);
            currentSecction++;
            timer = 0f;
        }
    }

    // Use this for initialization
    void Start()
    {

        SetSectionObject(0);
        SetSectionObject(1);
    }

    void SetSectionObject(int pNum)
    {
        var pRoot = GameObject.Find("P" + (pNum + 1) + "Objects");
        sectionObjects[pNum] = new List<CollapseTargets>();
        Transform secRoot;
        int sectionNum = 1;
        secRoot = pRoot.transform.Find("section" + sectionNum);
        while (secRoot)
        {
            sectionObjects[pNum].Add(new CollapseTargets());
            sectionObjects[pNum][sectionNum - 1].root = secRoot;
            foreach (Transform i in secRoot)
            {
                if (i != secRoot)
                {
                    sectionObjects[pNum][sectionNum - 1].children.Add(i);
                }
            }
            sectionObjects[pNum][sectionNum - 1].children = sectionObjects[pNum][sectionNum - 1].children.OrderBy((a) => a.transform.position.x).ToList();
            sectionNum++;
            secRoot = pRoot.transform.Find("section" + sectionNum);
            if(secRoot)Debug.Log(secRoot.name+sectionNum);
        }
    }

    public void Collapse(int sectionNum)
    {
        StartCoroutine(CollapseRoutine(0, sectionNum));
        StartCoroutine(CollapseRoutine(1, sectionNum));
    }
    IEnumerator CollapseRoutine(int pNum, int sectionNum)
    {
        Debug.Log("aa"+sectionNum);
        foreach (var child in sectionObjects[pNum][sectionNum].children)
        {
            if(!child.gameObject.GetComponent<Rigidbody>()&&
                !child.gameObject.GetComponent<MeshCollider>()) child.gameObject.AddComponent<Rigidbody>();
            yield return new WaitForSeconds(oneCollapseDelay);
        }
        if(Network.isServer&&
            sectionNum >= sectionObjects[pNum].Count - 1)
        {
            CmdEnd();
        }
    }
    [Command]
    void CmdEnd()
    {
        RpcEnd();
    }

    [ClientRpc]
    void RpcEnd()
    {
        PlayerRespawner.GetInstance().GameOverDirectly();
    }
}
