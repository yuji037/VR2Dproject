using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 固定砲台
public class GimmickTurret : GimmickBase {

    [SerializeField]
    GameObject m_oBulletPrefab;

    [SerializeField]
    float m_fShootInterval = 2f;

    [SerializeField]
    float m_fShootIntervalRandomize = 0f;

    [SerializeField]
    Vector3 m_vBirthPositionRandomizeOffset = Vector3.zero;

	// Use this for initialization
	void Start () {
        StartCoroutine(ShootCoroutine());
	}
	
    IEnumerator ShootCoroutine()
    {
        while ( true ) {
            yield return new WaitForSeconds(m_fShootInterval + Random.Range(-m_fShootIntervalRandomize, m_fShootIntervalRandomize));

            var bullet = Instantiate(m_oBulletPrefab);
            var posRandomize = transform.position + new Vector3(
                Random.Range(-m_vBirthPositionRandomizeOffset.x, m_vBirthPositionRandomizeOffset.x),
                Random.Range(-m_vBirthPositionRandomizeOffset.y, m_vBirthPositionRandomizeOffset.y),
                Random.Range(-m_vBirthPositionRandomizeOffset.z, m_vBirthPositionRandomizeOffset.z));

            bullet.transform.position = posRandomize;
            yield return null;
        }
    }


}
