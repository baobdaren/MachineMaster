using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class NiceButton_TextEffect : NiceButton
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	private TextMeshProUGUI _text;
	private string _content;
	private Coroutine _cor;

	// ----------------//
	// --- Unity消息
	// ----------------//
	protected override void Awake()
	{
		_text = GetComponentInChildren<TextMeshProUGUI>();
		_content = _text.text;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		_cor = StartCoroutine(CorTextEffect());
	}


	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		_cor = null;
		StopAllCoroutines();
		ResetText();
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		if (_cor != null)
		{
			StopCoroutine(_cor);
		}
		ResetText();
	}
	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	private void ResetText()
	{
		_text.text = _content;
	}

	private IEnumerator CorTextEffect()
	{
		bool flash = false;
		string flashString = string.Format($"{_content}{"<sprite=1>"}");
		while (true)
		{
			_text.text = flash ? flashString : _content;
			flash = !flash;
			yield return new WaitForSeconds(0.3f);
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
