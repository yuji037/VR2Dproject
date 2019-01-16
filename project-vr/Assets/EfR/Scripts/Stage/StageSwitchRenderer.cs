using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSwitchRenderer : SingletonMonoBehaviour<StageSwitchRenderer> {
	
	public void SwitchRendererFor2DMode()
	{
		var playerStatus = PlayerManager.LocalPlayer.GetComponent<PlayerStatus>();

		switch ( playerStatus.Number )
		{
			case PlayerNumber.Player1:
				SwitchRenderer(1, true);
				SwitchRenderer(2, false);
				break;
			case PlayerNumber.Player2:
				SwitchRenderer(1, false);
				SwitchRenderer(2, true);
				break;
		}
	}
	public void SwitchRendererForVRMode()
	{
		SwitchRenderer(1, true);
		SwitchRenderer(2, true);
	}

	public void SwitchRenderer(int playerNum, bool enabled)
	{
		var renderers = GameObject.Find("P" + playerNum + "Objects").GetComponentsInChildren<Renderer>();

		foreach(var ren in renderers )
		{
			ren.enabled = enabled;
		}
	}
}
