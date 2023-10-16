using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PartSelector
{
	// ------------- //
	// -- 序列化
	// ------------- //

	// ------------- //
	// -- 私有成员
	// ------------- //
	private List<BasePartCtrl> ClickedParts = new List<BasePartCtrl>(8);
	private List<BasePartCtrl> OverlapedPartsCache = new List<BasePartCtrl>(8);
	private BasePartCtrl LastClickedPart = null;

	// ------------- //
	// -- 公有成员
	// ------------- //
	public static PartSelector Instance { get; private set; } = new PartSelector();
	//public UnityEvent<BasePartCtrl> OnSelectPart = new UnityEvent<BasePartCtrl>();

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void	SelectClickedPart(BasePartCtrl clickedFrontpart)
	{
		Debug.LogError("点击了零件 " +  clickedFrontpart);

		OverlapedPartsCache.Clear();
		PartManager.Instance.GetMouseOverlapedParts	(OverlapedPartsCache);
		bool resetClickedList = ClickedParts.Count != OverlapedPartsCache.Count;
		if(!resetClickedList) 
		{
			for (int i = 0; i < ClickedParts.Count; i++)
			{
				if (ClickedParts[i] != OverlapedPartsCache[i])
				{
					resetClickedList = true;
					break;
				}
			}
		}
		if (resetClickedList)
		{
			(OverlapedPartsCache, ClickedParts) = (ClickedParts, OverlapedPartsCache);
		}
		OverlapedPartsCache.Clear();

		int selectPartIndex;
		if (LastClickedPart == null || !ClickedParts.Contains(LastClickedPart))
		{
			selectPartIndex = 0;
		}
		else
		{
			int nextClickIndexTest = ClickedParts.IndexOf(LastClickedPart) + 1;
			selectPartIndex = (nextClickIndexTest == ClickedParts.Count) ? 0 : nextClickIndexTest;
		}
		OpenViewToEditPart(ClickedParts[selectPartIndex]);
		LastClickedPart = ClickedParts[selectPartIndex];
	}

	//public void SelectDragingPart(BasePartCtrl dragingPart)
	//{
	//	BasePartCtrl editingPart = PartManager.Instance.EditingPart;
	//	dragingPart.PartDragCmpnt.CancleDrag();
	//	BasePartCtrl selectPart = dragingPart;
	//	// 当没有编辑零件时无需挑选
	//	// 当编辑零件就是拖拽零件时无需挑选
	//	if(editingPart != null && dragingPart != editingPart)
	//	{
	//		OverlapedPartsCache.Clear();
	//		GetMouseOverlapedParts(OverlapedPartsCache);
	//		if(OverlapedPartsCache.Contains(editingPart))
	//		{
	//			selectPart = editingPart;
	//		}
	//	}



	//}

	public void ClearClickedPart()
	{
		ClickedParts.Clear();
	}
	// ------------- //
	// -- 私有方法
	// ------------- //
	private PartSelector()
	{
		
	}

	private void OpenViewToEditPart(BasePartCtrl ctrl)
	{
		if(ctrl.IsPlayerPart)
		{
			UIManager.Instance.OpenView<MainViewPlayerPartEditor>(new ViewEditOpenData(ctrl as PlayerPartCtrl));
		}
		else 
		{
			UIManager.Instance.OpenView<MainViewScenePartEditor>(new ViewScenePartOpenData(ctrl as ScenePartCtrl));
		}
	}

}
