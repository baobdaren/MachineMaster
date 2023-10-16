using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MouseListener : MonoBehaviour
{
	// ------------------ //
	// --- 序列化
	// ------------------ //


	// ------------------ //
	// --- 公有成员
	// ------------------ //
	public class OnEvent : UnityEvent { }
	public  OnEvent OnMouseDown = new OnEvent();
	public  OnEvent OnMouseUp = new OnEvent();

	public static MouseListener Instance 
	{
		get 
		{
			if (_object == null)
			{
				_object = new GameObject("Mouse");
				_object.AddComponent<MouseListener>();
			}
			return _object.GetComponent<MouseListener>();
		}
	}

	// ------------------ //
	// --- 私有成员
	// ------------------ //
	private static GameObject _object;

	// ------------------ //
	// --- Unity消息
	// ------------------ //


	public void Update()
	{
		if (Mouse.current.leftButton.isPressed)
		{
			OnMouseDown?.Invoke();
		}
		else if(Mouse.current.leftButton.wasReleasedThisFrame)
		{
			OnMouseUp?.Invoke();
		}
	}


	// ------------------ //
	// --- 公有方法
	// ------------------ //


	// ------------------ //
	// --- 私有方法
	// ------------------ //
}
