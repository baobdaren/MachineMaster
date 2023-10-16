using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class AdsorablebCircle1 /* :AbsAdsorbableGameObject,IDragHandler,IBeginDragHandler,IEndDragHandler*/
{
	//// ------------------ //
	//// --- 序列化
	//// ------------------ //
	//public bool EnableDrag = true;
	//public class OnDragEvent : UnityEvent { }
	//public class OnDragPosEvent : UnityEvent<Vector2> { }

	//// ------------------ //
	//// --- 公有成员
	//// ------------------ //
	//public static bool IsDraging = false;

	//public OnDragPosEvent OnDraging = new OnDragPosEvent();
	//public OnDragEvent OnDragStart = new OnDragEvent();
	//public OnDragEvent OnDragEnd = new OnDragEvent();

	//public PartType SelfPartType { get; set; }

	//// ------------------ //
	//// --- 私有成员
	//// ------------------ //
	//private bool draging = false;                                                                                     


	//// 拖拽的初始位置和拖拽对象的偏移
	//private static Vector2 dragScreenOffset = Vector2.zero;

	//// ------------------ //
	//// --- Unity消息
	//// ------------------ //
	//private void Start()
	//{
	//	if (EnableAdsorb)
	//	{
	//		RegistAdsorbableTarget(MoveTransform);
	//	}
	//}

	//public void OnBeginDrag(PointerEventData eventData = null)
	//{
	//	if (!enabled || !EnableDrag || !Mouse.current.leftButton.isPressed)
	//	{
	//		return;
	//	}
	//	StartDrag();
	//}

	//public void OnDrag(PointerEventData eventData)
	//{
	//	if (eventData == null) { }
	//	//Debug.LogError("Unity 拖拽消息 - " + name);
	//	if (!enabled || !EnableDrag)
	//	{
	//		return;
	//	}
	//	Drag();
	//}

	//public void OnEndDrag(PointerEventData eventData= null)
	//{
	//	FinishDrag();
	//}

	//// ------------------ //
	//// -- 公有方法
	//// ------------------ //
	///// <summary>
	///// 强制拖拽对象，忽略了拖拽开始的过程，直接使得对象跟随正在按下的鼠标
	///// </summary>
	//public void ForceDrag()
	//{
	//	if (Mouse.current.leftButton.isPressed)
	//	{
	//		StartCoroutine(CorForceDrag());
	//	}
	//	else
	//	{
	//		Debug.LogError("启用强制鼠标拖拽时，鼠标左键没有处于按下状态");
	//	}
	//}


	//// ------------------ //
	//// --- 私有方法
	//// ------------------ //
	//private void StartDrag()
	//{
	//	if (!Mouse.current.leftButton.isPressed || IsDraging)
	//	{
	//		return;
	//	}
	//	IsDraging = true;
	//	//Debug.LogError("Drag 进程 Start");
	//	dragScreenOffset = Mouse.current.position.ReadValue() -(Vector2)CameraActor.Instance.MainCamera.WorldToScreenPoint(MoveTransform.position);
	//	//Debug.LogError("物体的屏幕坐标" + (Vector2)CameraActor.Instance.MainCamera.WorldToScreenPoint(MoveTransform.position));
	//}
	//private void Drag()
	//{
	//	if (!Mouse.current.leftButton.isPressed)
	//	{
	//		return;
	//	}
	//	//Debug.LogError("Drag 进程 Ing");
	//	Vector3 mouseScreenPos = Mouse.current.position.ReadValue() - dragScreenOffset;
	//	mouseScreenPos += (Vector3.forward * 10);
	//	Vector3 newPos = CameraActor.Instance.MainCamera.ScreenToWorldPoint(mouseScreenPos);
	//	newPos.Set(newPos.x, newPos.y, 0);
	//	if (draging == false)
	//	{
	//		OnDragStart?.Invoke();
	//	}
	//	draging = true;
	//	Cursor.visible = false;
	//	MoveTransform.position = newPos;
	//	//Debug.Log("当前位置"+ MoveTransform.position.ToString());
	//	if (EnableAdsorb)
	//	{
	//		Adsorb(MoveTransform);
	//	}
	//	OnDraging?.Invoke(MoveTransform.position);
	//	//Debug.Log("当前位置" + MoveTransform.position.ToString());
	//	//Debug.DrawLine(MoveTransform.position, CameraActor.Instance.MainCamera.ScreenToWorldPoint(dragScreenOffset + Mouse.current.position.ReadValue()));
	//}
	//private void FinishDrag()
	//{
	//	if (!Mouse.current.leftButton.wasReleasedThisFrame)
	//	{

	//		return;
	//	}
	//	IsDraging = false;
	//	//Debug.LogError("Drag 进程 Finish");
	//	OnDragEnd?.Invoke();
	//	Cursor.visible = true;
	//	XAdsortLine.SetActive(false);
	//	YAdsortLine.SetActive(false);
	//}

	//private IEnumerator CorForceDrag()
	//{
	//	transform.position = FirstAsset.Instance.CameraAction.MouseWorldPos;
	//	OnBeginDrag(null);
	//	while (Mouse.current.leftButton.isPressed)
	//	{
	//		Debug.Log("draging=================");
	//		OnDrag(null);
	//		yield return 0;
	//	}
	//	Debug.Log("draging end =======");
	//	OnEndDrag(null);
	//}
}
