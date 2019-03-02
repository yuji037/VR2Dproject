using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClearSelectMenu : NetworkBehaviour{
    public static ClearSelectMenu instance;


    [SerializeField]
    GameObject selectUI;

    [SerializeField]
    Transform cursor;

    [SerializeField]
    Transform stageSelectPos;

    [SerializeField]
    Transform gameEndPos;

    bool isActive;

    bool isGameEndSelecting;

    private void Start()
    {
        instance = this;
    }

    [Command]
    public void CmdSetActive(bool isActive)
    {
        RpcSetActive(isActive);
    }

    [ClientRpc]
    void RpcSetActive(bool isActive)
    {
        selectUI.SetActive(isActive);
        this.isActive = isActive;
    }

    private void Update()
    {
        if (! (isServer&&isActive) ) return;
        var inputHorizontal = Input.GetAxisRaw("Horizontal");
        var inputeEnter = Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Two);
        var inputRight = (inputHorizontal > 0.3f);
        var inputLeft = (inputHorizontal < -0.3f);

        if (inputeEnter)
        {
            RpcSetActive(false);
            CmdGoToMenu();
        }
        else if (inputRight)
        {
            RpcSelectGameEnd();
        }
        else if (inputLeft)
        {
            RpcSelectGameSelect();
        }


    }
    [ClientRpc]
    void RpcSelectGameEnd()
    {
        cursor.transform.position = gameEndPos.position;
        isGameEndSelecting = true;
    }

    [ClientRpc]
    void RpcSelectGameSelect()
    {
        cursor.transform.position = stageSelectPos.position;
        isGameEndSelecting = false;
    }

    [Command]
    void CmdGoToMenu()
    {
        RpcGoToMenu();
    }

    [ClientRpc]
    void RpcGoToMenu()
    {
        TVSwitch.IsOn = !isGameEndSelecting;
        Debug.Log("GoToMenu");
        GameCoordinator.GetInstance().ChangeStageSelectMenu();
		SoundManager.GetInstance().StopBGM();
		if ( isGameEndSelecting )
			TutorialKakunin.GetInstance().SetActiveThisObj(true);
    }
}
