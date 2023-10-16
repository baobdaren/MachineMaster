using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 1:拖拽零件时, 推断拖拽零件放置在与覆盖冲突零件不同层级
/// 2:拖拽零件时, 推断零件和吸附零件在同一层级
/// 3:层级数量自动增加
/// 4:层级列表顺序可下拖拽修改(类似Unity layer的功能)
/// 5:不同层级使用颜色 / 透明度区分
/// 
/// 
/// </summary>
public class PartLayerManager: ISaveable
{
	// ------------- //
	// -- 序列化
	// ------------- //
	public Dictionary<int, List<BasePartCtrl>> Layers { private set; get; }


	// ------------- //
	// -- 私有成员
	// ------------- //

	// ------------- //
	// -- 公有成员
	// ------------- //

	// ------------- //
	// -- Unity 消息
	// ------------- //

	// ------------- //
	// -- 公有方法
	// ------------- //
	public void ResortPart(BasePartCtrl ctrl)
	{
		if (ContainsPart(ctrl) == false)
		{
			AddPart(ctrl, 0);
		}
		// 获取覆盖冲突零件

		// 获取吸附零件???
	}

	public void ResortPart(BasePartCtrl ctrl, BasePartCtrl snapTarget)
	{
		if (ContainsPart(ctrl) == false)
		{
			AddPart(ctrl, 0);
		}
		// 获取覆盖冲突零件
		List<BasePartCtrl> _conflictPartList = new List<BasePartCtrl>();
		
	}

	public void AddPart(BasePartCtrl ctrl, int sortIndex)
	{
		Layers[sortIndex].Add(ctrl);
	}

	public void RemovePart(BasePartCtrl ctrl)
	{
		if (!ContainsPart(ctrl))
		{
			Debug.Assert(false, "移除的零件 不在层级注册中");
		}
		else
		{
			foreach (var item in Layers)
			{
				item.Value.Remove(ctrl);
			}
		}
	}

	public bool ContainsPart(BasePartCtrl ctrl)
	{
		foreach (var item in Layers)
		{
			if (item.Value.Remove(ctrl)) return true;
		}
		return false;
	}

	public void LoadArchive(ArchiveManager.Archive archive)
	{
		throw new System.NotImplementedException();
	}

	public void SaveIntoArchive(ArchiveManager.Archive archive)
	{
		//Dictionary<int, List<int>> Layers;
		//throw new System.NotImplementedException();
	}

	// ------------- //
	// -- 私有方法
	// ------------- //

	public struct PartLayerSortData
	{
		public int Index;
		public int PartID;
	}
}
