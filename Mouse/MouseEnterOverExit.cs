using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MouseEnterOverExit : MonoBehaviour,IPointerEnterHandler,IPointerDownHandler
{
	// ------------------ //
	// --- 序列化
	// ------------------ //
	[SerializeField]
	public class OnEvent : UnityEvent { }

	public OnEvent OnPressEnter = new OnEvent();
	//public OnEvent OnOver = new OnEvent();
	//public OnEvent OnExit = new OnEvent();


	// ------------------ //
	// --- 公有成员
	// ------------------ //


	// ------------------ //
	// --- 私有成员
	// ------------------ //


	// ------------------ //
	// --- Unity消息
	// ------------------ //
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (Mouse.current.leftButton.isPressed)
		{
			//Debug.LogError("enter press" + name);
			OnPressEnter?.Invoke();
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		//Debug.LogError("down" + name);
		OnPressEnter?.Invoke();
	}

	// ------------------ //
	// --- 公有方法
	// ------------------ //


	// ------------------ //
	// --- 私有方法
	// ------------------ //
}
