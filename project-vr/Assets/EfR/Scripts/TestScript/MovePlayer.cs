using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {
    NetworkView networkview;

    float inputHorizontal, inputVertical;
    Rigidbody rigidbody;

    [SerializeField]
    private float jumpPower = 1.0f;

    float moveSpeed = 3.0f;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        networkview = GetComponent<NetworkView>();
    }
	
	// Update is called once per frame
	void Update () {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        if (networkview.isMine)
        {

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));

            Vector3 moveForward = cameraForward * inputVertical + Camera.main.transform.right * inputHorizontal;

            rigidbody.velocity = moveForward * moveSpeed + new Vector3(0, rigidbody.velocity.y, 0);

            if (moveForward != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveForward);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigidbody.velocity += new Vector3(0, jumpPower, 0);
            }
            rigidbody.velocity += new Vector3(0, Physics.gravity.y * Time.deltaTime, 0);
        }
    }
}
