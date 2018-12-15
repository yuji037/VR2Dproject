using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerMove : NetworkBehaviour {

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
	MoveType		_moveType; 
    public MoveType moveType { get
		{
			return	_moveType;
		} }

	public bool canMove = true;

	#endregion


	#region Private Variables

	[SerializeField]
    PlayerMoveSettings[]	pmsDatasInDirectory;

    [SerializeField]
    PlayerMoveSettings		Pms;

	// 内部変数
	PlayerStatus			playerStatus;
	CharacterController		characterController;
    Transform				camVRTransform;
    Transform				cam2DTransform;
	float					inputHorizontal, inputVertical;
	Vector3					velocity = Vector3.zero;
	GameObject				moveFloorObject = null;
	Vector3					moveFloorPrevPos = Vector3.zero;

	[SerializeField]
    bool					isGrounded = true;


	#endregion


	#region Public Methods

	public Vector3	GetVelocity() { return velocity; }

	public void		StageInit()
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
			camVRTransform = VRObjectManager.GetInstance().GetBaseCameraObject().transform;
			cam2DTransform = GameObject.Find("Camera2D").transform;
		}
	}

	public void		RendererSwitchForPlayerMoveType(MoveType moveType)
	{
		playerStatus.RendererSwitchForPlayerMoveType(moveType);
	}
	public void		SwitchMoveType(MoveType __moveType)
	{
		CmdSetMoveType(__moveType);
		LoadMoveSettings(__moveType);
	}
	#endregion


	#region Private Methods & コールバック

	public override void OnStartLocalPlayer()
	{
		playerStatus = GetComponent<PlayerStatus>();
		characterController = GetComponent<CharacterController>();

		StageInit();
	}

	void LoadMoveSettings(MoveType __moveType)
	{
		Pms = pmsDatasInDirectory[(int)__moveType];

		if ( !Pms )
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
		//MaterialsManager.GetInstance().Change();
	}

	// Update is called once per frame
	void Update()
    {
        if (!isLocalPlayer) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // 2DならZ方向移動なくす
        if (moveType == MoveType._2D) inputVertical = 0f;

        // イベント中など、操作できない状態
        if (!canMove)
        {
            inputHorizontal = 0f;
            inputVertical = 0f;
            return;
        }

        // 着地判定
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, Pms.distanceToGround);
        if (!moveFloorObject && isGrounded && hit.collider.gameObject.tag == "LaserPointerFloorCreate")
        {
            //transform.parent = hit.collider.gameObject.transform;
            moveFloorObject = hit.collider.gameObject;
            moveFloorPrevPos = moveFloorObject.transform.position;
			Debug.Log("床に乗った");
        }

        if ( moveFloorObject && !isGrounded)
        {
            //transform.parent = null;
            moveFloorObject = null;

			Debug.Log("床から離れた");

		}

		// ジャンプ
		if (Pms.canJump && isGrounded)
        {  
            if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Two))
            {
                velocity.y = Pms.jumpPower;
            }
		}
	}

	void FixedUpdate()
	{
		if ( !isLocalPlayer ) return;

		// カメラの前方向のXZ成分（ワールド座標）を計算
		Vector3 cameraForwardXZ =	Vector3.Scale(camVRTransform.forward, new Vector3(1, 0, 1));
		if (	cameraForwardXZ ==	Vector3.zero )
			return;

				cameraForwardXZ =	Vector3.Normalize(cameraForwardXZ);

		// 実際に動くベクトル（ワールド座標）を計算
		Vector3 moveForwardXZ = Vector3.zero;
		switch ( moveType )
		{
			case MoveType.FPS:
				moveForwardXZ = cameraForwardXZ * inputVertical + camVRTransform.right * inputHorizontal;
				break;

			case MoveType.TPS:
				moveForwardXZ = cameraForwardXZ * inputVertical + camVRTransform.right * inputHorizontal;
				break;

			case MoveType._2D:
				moveForwardXZ = cam2DTransform.right * inputHorizontal;
				break;

            case MoveType.FIXED:
                moveForwardXZ = cameraForwardXZ * inputVertical + camVRTransform.right * inputHorizontal;
                break;
		}

		// XZ平面移動
		if ( isGrounded )
			velocity += moveForwardXZ * Pms.groundMoveAccel * Time.deltaTime;
		else
			velocity += moveForwardXZ * Pms.airMoveAccel * Time.deltaTime;

		// プレイヤーの向き
		switch ( moveType )
		{
			case MoveType.FPS:
				transform.rotation = Quaternion.LookRotation(cameraForwardXZ);
				break;

			case MoveType.TPS:
				transform.rotation = Quaternion.LookRotation(cameraForwardXZ);
				break;

			case MoveType._2D:
				if ( moveForwardXZ != Vector3.zero )
				{
					transform.rotation = Quaternion.LookRotation(moveForwardXZ);
				}
				break;

            case MoveType.FIXED:
                transform.rotation = Quaternion.LookRotation(cameraForwardXZ);
                break;
		}

		// 重力（空中時に受ける）
		if ( !isGrounded )
			velocity.y	-= Pms.gravity * Time.deltaTime;
		// 空気抵抗（次第に減速するためのもの）
		velocity.y		-= velocity.y * Pms.airVerticalResistance * Time.deltaTime;
		// 摩擦（次第に減速するためのもの）
		if ( isGrounded )
			velocity	-= velocity * Pms.groundFriction * Time.deltaTime;
		else
			velocity	-= velocity * Pms.airResistance * Time.deltaTime;


		// 速度上限
		if ( velocity.sqrMagnitude > Pms.limitSpeed * Pms.limitSpeed )
		{
			velocity = velocity.normalized * Pms.limitSpeed;
			Debug.Log("速度ブレーキ");
		}

		// velocityに従って移動
		var deltaMove = velocity * Time.deltaTime;

		// 動く床に乗っていればそれの移動量を加算
		if ( moveFloorObject )
		{
			var moveFloorNowPos = moveFloorObject.transform.position;
			var moveFloorDeltaPos = moveFloorNowPos - moveFloorPrevPos;
			deltaMove += LimitSpeedMoveFloor(moveFloorDeltaPos);
			DebugTools.DisplayText("deltaMove", deltaMove);
			moveFloorPrevPos = moveFloorObject.transform.position;
		}

		// キャラ移動
		characterController.Move(deltaMove);
	}

	Vector3 LimitSpeedMoveFloor(Vector3 moveFloorDeltaPos)
	{
		float limitSpeed = 500f;
		float limitDeltaLen = limitSpeed * Time.deltaTime;
		if ( moveFloorDeltaPos.sqrMagnitude > limitDeltaLen * limitDeltaLen )
		{
			moveFloorDeltaPos = moveFloorDeltaPos.normalized * limitDeltaLen;
			DebugTools.Log("速度制限：動く床");
		}
		return moveFloorDeltaPos;
	}

	#endregion


}
