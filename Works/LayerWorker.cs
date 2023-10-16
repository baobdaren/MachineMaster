using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.U2D;

/// <summary>
/// 自动推断零件与轴承的层级
/// </summary>
public class LayerWorker
{
	// ------------- //
	// -- 序列化
	// ------------- //
	//readonly List<(PartTypes, int)> SortOrderTable = new List<(PartTypes, int)>()
	//{
	//	(PartTypes.ScecneCustom, -1),
	//	(PartTypes.Wheel, 5),
	//	(PartTypes.Wheel, 4),
	//	(PartTypes.Wheel, 3),
	//	(PartTypes.Wheel, 2),
	//	(PartTypes.Wheel, 1),
	//	(PartTypes.Wheel, 0),
	//	(PartTypes.Engine, -1),
	//	(PartTypes.Gear, 7),
	//	(PartTypes.Gear, 6),
	//	(PartTypes.Gear, 5),
	//	(PartTypes.Gear, 4),
	//	(PartTypes.Gear, 3),
	//	(PartTypes.Gear, 2),
	//	(PartTypes.Gear, 1),
	//	(PartTypes.Gear, 0),
	//	(PartTypes.JETEngine, -1),
	//	(PartTypes.Spring, -1),
	//	(PartTypes.Presser, -1),
	//	(PartTypes.Steel, -1),
	//	(PartTypes.Rail, -1),
	//	(PartTypes.AVSensor, -1),
	//	(PartTypes.DISSensor, -1),
	//};

	// ------------- //
	// -- 私有成员
	// ------------- //
	public static LayerWorker Instance = new LayerWorker();

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	//public void Wrok(bool sortEditOrPhyscisAccessors)
	//{
	//	List<AbsPartAccessorBase> accessors = new List<AbsPartAccessorBase>(PlayerPartManager.Instance.AllPlayerPartCtrls.Count);
	//	foreach (var item in PlayerPartManager.Instance.AllPlayerPartCtrls)
	//	{
	//		accessors.Add(sortEditOrPhyscisAccessors ? item.MyEditPartAccesstor : item.MyPhysicsPartAccesstor);
	//	}
	//	Sort(accessors, sortEditOrPhyscisAccessors);
	//}

	// ------------- //
	// -- 私有方法
	// ------------- //
	//private int GetPartSortIndexByPartType(AbsPartAccessorBase partAccessor)
	//{
	//	return GetPartSortIndexByPartType(partAccessor.MyPartCtrl.MyPartType, (int)partAccessor.MyPartCtrl.Size);
	//}

	//private int GetPartSortIndexByPartType(PartTypes partType, int size)
	//{
	//	int? recordIndex = null;
	//	for (int i = 0; i < SortOrderTable.Count; i++)
	//	{
	//		(PartTypes, int) itemSortTable = SortOrderTable[i];
	//		if (itemSortTable.Item1 == partType)
	//		{
	//			if (itemSortTable.Item2 == -1 || itemSortTable.Item2 == size)
	//			{
	//				//Debug.Log(partType.ToString() + $":{size} 序号为 {i}");
	//				return i;
	//			}
	//		}
	//	}
	//	Debug.Assert(recordIndex.HasValue, $"{partType} : {size}");
	//	return recordIndex.Value;
	//}

	/// <summary>
	/// 获取连接插入的位置，返回插入节点的前一个点
	/// </summary>
	/// <param name="conn"></param>
	/// <param name="isEditOrPhysicsAccessor"></param>
	/// <returns></returns>
	//private AbsPartAccessorBase GetConnectionInsertPosition(BasePartConnection conn, bool isEditOrPhysicsAccessor)
	//{
	//	AbsPartAccessorBase accessorInsetAfter = null;
	//	int curResultSortOrder = -2;
	//	foreach (KeyValuePair<IConnectableCtrl, int> itemPart in conn.ConnectedParts)
	//	{
	//		if (itemPart.Key is ScenePartCtrl)
	//		{
	//			continue;
	//		}
	//		// 场景零件不做比较
	//		PlayerPartCtrl playerPartCtrl = itemPart.Key as PlayerPartCtrl;
	//		int itemPartConnectToOrder = GetPartSortIndexByPartType(playerPartCtrl.MyPartType, (int)playerPartCtrl.CoreData.Size);
	//		if (accessorInsetAfter == null || curResultSortOrder < itemPartConnectToOrder)
	//		{
	//			accessorInsetAfter = isEditOrPhysicsAccessor ? playerPartCtrl.MyEditPartAccesstor : playerPartCtrl.MyPhysicsPartAccesstor;
	//			curResultSortOrder = itemPartConnectToOrder;
	//		}
	//	}
	//	return accessorInsetAfter;
	//}

	/// <summary>
	/// 使用链表，第一遍排序零件，第二遍插入轴承
	/// </summary>
	/// <param name="partAccessors"></param>
	//private void Sort(List<AbsPartAccessorBase> partAccessors, bool sortEditOrPhysics)
	//{
	//	if (partAccessors.Count == 0)
	//	{
	//		return;
	//	}
	//	List<AbsPartAccessorBase> partAccessorsCopy = new List<AbsPartAccessorBase>(partAccessors);
	//	bool isEditAccessor = partAccessors[0].MyPartCtrl.MyEditPartAccesstor == partAccessors[0];
	//	// 根据零件类型排序
	//	//partAccessorsCopy.Sort((pa, pb) => { return GetPartSortIndexByPartType(pa) - GetPartSortIndexByPartType(pb); });
	//	partAccessorsCopy.Sort((pa, pb) => { return pa.MyPartCtrl.CoreData.Layer - pa.MyPartCtrl.CoreData.Layer; });
	//	List<ILayerSettable> settables = new List<ILayerSettable>(partAccessorsCopy);
	//	// 插入连接光标
	//	for (int i = 0; i < PartConnectionManager.Instance.AllEditConnection.Count; i++)
	//	{
	//		BasePartConnection itemConnection = PartConnectionManager.Instance.AllEditConnection[i];
	//		AbsPartAccessorBase insertConnectToPartFront = GetConnectionInsertPosition(itemConnection, isEditAccessor);
	//		settables.Insert(settables.IndexOf(insertConnectToPartFront)+1, sortEditOrPhysics ? itemConnection : itemConnection.BearingPhysics);
	//	}
	//	// 根据链表中的顺序一次设置层级
	//	// 每个单独设置+100，因为可能零件内部使用后续几个
	//	int lastOrder = 0;
	//	int countIndex = 0;
	//	foreach (ILayerSettable itemSettableNode in settables)
	//	{
	//		itemSettableNode.SetOrder = lastOrder;
	//		lastOrder += 100;
	//		//Debug.Log($"排序 {countIndex++} ：{itemSettableNode}");
	//	}
	//}

}
