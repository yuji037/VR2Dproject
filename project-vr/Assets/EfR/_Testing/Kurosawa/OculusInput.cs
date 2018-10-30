using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OculusInput : MonoBehaviour {
    [SerializeField]
    GameObject player;

    private OVRRaycaster ray;
    private OVRCameraRig rig;
    private RaycastHit hit;
    private WireAction wireAction;
    private ANIMATION animationMode = ANIMATION.None;

    public void AnimationMode(ANIMATION _mode) { animationMode = _mode; }

    private void Start()
    {
        wireAction = player.GetComponentInChildren<WireAction>();
        if (wireAction == null) { Debug.LogError("wireAction is null"); }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (animationMode != ANIMATION.None) { return; }
            animationMode = ANIMATION.Throw;
            Ray ray = new Ray(player.transform.position, player.transform.forward);
            if (Physics.Raycast(ray, out hit))
            {
                wireAction.IsTrrigerOn(player.transform.position, hit.transform.position);
                Debug.Log(hit.transform.name);
            }
            else { animationMode = ANIMATION.None; }
        }

        if (animationMode == ANIMATION.Throw)
        {
            StartCoroutine(wireAction.lineOperation.LineOpen());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (animationMode != ANIMATION.Hookking) { return; }
            StartCoroutine(wireAction.lineOperation.LineClose());
        }

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Debug.Log("A");
        }

        if (OVRInput.Get(OVRInput.Button.Four))
        {
            Debug.Log("Y");
        }

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            Debug.Log("OVRInput.RawButton.LIndexTrigger");
        }

        if (OVRInput.Get(OVRInput.RawTouch.LIndexTrigger))
        {
          //  Debug.Log("OVRInput.RawTouch.LIndexTrigger");
        }

        if (OVRInput.Get(OVRInput.RawNearTouch.LIndexTrigger))
        {
            if (Physics.Raycast(rig.leftHandAnchor.transform.position, rig.leftHandAnchor.transform.position + rig.leftHandAnchor.transform.forward, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);

            }
        }
    }
}
