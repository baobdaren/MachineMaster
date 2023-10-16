using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixedSlider : Slider
{
	// ------------------ //    
	// --- 序列化    
	// ------------------ //

	// ------------------ //    
	// --- 公有成员    
	// ------------------ //

	// ------------------ //   
	// --- 私有成员    
	// ------------------ //

	// ------------------ //    
	// --- Unity消息    
	// ------------------ //
	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		base.OnDeselect(eventData);
	}

	// ------------------ //    
	// --- 公有方法   
	// ------------------ //

	// ------------------ //   
	// --- 私有方法
	// ------------------ //
}
