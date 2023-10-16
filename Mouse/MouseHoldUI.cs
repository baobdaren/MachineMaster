using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[DisallowMultipleComponent]
public class MouseHoldUI : MonoBehaviour, IPointerDownHandler, IPointerExitHandler, IPointerEnterHandler
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //
	[Serializable] public class NonArgEvent : UnityEvent<PartTypes> { public NonArgEvent() { } }
	[SerializeField] public NonArgEvent OnHold = new NonArgEvent();
	[SerializeField] public NonArgEvent OnHoldStart = new NonArgEvent();
	[OnValueChanged("Edit_SetColor")]
	[SerializeField] private Color ImgNormal;
	[SerializeField] private Color ImgHighLight;
	[SerializeField] private Color ImgPressed;
	[OnValueChanged("Edit_SetColor")]
	[SerializeField] private Color TexNormal;
	[SerializeField] private Color TexHighLight;
	[SerializeField] private Color TexPressed;


	// ------------------ //    
	// --- 公有成员    
	// ------------------ //
	public PartTypes _partType;


	// ------------------ //   
	// --- 私有成员    
	// ------------------ //
	private bool exitCanDrag = false;
	private NiceText _tex;
	private Image _img;

	// ------------------ //    
	// --- Unity消息    
	// ------------------ //
	private void Awake()
	{
		_tex = GetComponentInChildren<NiceText>();
		_img = GetComponentInChildren<Image>();
	}



	public void OnPointerDown(PointerEventData eventData)
	{
		exitCanDrag = true;
		SetStateColor(ButtonState.Pressed);
	}


	public void OnPointerExit(PointerEventData eventData)
	{
		//if (exitCanDrag && Mouse.current.leftButton.isPressed)
		if (exitCanDrag && Mouse.current.leftButton.isPressed)
		{
			exitCanDrag = false;
			OnHold?.Invoke(_partType);
		}
		else
		{
			exitCanDrag = false;
		}
		SetStateColor(ButtonState.Normal);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		SetStateColor(ButtonState.High);
	}


	// ------------------ //    
	// --- 公有方法   
	// ------------------ //

	// ------------------ //   
	// --- 私有方法
	// ------------------ //
	private void SetStateColor(ButtonState buttonState)
	{
		switch (buttonState)
		{
			case ButtonState.Normal:
				_img.color = ImgNormal;
				_tex.color = TexNormal;
				break;
			case ButtonState.High:
				_img.color = ImgHighLight;
				_tex.color = TexHighLight;
				break;
			case ButtonState.Pressed:
				_img.color = ImgPressed;
				_tex.color = TexPressed;
				break;
		}
	}

	private enum ButtonState
	{
		Normal, High, Pressed
	}

	private void Edit_SetColor()
	{
		GetComponentInChildren<TextMeshProUGUI>().color = TexNormal;
		GetComponentInChildren<Image>().color = ImgNormal;
	}
}
