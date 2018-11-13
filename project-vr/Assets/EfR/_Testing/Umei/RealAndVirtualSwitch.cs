using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealAndVirtualSwitch : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    RealCameraAdjuster RCAdjuster;

    [SerializeField]
    ParticleSystem transRealParticle;

    ParticleSystem tgp;
    ParticleSystem transGameParticle
    {
        get
        {
            if(!tgp)tgp =  GameObject.Find("TransGameParticle").GetComponent<ParticleSystem>();
            return tgp;
        }
    }


    GameCameraAdjuster gcAd;
    GameCameraAdjuster GCAdjuster
    {
        get
        {
            if (!gcAd)
            {
                gcAd = GameObject.Find("GameCamera").GetComponent<GameCameraAdjuster>();
            }
            return gcAd;
        }
    }

    PlayerMove pMove;
    PlayerMove playerMove
    {
        get
        {
            if (!pMove)
            {
                pMove = GameObject.Find("Player").GetComponent<PlayerMove>();

            }
            return pMove;
        }


    }


    bool isReal = true;

    //遷移中
    bool isTrans;
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (isTrans) return;
            StartCoroutine(Translation());
        }
    }
    IEnumerator Translation()
    {
        isTrans = true;
        if (isReal)
        {
            transRealParticle.Play();
            yield return new WaitForSeconds(1.0f);
            playerMove.moveType = PlayerMove.MoveType.TPS;
            RCAdjuster.StartToVirtual();
            //realCamがTV画面まで近づいたらgameCameraを移動させる
            yield return new WaitForSeconds(2.0f);
            GCAdjuster.RealToVirtual();
            yield return new WaitForSeconds(2.1f);
            transRealParticle.Stop();
            //gameCameraがプレイヤー位置まで近づいたら
            RCAdjuster.EndToVirtual();
            isReal = false;

        }
        else
        {

            transGameParticle.Play();
            playerMove.moveType = PlayerMove.MoveType._2D;
            GCAdjuster.VirtualToReal();
            yield return new WaitForSeconds(1.0f);
            RCAdjuster.StartToReal();
            yield return new WaitForSeconds(1.0f);
            transGameParticle.Stop();
            RCAdjuster.EndToReal();
            isReal = true;
        }
        isTrans = false;
    }
}
