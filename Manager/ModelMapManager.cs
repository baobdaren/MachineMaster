using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArchiveManager;

public class ModelMapManager : Loadable<ModelMapManager>
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
	private Dictionary<PlayerPartCtrl, PartModelingNodeMap> _allPartsModelingMap = new Dictionary<PlayerPartCtrl, PartModelingNodeMap>();

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public PartModelingNodeMap GetMap(PlayerPartCtrl findKey)
	{
		if (_allPartsModelingMap.TryGetValue(findKey, out var result))
		{
			return result;
		}
		return null;
	}

	public PartModelingNodeMap CreateMap(PlayerPartCtrl partCtrl)
	{
		PartModelingNodeMap result = GetMap(partCtrl);
		if (result != null)
		{
			return result;
		}
		switch (partCtrl.MyPartType)
		{
			case PartTypes.JETEngine:
				result = new PartModelingNodeMap(ExpressionID.INPUT_JETEng);
				break;
			case PartTypes.Engine:
				result = new PartModelingNodeMap(ExpressionID.INPUT_ENGINE);
				break;
			case PartTypes.Presser:
				result = new PartModelingNodeMap(ExpressionID.INPUT_PRESSER);
				break;
		}
		Debug.LogError($"新增建模图 {partCtrl.GetHashCode()}");
		_allPartsModelingMap.Add(partCtrl, result);

		return result;
	}
	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override void OnResetData()
	{
		_allPartsModelingMap.Clear();
	}


	/// <summary>
	/// 建模界面的存档加载
	/// </summary>
	/// <param name="archive"></param>
	protected override void OnLoadedArchive(ArchiveManager.Archive archive)
	{
		_allPartsModelingMap.Clear();
		foreach (KeyValuePair<int, (List<ModelingExpressionNode>, List<ArchiveManager.SymbolConnectArchiveData>)> item in archive.partModelingMapsArchive)
		{
			// 该建模图所属的零件
			PlayerPartCtrl part = PlayerPartManager.Instance.AllPlayerPartCtrls[item.Key];
			// 该建模图所有节点
			List<ModelingExpressionNode> nodes = item.Value.Item1;
			// 该建模图所有节点连接关系
			List<ArchiveManager.SymbolConnectArchiveData> conns = item.Value.Item2;

			// 新建具有所有节点的建模图，并将其注册到管理列表
			_allPartsModelingMap.Add(part, new PartModelingNodeMap(nodes));
			part.ModelMap.AllNodes = nodes;
			// 遍历所有连接关系
			foreach (ArchiveManager.SymbolConnectArchiveData connItem in conns)
			{
				// 这个连接存储在的节点
				var nodeConn = nodes[connItem.inputIndex].InputConnections[connItem.inputIO];
				// 为这个节点设置连接关系（这个连接关系是为了加速建模计算）
				nodeConn.EnableConnection(nodes[connItem.inputIndex], connItem.inputIO, nodes[connItem.outputIndex], connItem.outputIO);
				part.ModelMap.AllConnection.Add(nodeConn);
			}
			Debug.LogError($" 加载存档零件 {part} 建模图 节点共有 {part.ModelMap.AllNodes.Count} 个 连接共有 {part.ModelMap.AllConnection.Count} 个");
		}
	}

	protected override void OnSaveingArchive(ArchiveManager.Archive archive)
	{
		archive.partModelingMapsArchive = new Dictionary<int, (List<ModelingExpressionNode>, List<ArchiveManager.SymbolConnectArchiveData>)>();
		foreach (PlayerPartCtrl partCtrl in PlayerPartManager.Instance.AllPlayerPartCtrls)
		{
			if (partCtrl.ModelMap != null)
			{
				int partCtrlIndex = PlayerPartManager.Instance.AllPlayerPartCtrls.IndexOf(partCtrl);
				List<SymbolConnectArchiveData> connList = new List<SymbolConnectArchiveData>();
				foreach (NodeInputConnection item in partCtrl.ModelMap.AllConnection)
				{
					int inputNodeIndex = partCtrl.ModelMap.AllNodes.IndexOf(item.InputNode);
					int outputNodeIndex = partCtrl.ModelMap.AllNodes.IndexOf(item.OutputNode);
					var data = new SymbolConnectArchiveData();
					data.inputIndex = inputNodeIndex;
					data.inputIO = item.InputNodeIOIndex;
					data.outputIndex = outputNodeIndex;
					data.outputIO = item.OutputNodeIOIndex;
					connList.Add(data);
				}
				archive.partModelingMapsArchive.Add(partCtrlIndex, (partCtrl.ModelMap.AllNodes, connList));
			}
		}
	}
	// ----------------//
	// --- 类型
	// ----------------//

}
