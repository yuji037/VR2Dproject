using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFrameRate : MonoBehaviour {

	int m_iFrameCounter = 0;
	float m_fTimer = 0f;

	private void Start()
	{
		DebugTools.RegisterDebugAction(KeyCode.K, () =>
		{
			float nowFrameRate = Application.targetFrameRate;
			if ( nowFrameRate == 15 )
				Application.targetFrameRate = -1;
			else
				Application.targetFrameRate = 15;
		}, "固定フレームレート変更 無制限/15FPS");
	}

	// Update is called once per frame
	void Update () {
		m_fTimer += Time.unscaledDeltaTime;
		m_iFrameCounter++;

		if (m_fTimer > 1f )
		{
			DebugTools.DisplayText("fps", m_iFrameCounter + "fps", 100);
			if(m_iFrameCounter < 60 )
			{
				DebugTools.Log("fps低下：" + m_iFrameCounter + "fps");
			}
			m_fTimer = 0f;
			m_iFrameCounter = 0;
		}

	}
}
