using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildViewEdit_Settings : BaseChildView
{
    // --------------- //
    // -- 序列化
    // --------------- //
    [SerializeField]
    Slider _slider;


	// --------------- //
	// -- 公有成员
	// --------------- //

	// --------------- //
	// -- 私有成员
	// --------------- //
	private TMP_InputField _sizeTex;

	// --------------- //
	// -- Unity消息
	// --------------- //
	private void Awake ( )
	{
		_slider.value = 0;
		_slider.onValueChanged.AddListener(SetSizeText);
	}
	private void Start ( )
	{
		ControllerPlayerPartEditor.Instance.OnStartEditPlayerPart.AddListener(OnEditNewPlayerPart);
	}
	private void OnEnable ( )
	{
		Regist();
	} 
	private void OnDisable ( )
	{
		UnRegist();
	}

	// --------------- //
	// -- 公有方法
	// --------------- //


	// --------------- //
	// -- 私有方法
	// --------------- //
	private void OnEditNewPlayerPart (PlayerPartCtrl editingPart)
	{
		UnRegist();
		// 当新的值大于原来的上限，或者原来的值大于新的上限，通知会使尺寸索引修改。
		(float, float, bool) settings = PartConfig.Instance.GetPartSizeSliderSetting(editingPart.MyPartType);
		// 先扩充到最大范围
		_slider.minValue = Mathf.Min(settings.Item2, settings.Item1);
		_slider.maxValue = Mathf.Max(settings.Item1, settings.Item2);
		// 必须先设置是否为整
		_slider.wholeNumbers = settings.Item3;
		_slider.SetValueWithoutNotify(editingPart.Size);
		SetSizeText(_slider.value);
		//Debug.LogError($"尺寸最小 {settings.Item1} 尺寸最大{settings.Item2} 使用整数{settings.Item3}");
		Regist();
	}

	private void SetSizeText(float v)
	{
		if (_sizeTex == null)
		{
			_sizeTex = _slider.GetComponentInChildren<TMP_InputField>();
		}
		_sizeTex.text = string.Format("{0:N2}", v);
		//LayerWorker.Instance.Wrok(true);
	}

	private void Regist()
	{
		//Debug.Log("enable 尺寸设置界面");
		_slider.onValueChanged.AddListener((float curValue) =>
		{
			(ModelPlayerPartEditor.Instance.EditingPlayerPartCtrl as PlayerPartCtrl).Size = curValue;
		});
	}

	private void UnRegist()
	{
		//Debug.Log("disable 尺寸设置界面");
		_slider.onValueChanged.RemoveAllListeners();
	}
}
