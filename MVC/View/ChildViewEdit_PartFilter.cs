using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class ChildViewEdit_PartFilter : BaseChildView
{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]
	private Dictionary<Toggle, PartTypes> _toggle2TypeDic;
	[SerializeField][ChildGameObjectsOnly]
	private ToggleGroup _toggleGroup;

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	private ModelPlayerPartEditor Model => ModelPlayerPartEditor.Instance;

	// ----------------//
	// --- Unity消息
	// ----------------//
	private void Awake()
	{
		_toggleGroup.allowSwitchOff = true;
		MyView.SetActive(false);
	}

	private void Start()
	{
		foreach (var item in _toggle2TypeDic)
		{
			item.Key.onValueChanged.AddListener((bool _) => { OnTgooleChanged(); });
		}
		//ControllerEdit.Instance.OnStartEditPart.AddListener(() => { _toggleGroup.SetAllTogglesOff(); });
	}

	private void OnEnable()
	{
		_toggleGroup.SetAllTogglesOff();
	}
	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetDisplay(bool active)
	{
		MyView.SetActive(active);
		_toggleGroup.SetAllTogglesOff(false);
		OnTgooleChanged();
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	/// <summary>
	/// 打开了任意一个零件类型，只能连接该类型
	/// </summary>
	private void OnTgooleChanged()
	{
		if (_toggleGroup.AnyTogglesOn())
		{
			Model.ConnectablePartType = _toggle2TypeDic[_toggleGroup.GetFirstActiveToggle()];
		}
		else
		{
			Model.ConnectablePartType = null;
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
