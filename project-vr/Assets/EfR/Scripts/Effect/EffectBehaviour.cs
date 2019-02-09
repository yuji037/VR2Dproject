using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBehaviour : MonoBehaviour {

	//// エフェクトの子にある、位置を追いかけるオブジェクト
	//[SerializeField]
	//GameObject attachObject1;
	//[SerializeField]
	//GameObject attachObject2;

	//// エフェクトの外にある、位置を追いかける対象
	//public GameObject attachTarget1;
	//public GameObject attachTarget2;

	//private void Update()
	//{

	//}

	public virtual void SetAttachTarget(int num, string targetName)
	{
		//switch ( num )
		//{
		//	case 1:
		//		attachTarget1 = GameObject.Find(targetName);
		//		break;

		//	case 2:
		//		attachTarget2 = GameObject.Find(targetName);
		//		break;
		//}
	}
}
