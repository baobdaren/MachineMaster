using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class AbsSectionPartBase : PlayerPartBase
{
	// -------------------- //
	// --- 序列化
	// -------------------- //


	// -------------------- //
	// --- 公有成员
	// -------------------- //


	// -------------------- //
	// --- 私有成员
	// -------------------- //

	// -------------------- //
	// --- Unity消息
	// -------------------- //


	// -------------------- //
	// --- 私有方法
	// -------------------- //


	//protected override void ChangeSize()
	//{
	//	int wantAmount = MyCtrlData.MainSizeIndex;
	//	Debug.LogError("修改多节零件尺寸之前-" + _sectionList.Count);
	//	while (_sectionList.Count < wantAmount)
	//	{
	//		var cloneSection = CreateSetcion();
	//		if (!cloneSection.gameObject.TryGetComponent(out MouseDoubleClick doubleClickCmpnt))
	//		{
	//			doubleClickCmpnt = cloneSection.AddComponent<MouseDoubleClick>();
	//		}
	//		doubleClickCmpnt.OnDoubleClick.AddListener(OnClick_AsEdittingGameObject);
	//		if (!cloneSection.gameObject.TryGetComponent(out MouseDragAbsort dragCmpnt))
	//		{
	//			dragCmpnt = cloneSection.AddComponent<MouseDragAbsort>();
	//		}
	//		//dragCmpnt.OnDragStart.AddListener(OnStartDragSection);
	//		//dragCmpnt.OnDragEnd.AddListener(OnEndDragSection);
	//		_sectionList.Add(cloneSection.transform);
	//		//Debug.LogError("创建节点" + (_sectionList.Count));
	//	}
	//	PlaceSection();
	//	ConnectSection();
	//	for (int i = wantAmount; i < _sectionList.Count; i++)
	//	{
	//		_sectionList[i].gameObject.SetActive(false);
	//	}
	//	foreach (var item in RigidChilds)
	//	{
	//		PartConfig.ApplyHighDrag(item);
	//	}
	//	this.OnSetLayer(Layer);
	//}

	//protected void OnStartDragSection()
	//{
	//	for (int i = 0; i < _sectionList.Count; i++)
	//	{
	//		SetSectionToDrag(_sectionList[i], true);
	//	}
	//}

	//protected void OnEndDragSection()
	//{
	//	for (int i = 0; i < _sectionList.Count; i++)
	//	{
	//		SetSectionToDrag(_sectionList[i], false);
	//	}
	//}


	////////////////////
	///// 公有
	////////////////////
	//public abstract GameObject CreateSetcion();
	//public abstract void ConnectSection();
	//public abstract void PlaceSection();

	//public void SetSectionToDrag(GameObject section, bool start)
	//{
	//	//Debug.LogError($"多节零件 进入状态 -> {(start ? ("拖拽") : ("设计"))}");
	//	const float normaleDrag = 1;
	//	const float normalAmgularDrag = 0.1f;
	//	foreach (var item in section.GetComponentsInChildren<Rigidbody2D>())
	//	{
	//		item.drag = start ? 2 : normaleDrag;
	//		item.gravityScale = start ? 0 : 1;
	//		item.angularDrag = start ? 2 : normalAmgularDrag;
	//	}
	//	//foreach (var item in GetComponentsInChildren<HingeJoint2D>())
	//	//{
	//	//	item.autoConfigureConnectedAnchor = false;
	//	//}
	//}
}
