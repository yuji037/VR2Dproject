using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class PlayerMove : NetworkBehaviour {

    PlayerStatus playerStatus;
    CharacterController characterController;

    public enum MoveType
    {
        FPS,
        TPS,
        _2D,
    }
    public MoveType moveType { get; private set; }

    [SerializeField]
    PlayerMoveSettings[] pmsDatasInDirectory;

    [SerializeField]
    PlayerMoveSettings Pms;

    float inputHorizontal, inputVertical;
    Vector3 velocity = Vector3.zero;
    [SerializeField]
    bool isGrounded = true;

    Transform camTransform;

    public override void OnStartLocalPlayer()
    {
        playerStatus = GetComponent<PlayerStatus>();
        characterController = GetComponent<CharacterController>();

        StageInit();
    }

    public void StageInit()
    {
        if ( isLocalPlayer )
        {
            var stageSettingObj = GameObject.Find("StageSettings");
            if ( stageSettingObj )
            {
                var setting = stageSettingObj.GetComponent<StageSettings>();

                moveType = setting.playerMoveTypeOnStart[playerStatus.playerControllerId - 1];
            }

        }
        Debug.Log("Load PlayerMoveSettings");
        LoadSettings();
        playerStatus.RendererSwitchForPlayerMoveType(moveType);

        camTransform = VRObjectManager.GetInstance().GetBaseCameraObject().transform;
    }

    public void LoadSettings()
    {
        Pms = pmsDatasInDirectory[(int)moveType];

        if (!Pms)
        {
            Debug.LogError("PlayerMoveSettings load failure");
        }
    }

    public void SwitchMoveType(MoveType _moveType)
    {
        moveType = _moveType;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // 2DならZ方向移動なくす
        if ( moveType == MoveType._2D ) inputVertical = 0f;

        // 着地判定
        //RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down/*, out hit*/, Pms.distanceToGround);

        // ジャンプ
        if (Pms.canJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Pms.jumpPower;
        }

        if ( Input.GetKeyDown(KeyCode.H) )
        {
            var oldType = moveType;
            moveType++; if ( moveType > MoveType._2D ) moveType = MoveType.FPS;
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer ) return;

        Vector3 cameraForwardXZ = Vector3.Scale(camTransform.forward, new Vector3(1, 0, 1));
        Vector3 moveForwardXZ = cameraForwardXZ * inputVertical + camTransform.right * inputHorizontal;

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
            velocity.y -= Pms.gravity * Time.deltaTime;
        // 空気抵抗（次第に減速するためのもの）
        velocity.y -= velocity.y * Pms.airVerticalResistance * Time.deltaTime;
        // 摩擦（次第に減速するためのもの）
        if (isGrounded)
            velocity -= velocity * Pms.groundFriction * Time.deltaTime;
        else
            velocity -= velocity * Pms.airResistance * Time.deltaTime;


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
