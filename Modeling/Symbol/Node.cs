using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 建模节点
/// 逻辑节点，Symbol是逻辑节点的游戏表示对象
/// </summary>
[Serializable]
public class ModelingExpressionNode
{
	// ----------------//
	// --- 公有成员
	// ----------------//
	public ExpressionBase Expression
	{
		get => ExpressionConfig.Instance.GetExpression(_expressionID);
	}
	[ES3NonSerializable]
	public Vector2 Pos
	{
		set
		{
			_position = value;
			UpdateSymbol();
		}
		get
		{
			return _position;
		}
	}
	/// <summary>
	/// 节点只保存输入连接
	/// Node在游戏运行时持有，存档时则单独保存
	/// </summary>
	public NodeInputConnection[] InputConnections
	{
		get 
		{
			if (_inputConnections == null)
			{
				_inputConnections = new NodeInputConnection[ExpressionConfig.Instance.GetExpression(_expressionID).InputAmount];
				for (int i = 0; i < _inputConnections.Length; i++)
				{
					_inputConnections[i] = new NodeInputConnection();
				}
			}
			return _inputConnections;
		}
	}
	[ES3NonSerializable]
	public Symbol NodeSymbol;
	/// <summary>
	/// 获取该节点的数据结果
	/// </summary>
	public float[] GetResult
	{
		get
		{
			if (_lastRunFrameCount != Time.frameCount)
			{
				_lastRunFrameCount = Time.frameCount;
				float[] inputArgs = new float[InputConnections.Length];
				// 传感器和控制台需要从界面获取
				switch (Expression.SortType)
				{
					case ExpressionBase.SorttingType.SENSOR:
						break;
					case ExpressionBase.SorttingType.CMD:
						inputArgs = new float[] { ModelSimulate.Instance.CommandDataCache[this] };
						break;
					default:
						for (int i = 0; i < InputConnections.Length; i++)
						{
							if (InputConnections[i].InUse)
							{
								inputArgs[i] = InputConnections[i].OutputNode.GetResult[InputConnections[i].OutputNodeIOIndex];
							}
						}
						break;
				}
				_result = Expression.Caculate(inputArgs, SettingsValue);
			}
			return _result;
		}
	}

	public ExpressionSettingsValueSet SettingsValue;


	// ----------------//
	// --- 私有成员
	// ----------------//
	[ES3NonSerializable]
	private int _lastRunFrameCount = -1;
	[ES3NonSerializable]
	private float[] _result;
	private Vector2 _position;
	private ExpressionID _expressionID;
	[ES3NonSerializable] // 存档时会另外存储
	private NodeInputConnection[] _inputConnections;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public ModelingExpressionNode(ExpressionID type)
	{
		_expressionID = type;
		if (ExpressionConfig.Instance == null)
		{
			Debug.LogError("游戏配置数据对象无法访问");
		}
		// 创建Node时，就要具有设置项的值
		SettingsValue = Expression.CreateDefaultSettingsValueSet();
	}
	public ModelingExpressionNode() 
	{
		SettingsValue = Expression.CreateDefaultSettingsValueSet();
	}

	public void UpdateSymbol()
	{
		if (NodeSymbol)
		{
			NodeSymbol.transform.position = Pos;
			foreach (NodeInputConnection item in InputConnections)
			{
				if (item.InUse)
				{
					item.UpdateLineRenderPosition();
				}
			}
			if (SymbolSettingView.Instance.ListenSymbol == NodeSymbol)
			{
				SymbolSettingView.Instance.DoMove(NodeSymbol.GetCornerPosition(new Vector2(-1,1)));
			}
		}
		//如果设置参数界面存在，也需要更新
	}
	// ----------------//
	// --- 私有方法
	// ----------------//
	// ----------------//
	// --- 类型
	// ----------------//
}

/// <summary>
/// 设置列表值的集合
/// </summary>
[Serializable]
public class ExpressionSettingsValueSet
{
	public float this[int index] { get { return SettingValue[index]; } }
	public List<float> SettingValue = new List<float>();
}