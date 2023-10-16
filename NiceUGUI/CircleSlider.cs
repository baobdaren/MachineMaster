using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class CircleSlider : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private Image FillImage;

	// ----------------//
	// --- 公有成员
	// ----------------//
	[HideInInspector]
	public Action<float> OnValueChanged;

	// ----------------//
	// --- 私有成员
	// ----------------//
	private PlayerPartCtrl _targetPartCtrl;
	private bool working = false;
	private CircleSliderFllowMouseBar _followMouseBar;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		FillImage.type = Image.Type.Filled;
		_followMouseBar = GetComponentInChildren<CircleSliderFllowMouseBar>();
	}

	private void Start()
	{
		GetComponent<Canvas>().worldCamera = CameraActor.Instance.MainCamera;
		FillImage.fillAmount = 1;
		_followMouseBar.OnRoating += OnRotate;
	}

	private void Update()
	{
		transform.position = _targetPartCtrl.Position;
	}

	private void OnRotate(float angle)
	{
		SetValue(angle, true);
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void Display(PlayerPartCtrl partCtrl)
	{
		gameObject.SetActive(true);
		_targetPartCtrl = partCtrl;
		transform.position = _targetPartCtrl.Position;
		_followMouseBar.Init(_targetPartCtrl.Rotation.eulerAngles.z);
	}

	public void Hide()
	{
		gameObject.SetActive(false);
		_targetPartCtrl = null;
	}

	public void SetValue(float value, bool notify)
	{
		value /= 360f;
		if (Keyboard.current.leftCtrlKey.isPressed)
		{
			FillImage.fillAmount = value;
		}
		else
		{
			FillImage.fillAmount = Mathf.Round(value * 360f)/360;
		}
		if (notify)
		{
			Notify();
		}
	}
	// ----------------//
	// --- 私有方法
	// ----------------//
	private void Notify()
	{
		OnValueChanged?.Invoke(FillImage.fillAmount * 360);
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
