using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtentions {

	/// <summary>
	/// 自オブジェクトの子を名前で検索する
	/// </summary>
	public static GameObject FindFirstChildByName(this GameObject go, string name)
	{
		var trs = go.GetComponentsInChildren<Transform>();
		// trs[0]は親なので無視
		for ( int i = 1; i < trs.Length; ++i )
		{
			if ( trs[i].name == name )
			{
				return trs[i].gameObject;
			}
		}
		return null;
	}

	/// <summary>
	/// 子階層にあるオブジェクト名を番号順に並べ、配列で返す。
	/// 番号は0～(要素数-1)。（例："CameraPos0", "CameraPos1"...）
	/// 子に他のオブジェクトが入ってはいけない
	/// </summary>
	/// <param name="prefixName">"pos1"の"pos"など、番号の前に付ける名前</param>
	public static Transform[] GetTransformsOrderByNumber(this GameObject go, string prefixName)
	{
		var trs = go.GetComponentsInChildren<Transform>();
		var ret = new Transform[trs.Length - 1];
		// 整理番号を格納する配列
		var nums = new int[trs.Length];
		// trs[0], nums[0]は自オブジェクトなので無視
		for (int i = 1; i < trs.Length; ++i )
		{
			var strNum = trs[i].name.Substring(prefixName.Length);

			if ( int.TryParse(strNum, out nums[i]) == false){
				Debug.LogError("番号取得失敗 : " + trs[i].name);
				nums[i] = -1;
			}
			else
			{
				// 取得成功
				Debug.Log(trs[i].name);
				ret[nums[i]] = trs[i];
			}
		}

		return ret;
	}
}
