using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class TestCmdButton : SerializedMonoBehaviour
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	Key TestKey;

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		if (TryGetComponent<CmdSlider>(out CmdSlider cmdSlider))
		{
			cmdSlider.CmdKey = TestKey;
		}
		if (TryGetComponent<CmdToggle>(out CmdToggle cmdToggle))
		{
			cmdToggle.CmdKey = TestKey;
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
