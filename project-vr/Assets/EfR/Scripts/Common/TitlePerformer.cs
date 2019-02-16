using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TitlePerformer :  NetworkBehaviour{
    [SerializeField]
    Image pressAnyButtonImage;

    [SerializeField]
    GameObject titlePanel;

    [SerializeField]
    SelectStageMenu selectStageMenu;

    [SerializeField]
    float pressAnyButtonActiveinterval;

    bool pressedAnyButton = false;

    private void Start()
    {
        StartCoroutine(PerformerCoroutine());
        StartCoroutine(LoopingActive());
    }
    private void Update()
    {
        if (!isServer||!TVSwitch.IsOn) return;
        if (Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Any))
        {
            RpcPressed();
            selectStageMenu.isReady = true;
        }
    }
    [ClientRpc]
    void RpcPressed()
    {
        pressedAnyButton= true;
    }

    IEnumerator PerformerCoroutine()
    {
        yield return new WaitUntil(()=>pressedAnyButton);
        titlePanel.SetActive(false);
    }

    IEnumerator LoopingActive()
    {
        while (true)
        {
            pressAnyButtonImage.enabled = !pressAnyButtonImage.enabled;
            yield return new WaitForSeconds(pressAnyButtonActiveinterval);
        }
    }

}
