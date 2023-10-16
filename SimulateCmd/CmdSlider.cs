using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class CmdSlider : Slider
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

	// ----------------//
	// --- 私有成员
	// ----------------//
	private KeyControl _keyCtrl;

	// ----------------//
	// --- Unity消息
	// ----------------//
	protected override void Update()
	{
		base.Update();
		if (Application.isPlaying && _keyCtrl != null && _keyCtrl.isPressed)
		{
			float yScroll = Mouse.current.scroll.ReadValue().y;
			if (yScroll != 0)
			{
				if (wholeNumbers)
				{
					value += (yScroll > 0 ? 1 : -1);
				}
				else
				{
					value += (yScroll > 0 ? 1 : -1) * (maxValue - minValue) / (Keyboard.current.spaceKey.isPressed ? 50 : 20);
				}
			}
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
