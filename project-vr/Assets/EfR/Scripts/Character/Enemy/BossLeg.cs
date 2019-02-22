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

    [SerializeField]
    MeteoFaller meteoFaller;

    [SerializeField]
    int minFall=1;

    [SerializeField]
    int maxFall=10;

    public bool isStampFalling;

    public bool IsBreak { get; private set; }

    Coroutine currentStampRoutine;

    Transform shockWavePoint;

    [SerializeField]
    float stampEndTime=3.0f;

    public System.Action OnStopStampAction;

    private void Start()
    {
        animator = GetComponent<Animator>();
        shockWavePoint = transform.Find("ShockWavePoint");
        if (isServer)
        {
            bottomHitNotifier.Initialize(CheckHitBottomObject);
        }
    }


    //下に指定したIDのギミックがある場合踏み付け停止、プレイヤーだった場合はリスポーン
    void CheckHitBottomObject(Collider collider)
    {
        if (!isServer||!isStampFalling) return;

        var Gimmick = collider.GetComponent<GimmickBase>();
        if (Gimmick)
        {
            if (Gimmick.GimmickID==1)
            {
                Gimmick.GetComponent<PlayerMove>().RpcRespawn();
            }
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
        if(OnStopStampAction!=null)OnStopStampAction();
    }

    IEnumerator ShockWaveRoutine()
    {
        animator.CrossFade("Stamp", 0f);
        animator.SetFloat("Speed",1.0f/stampEndTime);
        yield return new WaitForSeconds(stampEndTime);

        //エフェクト発生
        var obj = Instantiate(shockWavePrefab);
        obj.transform.position = shockWavePoint.position;
        obj.transform.rotation = Quaternion.identity;
        if (isServer)
        {
            //ゲームオーバー処理
            Debug.Log("GameOver");
            GameCoordinator.GetInstance().StartGameOverPerformance();
            //var rand = Random.Range(minFall,maxFall+1);
            //for(int i=0;i<=rand;i++)
            //{
            //    meteoFaller.FallMeteo();
            //}
        }
    }
}
