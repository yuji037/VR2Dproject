using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GimmickReceiveDamagePlayer : GimmickReceiveDamage {

    public bool m_IsFPS;

    [SerializeField]
    Image m_imDamageEffectPanelInFPS;

    [SerializeField]
    Color m_colorDamageEffectInFPS;

    [SerializeField]
    float m_fFadeDurationInFPS = 1f;

    Coroutine m_DamageCoroutineFPS = null;

    public override void ReceiveDamage()
    {
        if ( m_IsFPS )
        {
            if ( m_DamageCoroutineFPS != null )
            {
                StopCoroutine(m_DamageCoroutineFPS);
                m_DamageCoroutineFPS = null;
            }
            m_DamageCoroutineFPS = StartCoroutine(DamageColorFadeCoroutineFPS());
        }
    }

    IEnumerator DamageColorFadeCoroutineFPS()
    {
        for(float t = 0; t < m_fFadeDurationInFPS; t += Time.deltaTime )
        {
            m_imDamageEffectPanelInFPS.color =
                    Color.clear * t
                 +  m_colorDamageEffectInFPS * ( m_fFadeDurationInFPS - t ) / m_fFadeDurationInFPS;
            yield return null;
        }
        m_imDamageEffectPanelInFPS.color = Color.clear;
        m_DamageCoroutineFPS = null;
    }
}
