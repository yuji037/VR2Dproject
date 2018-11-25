using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerMove : NetworkBehaviour {

    PlayerStatus			playerStatus;
    CharacterController		characterController;

    public enum MoveType
    {
        FPS,
        TPS,
        _2D,
    }

	[SyncVar]
	MoveType		_moveType; 
    public MoveType moveType { get
		{
			return	_moveType;
		} }

    [SerializeField]
    PlayerMoveSettings[]	pmsDatasInDirectory;

    [SerializeField]
    PlayerMoveSettings		Pms;

	// 内部変数
    float inputHorizontal, inputVertical;
    Vector3 velocity = Vector3.zero;
    [SerializeField]
    bool isGrounded = true;

    Transform camTransform;

    public bool canMove=true;
    public override void OnStartLocalPlayer()
    {
        playerStatus = GetComponent<PlayerStatus>();
        characterController = GetComponent<CharacterController>();

        StageInit();
    }
    
    public Vector3 GetVelocity() { return velocity; }

    public void StageInit()
    {
        if ( isLocalPlayer )
        {
			var moveTypeOnStart = _moveType;

			var stageSettingObj = GameObject.Find("StageSettings");
            if ( stageSettingObj )
            {
                var setting = stageSettingObj.GetComponent<StageSettings>();

				moveTypeOnStart = setting.playerMoveTypeOnStart[playerStatus.playerControllerId - 1];
				SwitchMoveType(moveTypeOnStart);
            }
			else
			{
				Debug.LogWarning("StageSettingsオブジェクトがないため、Playerの視点モードが上手く設定できません。");
			}

			playerStatus.RendererSwitchForPlayerMoveType(moveTypeOnStart);
			camTransform = VRObjectManager.GetInstance().GetBaseCameraObject().transform;
		}
    }

    public void LoadMoveSettings(MoveType __moveType)
    {
        Pms = pmsDatasInDirectory[(int)__moveType];

        if (!Pms)
        {
            Debug.LogError("PlayerMoveSettings load failure");
        }
    }
    public void RendererSwitchForPlayerMoveType(MoveType moveType)
    {
        playerStatus.RendererSwitchForPlayerMoveType(moveType);
    }
    public void SwitchMoveType(MoveType __moveType)
    {
		CmdSetMoveType(__moveType);
		LoadMoveSettings(__moveType);
    }
	[Command]
	void CmdSetMoveType(MoveType _moveType)
	{
		this._moveType = _moveType;
	}

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // 2DならZ方向移動なくす
        if ( moveType == MoveType._2D ) inputVertical = 0f;
        if (!canMove)
        {
            inputHorizontal = 0f;
            inputVertical=0f;
        }

        // 着地判定
        //RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down/*, out hit*/, Pms.distanceToGround);

        // ジャンプ
        if (Pms.canJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Pms.jumpPower;
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer ) return;

		// カメラの前方向と右方向を基準に移動
        Vector3 cameraForwardXZ = Vector3.Scale(camTransform.forward, new Vector3(1, 0, 1));
		if ( cameraForwardXZ == Vector3.zero ) return;

		cameraForwardXZ			= Vector3.Normalize(cameraForwardXZ);
		Vector3 moveForwardXZ	= Vector3.zero;

		switch ( moveType )
		{
			case MoveType.FPS:
				moveForwardXZ = cameraForwardXZ * inputVertical + camTransform.right * inputHorizontal;
				break;

			case MoveType.TPS:
				moveForwardXZ = cameraForwardXZ * inputVertical + camTransform.right * inputHorizontal;
				break;

			case MoveType._2D:
				moveForwardXZ = new Vector3(inputHorizontal, 0, 0);
				break;
		}

        // XZ平面移動
        if (isGrounded)
            velocity += moveForwardXZ * Pms.groundMoveAccel * Time.deltaTime;
        else
            velocity += moveForwardXZ * Pms.airMoveAccel * Time.deltaTime;

        // プレイヤーに移動方向を向かせる（瞬時）
        if (moveForwardXZ != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveForwardXZ);
        }

        // 重力
        if (!isGrounded)
            velocity.y	-= Pms.gravity * Time.deltaTime;
        // 空気抵抗（次第に減速するためのもの）
        velocity.y		-= velocity.y * Pms.airVerticalResistance * Time.deltaTime;
        // 摩擦（次第に減速するためのもの）
        if (isGrounded)
            velocity	-= velocity * Pms.groundFriction * Time.deltaTime;
        else
            velocity	-= velocity * Pms.airResistance * Time.deltaTime;


        // 速度上限
        if (velocity.sqrMagnitude > Pms.limitSpeed * Pms.limitSpeed)
        {
            velocity = velocity.normalized * Pms.limitSpeed;
            Debug.Log("速度ブレーキ");
        }

        // velocityに従って移動
        characterController.Move(velocity * Time.deltaTime);
    }
}
