using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
[Serializable]
public class MouseDoubleClick : MonoBehaviour,IPointerClickHandler
{
	// ------------------ //
	// --- 序列化
	// ------------------ //
	[SerializeField]
	private readonly float clickDlay = 0.3f;

	// ------------------ //
	// --- 公有成员
	// ------------------ //
	public class MonoAction : UnityEvent { }
	[SerializeField]
	public MonoAction OnDoubleClick = new MonoAction();


	// ------------------ //
	// --- 私有成员
	// ------------------ //
	float lastClickTime = 0;


	// ------------------ //
	// --- Unity消息
	// ------------------ //
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Time.realtimeSinceStartup - lastClickTime <= clickDlay)
			{
				//Debug.LogError("double click");
				OnDoubleClick?.Invoke();
			}
			lastClickTime = Time.realtimeSinceStartup;
		}
	}


	private void OnDestroy()
	{
		OnDoubleClick.RemoveAllListeners();
	}
	// ------------------ //
	// --- 公有方法
	// ------------------ //


	// ------------------ //
	// --- 私有方法
	// ------------------ //
}
