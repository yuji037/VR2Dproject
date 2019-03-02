using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// Unityのデバッグ機能で足りないものを追加。
/// </summary>
public class DebugTools : SingletonMonoBehaviour<DebugTools> {

	#region Public Variables
	#endregion


	#region Private Variables

	[SerializeField]
	DebugDisplay debugDisplay;

	[SerializeField]
	bool isDebugMenuOn = false;

	[SerializeField]
	GameObject debugMenuCanvas;

	Dictionary<KeyCode, System.Action> debugActions = new Dictionary<KeyCode, Action>();
	Dictionary<KeyCode, string> debugActionDescriptions = new Dictionary<KeyCode, string>();

	#endregion


	#region Public Methods

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
	public static void UnregisterDisplayText(string tagName)
	{
		GetInstance().debugDisplay.UnregisterDisplayText(tagName);
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

	/// <summary>
	/// キー入力で発動したいデバッグ機能を追加する。
	/// </summary>
	/// <param name="keyCode"></param>
	/// <param name="action"></param>
	public static void RegisterDebugAction(KeyCode keyCode, System.Action action, string description)
	{
		GetInstance().debugActions				[keyCode] = action;
		GetInstance().debugActionDescriptions	[keyCode] = description;
	}

	#endregion


	#region Private Methods & Callback

	private void Start()
	{
		UpdateGuideText();
	}

	private void Update()
	{
		if ( Input.GetKeyDown(KeyCode.F3) )
		{
			isDebugMenuOn = !isDebugMenuOn;
			SwitchDebugMenuCanvas();
			UpdateGuideText();
		}

		if ( isDebugMenuOn )
		{
			foreach ( var debugAction in debugActions )
			{
				if ( Input.GetKeyDown(debugAction.Key) )
				{
					Log(debugAction.Key.ToString() + "を押下 : " + debugActionDescriptions[debugAction.Key]);
					debugAction.Value();
				}
			}
		}
	}

	void UpdateGuideText()
	{
		DisplayText("デバッグ案内", "デバッグメニューON/OFF：F3 　現在：" + (isDebugMenuOn?"ON":"OFF"), 10000);
		if ( isDebugMenuOn )
		{
			foreach(var pair in debugActionDescriptions )
			{
				DisplayText(			"Debug " + pair.Key,	pair.Key.ToString() + " : " + pair.Value, 9000);
			}
		}
		else
		{
			foreach ( var pair in debugActionDescriptions )
			{
				UnregisterDisplayText(	"Debug " + pair.Key);
			}
		}
	}

	void SwitchDebugMenuCanvas()
	{
		debugMenuCanvas.SetActive(isDebugMenuOn);
	}

	static string GetString(object obj)
	{
		string str = null;
		if ( obj is string )
		{
			str = obj as string;
		}
		else
		{
			str = obj.ToString();
		}
		return str;
	}

	#endregion


}

