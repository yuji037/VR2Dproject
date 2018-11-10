using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;




public class PlayerMove : MonoBehaviour {
    
    public enum MoveType {
        FPS,
        TPS,
        _2D,
    }
    [SerializeField]
    MoveType moveType;

    [SerializeField]
    bool useNetwork;

    [SerializeField]
    PlayerMoveSettings[] pmsDatasInDirectory;


    [SerializeField]
    PlayerMoveSettings Pms;

    [SerializeField]
    bool isGrounded = true;
    
    [SerializeField]
    // 挙動計算をこのクライアントで行うか？
    bool isAssignedLocal = false;

    NetworkView networkview;
    CharacterController characterController;

    float inputHorizontal, inputVertical;
    Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        if ( useNetwork ) networkview = GetComponent<NetworkView>();
        characterController = GetComponent<CharacterController>();

        LoadSettings();
        isAssignedLocal = ( !useNetwork || networkview.isMine );
    }

    public void LoadSettings()
    {
        // Resources.Load()が良く失敗してnullを返すので直接Inspectorから代入
        //switch ( moveType )
        //{
        //    case MoveType.FPS:
        //        Pms = Resources.Load<PlayerMoveSettings>("Chara/PlayerMoveSettingsFPS");
        //        break;
        //    case MoveType.TPS:
        //        Pms = Resources.Load<PlayerMoveSettings>("Chara/PlayerMoveSettingsTPS");
        //        break;
        //    case MoveType._2D:
        //        Pms = Resources.Load<PlayerMoveSettings>("Chara/PlayerMoveSettings2D");
        //        break;
        //}
        Pms = pmsDatasInDirectory[(int)moveType];

        if ( !Pms )
        {
            Debug.LogError("PlayerMoveSettings load failure");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ( !isAssignedLocal ) return;

        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        // 着地判定
        //RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down/*, out hit*/, Pms.distanceToGround);

        // ジャンプ
        if ( Pms.canJump && isGrounded && Input.GetKeyDown(KeyCode.Space) )
        {
            velocity.y = Pms.jumpPower;
        }
    }

    void FixedUpdate()
    {
        if ( !isAssignedLocal ) return;

        Vector3 cameraForwardXZ = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));
        Vector3 moveForwardXZ = cameraForwardXZ * inputVertical + Camera.main.transform.right * inputHorizontal;

        // XZ平面移動
        if ( isGrounded )
            velocity += moveForwardXZ * Pms.groundMoveAccel   * Time.deltaTime;
        else
            velocity += moveForwardXZ * Pms.airMoveAccel      * Time.deltaTime;

        // プレイヤーに移動方向を向かせる（瞬時）
        if ( moveForwardXZ != Vector3.zero )
        {
            transform.rotation = Quaternion.LookRotation(moveForwardXZ);
        }

        // 重力
        if ( !isGrounded )
            velocity.y -= Pms.gravity * Time.deltaTime;
        // 空気抵抗（次第に減速するためのもの）
        velocity.y -= velocity.y * Pms.airVerticalResistance * Time.deltaTime;
        // 摩擦（次第に減速するためのもの）
        if ( isGrounded )
            velocity -= velocity * Pms.groundFriction * Time.deltaTime;
        else
            velocity -= velocity * Pms.airResistance * Time.deltaTime;


        // 速度上限
        if ( velocity.sqrMagnitude > Pms.limitSpeed * Pms.limitSpeed )
        {
            velocity = velocity.normalized * Pms.limitSpeed;
            Debug.Log("速度ブレーキ");
        }

        // velocityに従って移動
        characterController.Move(velocity * Time.deltaTime);
    }
}
