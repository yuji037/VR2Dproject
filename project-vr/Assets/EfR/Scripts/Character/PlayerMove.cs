using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "PlayerMoveSettings", menuName = "Create PlayerMoveSettings", order = 0)]
public class PlayerMoveSettings : ScriptableObject {

    public bool canJump;
    public float jumpPower;
    public float limitSpeed;
    public float groundMoveAccel;
    public float airMoveAccel;
    public float gravity;
    public float airVerticalResistance;
    public float airResistance;
    public float groundFriction;
    public float distanceToGround;
}


public class PlayerMove : MonoBehaviour {

    public enum MoveType {
        FPS,
        TPS,
        _2D,
    }
    [SerializeField]
    MoveType moveType;

    NetworkView networkview;
    [SerializeField]
    bool useNetwork;

    float inputHorizontal, inputVertical;
    CharacterController characterController;

    Vector3 velocity = Vector3.zero;

    public PlayerMoveSettings Pms { get; private set; }

    [SerializeField]
    bool isGrounded = true;

    // Use this for initialization
    void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        if ( useNetwork ) networkview = GetComponent<NetworkView>();
        characterController = GetComponent<CharacterController>();

        LoadSettings();
    }

    public void LoadSettings()
    {
        switch ( moveType )
        {
            case MoveType.FPS:
                Pms = Resources.Load<PlayerMoveSettings>("Chara/PlayerMoveSettingsFPS");
                break;
            case MoveType.TPS:
                Pms = Resources.Load<PlayerMoveSettings>("Chara/PlayerMoveSettingsTPS");
                break;
            case MoveType._2D:
                Pms = Resources.Load<PlayerMoveSettings>("Chara/PlayerMoveSettings2D");
                break;
        }
        if ( !Pms )
        {
            Debug.LogError("PlayerMoveSettings load failure");
        }
    }

    // Update is called once per frame
    void Update()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");

        RaycastHit hit;
        if ( Physics.Raycast(transform.position, Vector3.down, out hit, Pms.distanceToGround) )
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        // ジャンプ
        if ( Pms.canJump && isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Pms.jumpPower;
        }
    }

    void FixedUpdate()
    {

        if ( !useNetwork || networkview.isMine )
        {

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));

            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

            if ( isGrounded ) velocity += moveForward * Pms.groundMoveAccel * Time.deltaTime;
            else velocity += moveForward * Pms.airMoveAccel * Time.deltaTime;


            if ( moveForward != Vector3.zero )
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }
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
            characterController.Move(velocity * Time.deltaTime);
        }
    }
}
