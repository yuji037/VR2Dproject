using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
public class GimmickSave : GimmickBase
{
    [SerializeField]
    Transform respawnPoint;

	[SerializeField]
	GameObject savePointDisplayObj;
	[SerializeField]
	GameObject okDisplayObj;

	private void Start()
    {
        isCallOnlyServer = false;
        m_acTriggerEnterAction += Save;
    }

	void Save(Collider collider)
    {
        if (PlayerManager.LocalPlayer == collider.gameObject)
        {
            PlayerRespawner.GetInstance().SaveLocalPlayerRespawnPoint((respawnPoint)?respawnPoint.position:transform.position);
            if (savePointDisplayObj && okDisplayObj)
            {
                savePointDisplayObj.SetActive(false);
                okDisplayObj.SetActive(true);
            }
		}
    }
}
