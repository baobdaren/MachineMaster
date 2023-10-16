using UnityEngine;
using UnityEngine.Events;

public class NiceButtonWithStateAbandon : NiceButton
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	[SerializeField]
	public ButtonState State;
	public GameObject Obj { get => _obj; }
	public class StateButtonClickEvent:UnityEvent<NiceButtonWithStateAbandon> { }
	public StateButtonClickEvent onStateClick = new StateButtonClickEvent();

	// ----------------//
	// --- 私有成员
	// ----------------//
	[SerializeField]
	private GameObject _obj;

	// ----------------//
	// --- Unity消息
	// ----------------//
	protected override void Awake()
	{
		onClick.AddListener(() => { onStateClick?.Invoke(this); });
	}

	// ----------------//
	// --- 公有方法
	// ----------------//
	public void ReverseState()
	{
		switch (State)
		{
			case ButtonState.ON:
				State = ButtonState.OFF;
				break;
			case ButtonState.OFF:
				State = ButtonState.ON;
				break;
			default:
				break;
		}
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
	public enum ButtonState
	{ 
		ON, OFF, NONE
	}
}

