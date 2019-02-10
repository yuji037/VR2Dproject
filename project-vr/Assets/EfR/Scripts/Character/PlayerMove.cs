﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerMove : NetworkBehaviour
{

    #region Inner Enum
    public enum MoveType
    {
        FPS,
        TPS,
        _2D,
        FIXED,
    }
    #endregion


    #region Public Variables

    // 値をネットワーク同期させたいメンバ変数には[SyncVar]を付ける。
    // 
    //[SyncVar]
    MoveType _moveType;
    public MoveType moveType
    {
        get
        {
            return _moveType;
        }
    }

    public bool canMove = true;

    public bool debugInfiniteJump = false;

    //Stageが変わるとStageInit()が呼び出されるまでfalse
    public bool isReady = false;
    #endregion


    #region Private Variables

    [SerializeField]
    PlayerMoveSettings[] pmsDatasInDirectory;

    [SerializeField]
    PlayerMoveSettings Pms;

    [SerializeField]
    LayerMask fieldLayerMask;

    // 内部変数
    PlayerStatus playerStatus;
    CharacterController characterController;
    Animator animator;
    Transform camVRTransform;
    Transform cam2DTransform;
    float inputHorizontal, inputVertical;
    Vector3 velocity = Vector3.zero;
    GameObject moveFloorObject = null;
    Vector3 moveFloorPrevPos = Vector3.zero;

    [SerializeField]
    bool isGrounded = true;

    [SerializeField]
    bool isJumping = false;
    [SerializeField]
    float jumpingTime = 0f;

    #endregion


    #region Public Methods

    public Vector3 GetVelocity() { return velocity; }
    public bool IsGrounded() { return isGrounded; }
	public bool IsInputDescentFloor()
	{
		// ローカルプレイヤーなら床の当たり判定なくさないと他クライアントから変に見える
		//if ( !isLocalPlayer ) return true;
		return isGrounded && inputVertical < -0.4f;
	}

    public void StageInit()
    {
        if (isLocalPlayer)
        {
            var moveTypeOnStart = _moveType;

            var stageSettingObj = GameObject.Find("StageSettings");
            if (stageSettingObj)
            {
                var setting = stageSettingObj.GetComponent<StageSettings>();

                var playerNumber = playerStatus.playerControllerId;

                moveTypeOnStart = setting.playerMoveTypeOnStart[playerNumber];
                SwitchMoveType(moveTypeOnStart);

                playerStatus.CmdSetActive(setting.playerActiveOnStart[playerNumber]);
                DebugTools.Log("プレイヤアク"+playerNumber+ setting.playerActiveOnStart[playerNumber]);
               

                Debug.Log("change" + moveTypeOnStart);
            }
            else
            {
                Debug.LogWarning("StageSettingsオブジェクトがないため、Playerの視点モードが上手く設定できません。");
            }


            //playerStatus.RendererSwitchForPlayerMoveType(moveTypeOnStart);
            camVRTransform = VRObjectManager.GetInstance().GetBaseCameraObject().transform;
            cam2DTransform = GameObject.Find("Camera2D").transform;
            isReady = true;
     
        }
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero;
    }

    public void RendererSwitchForPlayerMoveType(MoveType moveType)
    {
        playerStatus.RendererSwitchForPlayerMoveType(moveType);
    }
    public void SwitchMoveType(MoveType __moveType)
    {
        _moveType = __moveType;
        CmdSetMoveType(__moveType);
        LoadMoveSettings(__moveType);
    }

    [ClientRpc]
    public void RpcRespawn()
    {
        if (!isLocalPlayer) return;
        PlayerRespawner.GetInstance().RespawnLocalPlayer();
    }

    public void Jump(float jumpPower)
    {
        var powerRate = 1f;
        if (InputKeepJump())
        {
            powerRate = 2.5f;
            animator.SetBool("Jump", true);
        }

        velocity.y = jumpPower * powerRate;
    }
    #endregion


    #region Private Methods & コールバック

    public override void OnStartLocalPlayer()
    {
        playerStatus = GetComponent<PlayerStatus>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        //StageInit();

        DebugTools.RegisterDebugAction(KeyCode.J, () => debugInfiniteJump = !debugInfiniteJump, "無限ジャンプON/OFF");
    }

    void LoadMoveSettings(MoveType __moveType)
    {
        Pms = pmsDatasInDirectory[(int)__moveType];

        if (!Pms)
        {
            Debug.LogError("PlayerMoveSettings load failure");
        }
    }
    [Command]
    void CmdSetMoveType(MoveType _moveType)
    {
        RpcSwitchMoveType(_moveType);
    }
    [ClientRpc]
    void RpcSwitchMoveType(MoveType _moveType)
    {
        this._moveType = _moveType;

        if (isLocalPlayer)
        {
            //MaterialsManager.GetInstance().Change();
            if (_moveType == PlayerMove.MoveType._2D)
                StageSwitchRenderer.GetInstance().SwitchRendererFor2DMode();
            else
                StageSwitchRenderer.GetInstance().SwitchRendererForVRMode();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer||!playerStatus.Active) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // 2DならZ方向入力なくす
        //if (moveType == MoveType._2D) inputVertical = 0f;

        // イベント中など、操作できない状態
        if (!canMove)
        {
            inputHorizontal = 0f;
            inputVertical = 0f;
            return;
        }

        CheckGround();

        UpdateJump();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer || !isReady) return;

		if ( !canMove )
		{
			velocity = Vector3.zero;
			return;
		}

        // カメラの前方向のXZ成分（ワールド座標）を計算
        Vector3 cameraForwardXZ = Vector3.Scale(camVRTransform.forward, new Vector3(1, 0, 1));
        if (cameraForwardXZ == Vector3.zero)
            return;

        cameraForwardXZ = Vector3.Normalize(cameraForwardXZ);

        // 実際に動くベクトル（ワールド座標）を計算
        Vector3 moveForwardXZ = Vector3.zero;
        switch (moveType)
        {
            case MoveType.FPS:
                moveForwardXZ = cameraForwardXZ * inputVertical + camVRTransform.right * inputHorizontal;
                break;

            case MoveType.TPS:
                moveForwardXZ = cameraForwardXZ * inputVertical + camVRTransform.right * inputHorizontal;
                break;

            case MoveType._2D:
                if (cam2DTransform) moveForwardXZ = cam2DTransform.right * inputHorizontal;
                break;

            case MoveType.FIXED:
                moveForwardXZ = cameraForwardXZ * inputVertical + camVRTransform.right * inputHorizontal;
                break;
        }

        // XZ平面移動
        if (isGrounded)
            velocity += moveForwardXZ * Pms.groundMoveAccel * Time.deltaTime;
        else
            velocity += moveForwardXZ * Pms.airMoveAccel * Time.deltaTime;


        var velocityXZ = new Vector3(velocity.x, 0, velocity.z);
        // プレイヤーの向き
        switch (moveType)
        {
            case MoveType.FPS:
                transform.rotation = Quaternion.LookRotation(cameraForwardXZ);
                break;

            case MoveType.TPS:
            case MoveType._2D:
            case MoveType.FIXED:
                if (moveForwardXZ != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(velocityXZ);
                }
                break;

                //if ( moveForwardXZ != Vector3.zero )
                //{
                //	transform.rotation = Quaternion.LookRotation(moveForwardXZ);
                //}
                //break;

                //            transform.rotation = Quaternion.LookRotation(cameraForwardXZ);
                //            break;
        }

        animator.SetFloat("Speed", velocityXZ.magnitude);

        // 重力（空中時に受ける）
        if (!isGrounded)
            velocity.y -= Pms.gravity * Time.deltaTime;
        // 空気抵抗（次第に減速するためのもの）
        //velocity.y		-= velocity.y * Pms.airVerticalResistance * Time.deltaTime;
        // 速度抵抗（次第に減速するためのもの）
        if (isGrounded)
            velocity -= velocity * Pms.groundRegistance * Time.deltaTime;
        else
            velocity -= velocity * Pms.airResistance * Time.deltaTime;


        // 速度上限
        if (velocity.sqrMagnitude > Pms.limitSpeed * Pms.limitSpeed)
        {
            velocity = velocity.normalized * Pms.limitSpeed;
            Debug.Log("速度ブレーキ");
        }

        // velocityに従って移動
        var deltaMove = velocity * Time.deltaTime;

        // 動く床に乗っていればそれの移動量を加算
        if (moveFloorObject)
        {
            var moveFloorNowPos = moveFloorObject.transform.position;
            var moveFloorDeltaPos = moveFloorNowPos - moveFloorPrevPos;
            deltaMove += LimitSpeedMoveFloor(moveFloorDeltaPos);
            moveFloorPrevPos = moveFloorObject.transform.position;
        }

        // キャラ移動
        characterController.Move(deltaMove);
    }

    Vector3 LimitSpeedMoveFloor(Vector3 moveFloorDeltaPos)
    {
        float limitSpeed = 500f;
        float limitDeltaLen = limitSpeed * Time.deltaTime;
        if (moveFloorDeltaPos.sqrMagnitude > limitDeltaLen * limitDeltaLen)
        {
            moveFloorDeltaPos = moveFloorDeltaPos.normalized * limitDeltaLen;
            DebugTools.Log("速度制限：動く床");
        }
        return moveFloorDeltaPos;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + new Vector3(0, Pms.distanceToGround, 0), transform.position + Vector3.down * Pms.distanceToGround);
    }
    void CheckGround()
    {
        // 着地判定
        RaycastHit hit;
        var prevIsGrounded = isGrounded;
        //床に埋まっている場合、足元から出してもレイが当たらないので、原点を足元から少し浮かす
        isGrounded = Physics.Raycast(
			transform.position+new Vector3(0,Pms.distanceToGround,0), 
			Vector3.down, 
			out hit, 
			Pms.distanceToGround*2,
			fieldLayerMask);
        if (isGrounded && !prevIsGrounded)
        {
            // 着地した瞬間
            animator.SetBool("Jump", false);
        }

        CheckMoveFloor(hit);
    }

    void CheckMoveFloor(RaycastHit hit)
    {

        if (!moveFloorObject && isGrounded && hit.collider.gameObject.tag == "LaserPointerFloorCreate")
        {
            //transform.parent = hit.collider.gameObject.transform;
            moveFloorObject = hit.collider.gameObject;
            moveFloorPrevPos = moveFloorObject.transform.position;
            Debug.Log("床に乗った");
        }

        if (moveFloorObject && !isGrounded)
        {
            //transform.parent = null;
            moveFloorObject = null;

            Debug.Log("床から離れた");
        }
    }

    bool InputTriggerJump()
    {
        return Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Two);
    }

    bool InputKeepJump()
    {
        return Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.Two);
    }

    // ジャンプ処理
    void UpdateJump()
    {
        // ジャンプ開始
        if ((Pms.canJump && isGrounded && !isJumping) || debugInfiniteJump)
        {
            if (InputTriggerJump())
            {
                isJumping = true;
                animator.SetBool("Jump", true);
            }
        }
        // ジャンプ中
        if (isJumping)
        {
            jumpingTime += Time.deltaTime;

            velocity.y = Pms.jumpPower * ((Pms.jumpingDuration - jumpingTime) / Pms.jumpingDuration);
        }
        // 長押しジャンプ停止
        if ((!InputTriggerJump() && !InputKeepJump()) || jumpingTime >= Pms.jumpingDuration)
        {
            //Debug.Log("長押しジャンプ停止");
            isJumping = false;
            jumpingTime = 0f;
        }
        //// ジャンプ中に天井にぶつかったら
        //RaycastHit hit;
        //bool hitRoof = Physics.Raycast( transform.position, Vector3.up, out hit, Pms.distanceToGround );
        //if ( hitRoof )
        //{
        //	Debug.Log("天井ヒット");
        //	isJumping = false;
        //}
    }

    #endregion


}
