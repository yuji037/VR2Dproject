using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChara : MonoBehaviour {

    Vector3 forceVec;

    float inputHorizontal, inputVertical;
    [SerializeField] float speed;
    Rigidbody rigidbody;

    [SerializeField]
    Vector3 initpos;

    [SerializeField]
    GameObject gimickobj;

    // Use this for initialization
    void Start () {
        forceVec = new Vector3(0, 0, 0);
        rigidbody = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {



        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        
        forceVec = new Vector3(inputHorizontal, inputVertical, 0) * Time.deltaTime * speed;
        forceVec = Camera.main.transform.rotation * forceVec;
        transform.position += forceVec * Time.deltaTime * speed;
        //rigidbody.AddForce(forceVec, ForceMode.VelocityChange);

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Camera.main.transform.position = initpos;
        }

    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name== "GimickWall")
        {

            Camera.main.transform.position = initpos;

        }

    }

}
