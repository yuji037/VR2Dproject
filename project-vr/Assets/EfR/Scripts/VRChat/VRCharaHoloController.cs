using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCharaHoloController : SingletonMonoBehaviour<VRCharaHoloController> {
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {

            StartCoroutine(VRCharaHoloController.GetInstance().P1VRChatCharaFadeOut());
        }
    }
    public IEnumerator P1VRChatCharaFadeOut()
    {
        Debug.Log("Holo Start");

        var renderer = GameObject.Find("VRChatCharaPos" + (2)).GetComponentInChildren<SkinnedMeshRenderer>();
        var wPosY = renderer.transform.position.y;
        var holoMat = renderer.material;
        var speed = 3.0f;
        holoMat.SetFloat("_App", 1);
        for (float t = 0; t <= 10.0f; t += Time.deltaTime * speed)
        {
            holoMat.SetFloat("_Pos", wPosY + t);
            yield return null;
        }
    }
}
