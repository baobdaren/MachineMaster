using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CmdToggle : Toggle
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public Key CmdKey
	{
		set
		{
			_keyCtrl = Keyboard.current[value];
		}
	}
	private KeyControl _keyCtrl;

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//

	private void Update()
	{
		if (Application.isPlaying && _keyCtrl != null && _keyCtrl.wasPressedThisFrame)
		{
			isOn = !isOn;
		}
	}

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
