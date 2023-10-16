using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpriteButton : MonoBehaviour, IPointerClickHandler
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //


	// ------------- //
	// -- 公有成员
	// ------------- //
	public UnityEvent OnClick = new UnityEvent();

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void OnPointerClick(PointerEventData eventData)
	{
		OnClick?.Invoke();
	}

	// ------------- //
	// -- 私有方法
	// ------------- //
}
