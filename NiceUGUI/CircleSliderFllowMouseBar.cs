using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System;
using UnityEngine.InputSystem;

public class CircleSliderFllowMouseBar : SerializedMonoBehaviour, IDragHandler,IEndDragHandler
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public Action<float> OnRoating;

	// ----------------// 
	// --- 私有成员
	// ----------------//
	[SerializeField]
	private GameObject _rotateBar;
	private CircleSlider _rotateCenter;
	private float _rotateRadius;
	private float _dragBar;
	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		_rotateCenter = GetComponentInParent<CircleSlider>();
		_rotateRadius = Vector2.Distance(_rotateBar.transform.position, _rotateCenter.transform.position);
		_dragBar = Vector2.Distance(transform.position, _rotateCenter.transform.position);
	}
	public void OnDrag(PointerEventData eventData = null)
	{
		transform.position = CameraActor.Instance.MouseWorldPos;
		Vector3 dir = CameraActor.Instance.MouseWorldPos - _rotateCenter.transform.position;
		float angle = Vector2.SignedAngle(Vector2.right, dir);
		SetDisplay(angle, true);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.position = _rotateCenter.transform.position + (_rotateBar.transform.position - _rotateCenter.transform.position) * _dragBar;
	}
	// ----------------//
	// --- 公有方法
	// ----------------//
	public void Init(float angle)
	{
		SetDisplay(angle, false);
		transform.position = _rotateCenter.transform.position + (_rotateBar.transform.position - _rotateCenter.transform.position) * _dragBar;
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	private void SetDisplay(float angle, bool notify)
	{
		Vector3 dir = new Vector2(_rotateRadius * Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
		_rotateBar.transform.SetPositionAndRotation(_rotateCenter.transform.position + dir * _rotateRadius, Quaternion.Euler(0, 0, angle));
		if (notify)
		{
			OnRoating?.Invoke(angle > 0 ? angle : angle + 360);
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
