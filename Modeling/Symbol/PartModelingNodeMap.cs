using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// һ���ɱ����������н�ģ����/�ṹ
/// </summary>
public class PartModelingNodeMap
{
	// ----------------//
	// --- ���г�Ա
	// ----------------//

	// ----------------//
	// --- ˽�г�Ա
	// ----------------//
	private ModelingExpressionNode Root;
	// ----------------//
	// --- ���з���
	// ----------------//
	public List<ModelingExpressionNode> AllNodes = new List<ModelingExpressionNode>();
	public List<NodeInputConnection> AllConnection = new List<NodeInputConnection>();
	public PartModelingNodeMap(ExpressionID rootExpressionType)
	{
		Root = AddNode(rootExpressionType);
		Root.Pos = GameConfig.Instance.ModelingMapStartPosition;
		Debug.LogError("�����˽�ģ��ͼ" + Root.Pos);
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
	/// ����һ���µ�����
	/// </summary>
	/// <param name="output"></param>
	/// <param name="outputIndex"></param>
	/// <param name="input"></param>
	/// <param name="inputIndex"></param>
	public void AddConnection(ModelingExpressionNode output, int outputIndex, ModelingExpressionNode input, int inputIndex)
	{
		// �Ƴ�������ͬһ������ϵ���������
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
		//��������ڵ�������ϵ�����
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
			Debug.LogError("ɾ����������ʱ ������������");
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
		Debug.LogError($"׼�����½�ģ��ͼ�� ���� {AllConnection.Count} ���� Node {AllNodes.Count} ��");
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
	// --- ˽�з���
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
	// --- ����
	// ----------------//
	
}

