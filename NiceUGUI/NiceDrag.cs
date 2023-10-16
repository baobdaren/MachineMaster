using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class NiceDrag : SerializedMonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public class DragEvent : UnityEvent { }
	public class DragPosEvent : UnityEvent<Vector2> { }
	public DragEvent BeginDrag = new DragEvent();
	public DragEvent EndDrag = new DragEvent();
	public DragEvent Drag = new DragEvent();
	//public DragPosEvent DragPos = new DragPosEvent();

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//
	public void OnBeginDrag(PointerEventData eventData)
	{
		BeginDrag?.Invoke();
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (transform is RectTransform)
		{
			transform.position = Mouse.current.position.ReadValue();
		}
		else
		{
			transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		}
		Drag?.Invoke();
		//DragPos?.Invoke(transform.position);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		EndDrag?.Invoke();
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void ForceDrag()
	{ 
		StartCoroutine(Cor_ForceDrag());
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	private IEnumerator Cor_ForceDrag()
	{
		if (Mouse.current.leftButton.isPressed == false) yield return 0;
		OnBeginDrag(null);
		OnDrag(null);
		while (Mouse.current.leftButton.isPressed)
		{
			yield return 0;
		}
		OnEndDrag(null);
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
