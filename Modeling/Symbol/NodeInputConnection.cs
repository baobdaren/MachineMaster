using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 节点的连接数据
/// 连接始终存在，但是数据可能为空???
/// </summary>
public class NodeInputConnection
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public ModelingExpressionNode OutputNode;
	public ModelingExpressionNode InputNode;
	public int OutputNodeIOIndex;
	public int InputNodeIOIndex;
	public LineRenderer ConnectLine { set; get; }
	/// <summary>
	/// 是否正在使用这个连接
	/// 即：输入输出节点都存在
	/// </summary>
	public bool InUse{ get => (OutputNode != null) && (InputNode != null); }

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	/// <summary>
	/// 启用
	/// </summary>
	/// <param name="input"></param>
	/// <param name="inputIOIndex"></param>
	/// <param name="output"></param>
	/// <param name="outputIOIndex"></param>
	public void EnableConnection(ModelingExpressionNode input, int inputIOIndex, ModelingExpressionNode output, int outputIOIndex)
	{
		Debug.Log($"创建一个连接 输出：{output} 输出索引：{outputIOIndex}     输入：{input} 输入索引{inputIOIndex} ");
		OutputNode = output;
		InputNode = input;
		OutputNodeIOIndex = outputIOIndex;
		InputNodeIOIndex = inputIOIndex;
	}

	/// <summary>
	/// 禁用
	/// </summary>
	public void DisableConnection()
	{
		OutputNode = null;
		InputNode = null;
		if (ConnectLine)
		{
			// 这个Linerender必须立即删除
			// 当新的连接直接替换旧的输入连接时会重建，如果不删除会导致当前帧不创建新的，但是依然在下一帧删除
			GameObject.DestroyImmediate(ConnectLine.gameObject);
		}
	}

	public NodeInputConnection() { }

	public void UpdateLineRenderPosition()
	{
		// 直出折弯点距离
		Vector2 fpCheck = new Vector2(0.1f, 0);
		if (InUse && InputNode.NodeSymbol && OutputNode.NodeSymbol && ConnectLine)
		{
			Vector2 posRight = InputNode.NodeSymbol.GetIOPosition(false, InputNodeIOIndex);
			Vector2 posLeft = OutputNode.NodeSymbol.GetIOPosition(true, OutputNodeIOIndex);
			if (Mathf.Abs(posRight.x - posLeft.x) > fpCheck.magnitude*3 && Mathf.Abs(posRight.y - posLeft.y) > fpCheck.magnitude && posLeft.x < posRight.x)
			{
				Vector2 posRightSide = posRight - fpCheck;
				Vector2 posLeftSide = posLeft + fpCheck;
				ConnectLine.positionCount = 4;
				ConnectLine.SetPositions(new Vector3[] { posRight, posRightSide, posLeftSide, posLeft });
			}
			else
			{
				ConnectLine.positionCount = 2;
				ConnectLine.SetPositions(new Vector3[]
				{
					posRight, posLeft
				});
			}
		}
	}
	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//





}
