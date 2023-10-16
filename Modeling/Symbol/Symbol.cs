using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static PartModelingNodeMap;

public class Symbol : MonoBehaviour
{
	// ------------ //
	// --  序列化
	// ------------ //
	[SerializeField]
	protected Transform _inputArea;
	[SerializeField]
	protected Transform _outputArea;

	[SerializeField]
	protected TextMeshPro _funcText;

	// --------------- //
	// --  公有
	// --------------- //
	public string Title
	{
		set
		{
			_funcText.text = value;
		}
	}
	public ModelingExpressionNode MyNode 
	{
		get
		{
			if (ModelModeling.Instance.CreatedSymbolToNode.ContainsKey(this) == false)
			{
				Debug.Log("不存在 - " + this.gameObject.GetHashCode());
			} 
			return ModelModeling.Instance.CreatedSymbolToNode[this];

		}
	}
	public ExpressionBase MyExpression
	{
		get => MyNode.Expression;
	}

	// ------------- //
	// --  私有属性
	// ------------- //
	protected List<SymbolIOFace> _inputInterfaces;
	protected List<SymbolIOFace> _outputInterfaces;
	[SerializeField]
	private GameObject _originInterface;

	// ------------- //
	// --  私有方法
	// ------------- //

	protected virtual void On_DoubleClick ()
	{
		if (MyNode.Expression.SortType == ExpressionBase.SorttingType.INPUT)
		{
			return;
		}
		SymbolSettingView.Instance.Display(this, MyNode.Expression.SettingItemList, MyNode.SettingsValue);
		//ModelModeling.Instance.UsingPart.ModelMap.SetPosition(MyNode, transform.position);
		On_Draging(MyNode.Pos);
	}
	private void On_Draging(Vector2 pos)
	{
		ModelModeling.Instance.UsingPart.ModelMap.SetNodePosition(MyNode, pos);
	}

	// ------------- //
	// --  Unity消息
	// ------------- //
	protected void Awake ( )
	{
		_originInterface.SetActive(false);
		GetComponentInChildren<MouseDoubleClick>().OnDoubleClick.AddListener(On_DoubleClick);
		GetComponentInChildren<SnapableBase>().OnDraging.AddListener( On_Draging);
	}

	// ------------- //
	// --  公有方法
	// ------------- //

	public Vector3 GetIOPosition(bool output, int index)
	{
		return (output ? (_outputInterfaces) : (_inputInterfaces))[index].transform.position;
	}
	//public bool Work ( )
	//{
	//	if(!WorkOver)
	//	{
	//		float[] calculateResults = Calculate(GetInputDatas());
	//		// 接收元件不保存输出数据，所以要判断输出接口长度
	//		for(int i = 0; i < calculateResults.Length && i < _outputInterfaces.Count; i++)
	//		{
	//			if(_outputInterfaces[i].IOData!=null)
	//			{
	//				_outputInterfaces[i].IOData.IOSendNum = calculateResults[i];
	//			}
	//		}
	//		WorkOver = true;
	//	}
	//	return WorkOver;
	//}
	/// <summary> 
	/// 计算不同方向上平均排列的点
	/// </summary>
	/// <returns></returns>
	public Vector2 GetCornerPosition (Vector2 dir)
	{
		//if (amount == 0)
		//{
		//	return null;
		//}
		//float width;
		//float height;
		//if (TryGetComponent<SpriteRenderer>(out var tsCmpnt))
		//{
		//	width = tsCmpnt.size.x;
		//	height = tsCmpnt.size.y;
		//}
		//else
		//{
		//	return null;
		//}
		//Vector2 bottomLeft = tsCmpnt.size/2 +  (Vector2)transform.position;
		//Vector2[] result = new Vector2[amount];
		//if(dir == Vector2.up || dir == Vector2.down)
		//{
		//	float h = dir == Vector2.down ? 0 : height;
		//	for(int i = 0; i < amount; i++)
		//	{
		//		result[i] = bottomLeft + new Vector2(((i + 1) / amount) * width, h);
		//	}
		//	return result;
		//}
		//if(dir == Vector2.left || dir == Vector2.right)
		//{
		//	float w = dir == Vector2.left ? 0 : width;
		//	for(int i = 0; i < amount; i++)
		//	{
		//		result[i] = bottomLeft + new Vector2(((i + 1) / amount) * height, w);
		//	}
		//	return result;
		//}
		//if(dir == Vector2.one)
		//{
		//	// 返回右上角
		//	return new Vector2[] { bottomLeft + new Vector2(width, height) };
		//}
		//// 返回左上角
		//return new Vector2[] { bottomLeft };

		SpriteRenderer sp = GetComponent<SpriteRenderer>();
		return (Vector2)sp.transform.position + sp.size / 2 * dir;
	}

	public void InitInterfaces ( )
	{
		while(_inputArea.childCount > 0)
			GameObject.DestroyImmediate(_inputArea.GetChild(0).gameObject);
		while(_outputArea.childCount > 0)
			GameObject.DestroyImmediate(_outputArea.GetChild(0).gameObject);
		_inputInterfaces = new List<SymbolIOFace>(MyExpression.InputAmount);
		_outputInterfaces =  new List<SymbolIOFace>(MyExpression.OutputAmount);
		int inputIndex = 0;
		int outputIndex = 0;
		while(_inputArea.childCount < _inputInterfaces.Capacity)
		{
			GameObject inputFace = GameObject.Instantiate(_originInterface, _inputArea);
			inputFace.SetActive(true);
			inputFace.GetComponent<SymbolIOFace>().InitIOFace(this, true, inputIndex++);
			_inputInterfaces.Add(inputFace.GetComponent<SymbolIOFace>());
		}
		while(_outputArea.childCount < _outputInterfaces.Capacity)
		{
			GameObject outputFace = GameObject.Instantiate(_originInterface, _outputArea);
			outputFace.SetActive(true);
			outputFace.GetComponent<SymbolIOFace>().InitIOFace(this, false, outputIndex++);
			_outputInterfaces.Add(outputFace.GetComponent<SymbolIOFace>());
		}
		PlaceChildren(_inputArea);
		PlaceChildren(_outputArea);
	}
	// --------------- //
	// -- 私有方法
	// --------------- //
	private void PlaceChildren(Transform parent)
	{
		float boxHeight = 1.8f;
		if (parent.childCount == 0)
		{
			return;
		}
		float yStep = boxHeight / (parent.transform.childCount+1);
		for (int i = 0; i < parent.transform.childCount; i++)
		{
			// y轴的位置初始为竖边的中间，这里减去竖边长的一半
			parent.transform.GetChild(i).transform.localPosition = new Vector3(0, (i + 1) * yStep - boxHeight / 2, 0);
		}
	}
}


