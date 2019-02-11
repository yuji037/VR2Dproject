using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BossLeg : NetworkBehaviour
{

    [SerializeField]
    GameObject shockWavePrefab;

    //下にオブジェクトがあることを通知
    [SerializeField]
    HitNotifier bottomHitNotifier;

    Animator animator;

    [SerializeField]
    int[] stampStoppableIDs;

    [SerializeField]
    ParticleSystem breakEffect;

    public bool isStampFalling;

    public bool IsBreak { get; private set; }

    Coroutine currentStampRoutine;

    Transform shockWavePoint;

    private void Start()
    {
        animator = GetComponent<Animator>();
        shockWavePoint = transform.Find("ShockWavePoint");
        if (isServer)
        {
            bottomHitNotifier.Initialize(CheckHitBottomObject);
        }
    }


    //下に指定したIDのギミックがある場合踏み付け停止
    void CheckHitBottomObject(Collider collider)
    {
        if (!isServer||!isStampFalling) return;
        var Gimmick = collider.GetComponent<GimmickBase>();
        if (Gimmick)
        {
            foreach (var i in stampStoppableIDs)
            {
                if (Gimmick.GimmickID == i)
                {
                    var time=animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    RpcStopStamp(time);
                    IsBreak = true;
                    return;
                }
            }
        }
    }

    [ClientRpc]
    public void RpcStartStamp()
    {
        currentStampRoutine = StartCoroutine(ShockWaveRoutine());
    }

    [ClientRpc]
    void RpcStopStamp(float animClipTime)
    {
        animator.CrossFade("ShockWavePoint", animClipTime);
        animator.speed = 0f;
        StopCoroutine(currentStampRoutine);
        breakEffect.Play();
        Debug.Log("止まった");
    }

    IEnumerator ShockWaveRoutine()
    {
        animator.CrossFade("Stamp", 0f);
        yield return new WaitForSeconds(3.0f);
        var obj = Instantiate(shockWavePrefab);
        obj.transform.position = shockWavePoint.position;
        obj.transform.rotation = Quaternion.identity;
    }

}
