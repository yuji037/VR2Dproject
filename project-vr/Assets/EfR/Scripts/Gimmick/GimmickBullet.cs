using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickBullet : GimmickBase {

    [SerializeField]
    public Vector3 m_vDir = new Vector3(-1, 0, 0);

    [SerializeField]
    public float m_fSpeed = 1f;

    [SerializeField]
    int[] m_iHitReceiverGimmickIDs;

    private void Start()
    {
        m_aTriggerEnterAction += HitAction;
    }

    private void Update()
    {
        transform.Translate(m_vDir * m_fSpeed * Time.deltaTime);
    }

    void HitAction(Collider other , int otherGimmickID)
    {
        foreach(var hitReceiverID in m_iHitReceiverGimmickIDs )
        {
            if(otherGimmickID == hitReceiverID )
            {
                var receiver = GimmickManager.GetGimmick(hitReceiverID) as GimmickReceiveDamage;

                receiver.ReceiveDamage();
                Destroy(gameObject);
                break;
            }
        }
    }
}
