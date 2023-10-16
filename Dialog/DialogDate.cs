using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct DialogDate
{
	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //
	[GUIColor("GetContentColor")]
	public string DialogContent;
	public bool IsPlayerContent;
	public bool HasEvent;
	[HideIf("@! HasEvent")]
	[GUIColor("GetContentColor")]
	public UnityEvent OnContentEnd;

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void Finish(int r = 0)
	{
		if (HasEvent)
			OnContentEnd?.Invoke();
	}

	public void Log()
	{
		Debug.Log("对话结束");
	}
	// ------------- //
	// -- 私有方法
	// ------------- //
#if UNITY_EDITOR
	public Color GetContentColor()
	{
		return IsPlayerContent ? Color.green : Color.yellow;
	}
#endif
}
