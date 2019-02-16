using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 実機上でDebug情報を表示するためのクラス。
/// </summary>
public class DebugDisplay : MonoBehaviour {

	#region 設定用変数
	[SerializeField]
	float m_fCanvasPlaneDistance = 0.1f;
	#endregion

	#region 内部変数
	[Serializable]
	public class DebugDisplayTextIns {
		public string TagName;
		public string Message;
		public int DisplayPriority;
	}

	[SerializeField]
	Text remainText;
	[SerializeField]
	Text logText;

	List<DebugDisplayTextIns> m_lsSortedUpdateTexts = new List<DebugDisplayTextIns>();
	Dictionary<string, DebugDisplayTextIns> m_dicRemainTexts = new Dictionary<string, DebugDisplayTextIns>();

	[SerializeField]
	int m_iMaxLogLines = 6;

	List<string> m_lsLogTexts = new List<string>();

	[SerializeField]
	Canvas m_canvasDisplay;
	Camera m_camTarget;
	#endregion

	#region 外部関数（だがDebugToolsのみがアクセスする想定）
	public void UpdateDisplayText(string tagName, string message, int displayPriority)
	{
		if ( m_dicRemainTexts.ContainsKey(tagName) == false )
		{
			RegisterRemainText(tagName, message, displayPriority);
		}
		UpdateRemainTextDictionary(tagName, message, displayPriority);
	}

	public void UnregisterDisplayText(string tagName)
	{
		UnregisterRemainText(tagName);
	}

	public void AddLog(string message)
	{
		// 登録
		if ( m_lsLogTexts.Count >= m_iMaxLogLines )
		{
			// 最大行数超えたら先頭から削除
			m_lsLogTexts.RemoveAt(0);
		}
		m_lsLogTexts.Add(message);

		UpdateLogTextUI();
	}
	#endregion

	#region 内部関数
	private void Update()
	{
		if ( m_canvasDisplay.worldCamera == null )
		{
			FindTargetCamera();
			if ( m_camTarget )
			{
				m_canvasDisplay.worldCamera = m_camTarget;
				m_canvasDisplay.planeDistance = m_fCanvasPlaneDistance;
			}
		}
	}

	void FindTargetCamera()
	{
		var camObj = VRObjectManager.GetInstance().GetBaseCameraObject();
		if ( camObj == null )
			return;

		m_camTarget = camObj.GetComponent<Camera>();
	}

	void RegisterRemainText(string tagName, string displayText, int displayPriority)
	{
		var newText = new DebugDisplayTextIns();
		newText.TagName = tagName;
		newText.Message = displayText;
		newText.DisplayPriority = displayPriority;

		m_dicRemainTexts[tagName] = newText;

		m_lsSortedUpdateTexts.Add(newText);
		SortRemainTextList();
	}

	void UnregisterRemainText(string tagName)
	{
		DebugDisplayTextIns target = null;
		if(m_dicRemainTexts.TryGetValue(tagName, out target) )
		{
			m_dicRemainTexts.Remove(tagName);
			m_lsSortedUpdateTexts.Remove(target);
			UpdateRemainTextUI();
		}
		else
		{
			//Debug.LogError(tagName + "というデバッグテキストは見つかりませんでした");
		}
	}

	void UpdateRemainTextDictionary(string tagName, string displayText, int displayPriority)
	{
		m_dicRemainTexts[tagName].Message = displayText;

		if( m_dicRemainTexts[tagName].DisplayPriority	!= displayPriority )
		{
			m_dicRemainTexts[tagName].DisplayPriority	= displayPriority;
			SortRemainTextList();
		}
		UpdateRemainTextUI();
	}

	void SortRemainTextList()
	{
		m_lsSortedUpdateTexts.Sort((a, b) => b.DisplayPriority - a.DisplayPriority);
	}

	void UpdateLogTextUI()
	{
		logText.text = "";
		foreach ( var mes in m_lsLogTexts )
		{
			logText.text += mes + "\n";
		}
	}

	void UpdateRemainTextUI()
	{
		remainText.text = "";
		foreach ( var displayText in m_lsSortedUpdateTexts )
		{
			remainText.text += displayText.Message + "\n";
		}
	}
	#endregion
}
