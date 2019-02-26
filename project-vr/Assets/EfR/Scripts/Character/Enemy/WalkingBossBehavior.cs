using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;

public class WalkingBossBehavior : BossBehaviorBase
{
    [SerializeField]
    Transform[] stampPoints;

    [SerializeField]
    BossLeg[] bossLegs;

    [SerializeField]
    GameObject[] objectsEachSecctions;

    [SerializeField]
    float[] moveSpeedEachSecctions;

    [SerializeField]
    GameObject collapseBridgeRoot;

    [SerializeField]
    Animator bossAnimator;

    [SerializeField]
    float collapseDelay = 0.6f;

    [SerializeField]
    Material bridgeMaterial;

    float speed = 1.0f;

    public override void StartBehavior()
    {
        if (!isServer) return;
        DebugTools.RegisterDebugAction(KeyCode.Alpha9, () => speed += 1.0f, "bossSpeedUp");
        DebugTools.RegisterDebugAction(KeyCode.Alpha8, () => speed -= 1.0f, "bossSpeedDown");
        StartCoroutine(MoveRoutine());
    }

    public override void StopBehavior()
    {
        Debug.LogError("このAIは停止不可");
    }

    IEnumerator MoveRoutine()
    {

        int currentSecction = 0;
        while (bossLegs.Length > currentSecction)
        {
            bool isStopped = false;
            bossLegs[currentSecction].OnStopStampAction += () =>
            {
                isStopped = true;
            };

            //走る
            while (stampPoints[currentSecction].position.x > transform.position.x)
            {
                transform.Translate(moveSpeedEachSecctions[currentSecction] * speed * Time.deltaTime, 0, 0, Space.World);
                yield return null;
            }
            bossLegs[currentSecction].RpcStartStamp();
            yield return new WaitUntil(() => isStopped);
            yield return new WaitForSeconds(2.0f);
            var gimmicks = objectsEachSecctions[currentSecction].GetComponentsInChildren<GimmickBase>();
            foreach (var i in gimmicks)
            {
                i.DestroyThisObject();
            }
            RpcNonActiveSecctionObjects(currentSecction);
            currentSecction++;
        }
        RpcCollapseBridge();
    }
    [ClientRpc]
    void RpcCollapseBridge()
    {
        StartCoroutine(CollapseRoutine());
    }
    IEnumerator CollapseRoutine()
    {
        bossAnimator.CrossFade("Beam", 0f);
        var bridges = collapseBridgeRoot.GetComponentsInChildren<Rigidbody>();
        yield return new WaitForSeconds(1.0f);
        foreach (var i in bridges)
        {
            i.GetComponent<MeshRenderer>().material = bridgeMaterial;
        }
        var sortedbridges = bridges.OrderBy((x) => x.transform.position.x);
        foreach (var i in sortedbridges)
        {
            i.isKinematic = false;
            yield return new WaitForSeconds(collapseDelay);
        }
    }


    [ClientRpc]
    void RpcNonActiveSecctionObjects(int secctionNum)
    {
        objectsEachSecctions[secctionNum].SetActive(false);
    }


}
