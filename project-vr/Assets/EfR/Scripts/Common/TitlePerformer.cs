using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitlePerformer : MonoBehaviour {
    [SerializeField]
    Image pressAnyButton;

    [SerializeField]
    GameObject titlePanel;

    [SerializeField]
    SelectStageMenu selectStageMenu;

    [SerializeField]
    float tenmetuTime;
    private void Start()
    {
        StartCoroutine(PerformerCoroutine());
        StartCoroutine(Tenmetu());
    }
    IEnumerator PerformerCoroutine()
    {
         yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Space) || OVRInput.GetDown(OVRInput.Button.Two));
        selectStageMenu.isReady = true;
        titlePanel.SetActive(false);
    }

    IEnumerator Tenmetu()
    {
        while (true)
        {
            pressAnyButton.enabled = !pressAnyButton.enabled;
            yield return new WaitForSeconds(tenmetuTime);
        }
    }

}
