using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WireAction : MonoBehaviour
{
    [SerializeField]
    float wireAct;

    private GameObject player = null;
    private Rigidbody playersRigidbody = null;
    private Vector3 endWire;
    private float wireLength = 0.0f;
    bool isHook = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) { Debug.LogError("Don't Found Player"); }

        playersRigidbody = player.GetComponent<Rigidbody>();
    }
}
