using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个可编程零件的所有建模内容/结构
/// </summary>
public class PartModelingNodeMap
{
	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//
	private ModelingExpressionNode Root;
	// ----------------//
	// --- 公有方法
	// ----------------//
	public List<ModelingExpressionNode> AllNodes = new List<ModelingExpressionNode>();
	public List<NodeInputConnection> AllConnection = new List<NodeInputConnection>();
	public PartModelingNodeMap(ExpressionID rootExpressionType)
	{
		Root = AddNode(rootExpressionType);
		Root.Pos = GameConfig.Instance.ModelingMapStartPosition;
		Debug.LogError("创建了建模地图" + Root.Pos);
	}

	public PartModelingNodeMap(List<ModelingExpressionNode> nodes)
	{
		Root = nodes[0];
		AllNodes = nodes;
	}

	public ModelingExpressionNode AddNode(ExpressionID expressionType)
	{
		ModelingExpressionNode n = new ModelingExpressionNode(expressionType);
		AllNodes.Add(n);
		UpdateSymbolAndConnection();
		return n;
	}

	public void SetNodePosition(ModelingExpressionNode node, Vector2 pos)
	{
		node.Pos = pos;
		foreach (NodeInputConnection inputConnItem in AllConnection)
		{
			//inputConnItem?.UpdateLineRenderPosition();
			if (inputConnItem.InputNode == node || inputConnItem.OutputNode == node)
			{
				inputConnItem.UpdateLineRenderPosition();
			}
		}
		//foreach (var item in node.InputConnection)
		//{
		//	item?.UpdateLinePoss();
		//}
		//foreach (var item in node.OutputConnection)
		//{
		//	item?.UpdateLinePoss();
		//}
	}

	/// <summary>
	/// 创建一个新的连接
	/// </summary>
	/// <param name="output"></param>
	/// <param name="outputIndex"></param>
	/// <param name="input"></param>
	/// <param name="inputIndex"></param>
	public void AddConnection(ModelingExpressionNode output, int outputIndex, ModelingExpressionNode input, int inputIndex)
	{
		// 移除连接在同一个入口上的其他连接
		var removeConnList = AllConnection.FindAll((NodeInputConnection itemConn) =>
		{
			return ((itemConn.InputNode == input) && (itemConn.InputNodeIOIndex == inputIndex));
				//((itemConn.OutputNode == output) && (itemConn.OutputNodeIOIndex == outputIndex));
		});
		while (removeConnList.Count > 0)
		{
			DeleteConnection(removeConnList[0]);
			removeConnList.RemoveAt(0);
		}
		//启用这个节点该引脚上的连接
		input.InputConnections[inputIndex].EnableConnection(input, inputIndex, output, outputIndex);
		AllConnection.Add(input.InputConnections[inputIndex]);
		UpdateSymbolAndConnection();
	}

	public void ClearNodeConnection(ModelingExpressionNode node)
	{ 
		AllConnection.RemoveAll(
			(NodeInputConnection connItem)=>
			{
				if (connItem.OutputNode == node)
				{
					DeleteConnection(connItem.InputNode, connItem.InputNodeIOIndex);
					return true;
				}
				return false;
			}
		);
		for (int i = 0; i < node.InputConnections.Length; i++)
		{
			DeleteConnection(node, i);
		}
	}

	public void DeleteNode(ModelingExpressionNode node)
	{
		ClearNodeConnection(node);
		AllNodes.Remove(node);
	}

	public float[] Caculate()
	{
		return Root.GetResult;
	}


	public void DeleteConnection(ModelingExpressionNode node, int inputConnectionIndex)
	{
		if (inputConnectionIndex >= node.InputConnections.Length)
		{
			Debug.LogError("删除输入连接时 索引超出长度");
		} 
		NodeInputConnection deleteConn = node.InputConnections[inputConnectionIndex];
		DeleteConnection(deleteConn);
	}

	public void DeleteConnection(NodeInputConnection deletingConn)
	{
		deletingConn.DisableConnection();
		AllConnection.Remove(deletingConn);
		UpdateSymbolAndConnection();
	}

	public void UpdateSymbolAndConnection()
	{
		Debug.LogError($"准备更新建模地图： 连接 {AllConnection.Count} 个； Node {AllNodes.Count} 个");
		foreach (var item in AllConnection)
		{
			item.UpdateLineRenderPosition();
		}
		foreach (var item in AllNodes)
		{
			item.UpdateSymbol();
		}
	}

	// ----------------//
	// --- 私有方法
	// ----------------//



	//private void DeleteOutputConnection(Node node, int inputConnIndex)
	//{
	//	NodeIOConnection conn = node.OutputConnection[inputConnIndex];
	//	NodeIOConnection[] outputConnections = conn.MasterNode.OutputConnection;
	//	for (int i = 0; i < outputConnections.Length; i++)
	//	{
	//		if (outputConnections[i] == conn)
	//		{
	//			GameObject.Destroy(outputConnections[i].ConnectLine.gameObject);
	//			outputConnections[i] = null;
	//			break;
	//		}
	//	}
	//	AllConnection.RemoveAll(a => a[0] == conn || a[1] == conn);
	//}


	// ----------------//
	// --- 类型
	// ----------------//
	
}

