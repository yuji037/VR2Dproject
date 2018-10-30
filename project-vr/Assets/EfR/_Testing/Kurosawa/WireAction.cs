using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ANIMATION : int
{
    None,
    Throw,
    Hookking,
    Reverse,
    Pulling
}

public class WireAction : MonoBehaviour
{
    [SerializeField]
    float wireAct;
    [SerializeField]
    GameObject player;
    private Rigidbody playersRigidbody = null;
    private Vector3 endWire;
    bool isHook = false;
    public LineRendererOperation lineOperation;

    // Use this for initialization
    void Start()
    {
        if (player == null) { Debug.LogError("Don't Set Player"); }
        lineOperation = GetComponent<LineRendererOperation>();
        if (lineOperation == null) { Debug.LogError("Don't Set lineOperation"); }
        playersRigidbody = player.GetComponent<Rigidbody>();
    }

    public void IsTrrigerOn(Vector3 _start, Vector3 _end)
    {
        lineOperation.LineSet(_start, _end);
    }


}
