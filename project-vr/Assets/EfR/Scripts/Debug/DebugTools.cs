using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Unityのデバッグ機能で足りないものを追加。
/// </summary>
public class DebugTools : SingletonMonoBehaviour<DebugTools> {

	[SerializeField]
	DebugDisplay debugDisplay;

	/// <summary>
	/// 実機上で表示し続けたい情報を表示。
	/// 初めてみるtagNameなら行を追加し、
	/// 既存のtagNameならその行を更新する。
	/// </summary>
	/// <param name="tagName">識別名</param>
	/// <param name="message">表示したい文字列</param>
	/// <param name="displayPriority">表示優先度（高いほど上に表示）</param>
	public static void DisplayText(string tagName, string message, int displayPriority = 0)
	{
		GetInstance().debugDisplay.UpdateDisplayText(tagName, message, displayPriority);
	}
	public static void DisplayText(string tagName, object message, int displayPriority = 0)
	{
		GetInstance().debugDisplay.UpdateDisplayText(tagName, GetString(message), displayPriority);
	}

	/// <summary>
	/// 実機上のDebug.Log()。タイムラインのように流れていく。
	/// </summary>
	/// <param name="message"></param>
	public static void Log(string message)
	{
		GetInstance().debugDisplay.AddLog(message);
	}
	public static void Log(object message)
	{
		GetInstance().debugDisplay.AddLog(GetString(message));
	}

	static string GetString(object obj)
	{
		string str = null;
		if(obj is string )
		{
			str = obj as string;
		}
		else
		{
			str = obj.ToString();
		}
		return str;
	}
}

