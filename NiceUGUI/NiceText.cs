using TMPro;
using UnityEngine;

public class NiceText: TextMeshProUGUI
{
	// ------------- //
	// -- 序列化
	// ------------- //
	[SerializeField]
	public bool EnableLocalization = true;

	// ------------- //
	// -- 私有成员
	// ------------- //
	private string originalText;

	// ------------- //
	// -- 公有成员
	// ------------- //
	public override string text 
	{
		get => base.text;
		set
		{
			base.text = value;
			ForceTranslateText();
		}
	}

	// ------------- //
	// -- Unity 消息
	// ------------- //
	protected override void Awake()
	{
		originalText = text;
		base.Awake();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		ForceTranslateText();
	}

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void ForceTranslateText()
	{
		if (Application.isPlaying) return;
		base.text = LocalizationManager.Instance.Translate(originalText);
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
}
