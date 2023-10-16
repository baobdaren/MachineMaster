using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NiceToggle : Toggle
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

	// ----------------//
	// --- Unity消息
	// ----------------//


	// ----------------//
	// --- 公有方法
	// ----------------//
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);
	}

	//public override void OnPointerExit(PointerEventData eventData)
	//{
	//       base.OnPointerExit(eventData);
	//       //base.OnPointerExit(eventData);
	//       Debug.LogError("Exit");
	//       this.DoStateTransition(SelectionState.Normal, true);
	//}


	public override void OnPointerUp(PointerEventData eventData)
	{
		base.OnPointerUp(eventData);
		base.OnDeselect(eventData);
	}

	//public override void OnMove(AxisEventData eventData)
	//{
	//	base.OnMove(eventData);
	//       Debug.LogError("OnMove");
	//}
	//public override void OnPointerEnter(PointerEventData eventData)
	//{
	//       base.OnPointerEnter(eventData);
	//       Debug.LogError("Enter");
	//   }

	//public override void OnPointerDown(PointerEventData eventData)
	//{
	//	base.OnPointerDown(eventData);
	//       Debug.LogError("Down");
	//   }

	//public override void OnDeselect(BaseEventData eventData)
	//{
	//	base.OnDeselect(eventData);
	//       Debug.LogError("OnDeselect");
	//   }

	//public override void OnSelect(BaseEventData eventData)
	//{
	//	base.OnSelect(eventData);
	//       Debug.LogError("OnSelect");

	//   }
}
