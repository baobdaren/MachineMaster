using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 建模符号的连接器
/// </summary>
public class SymbolConnector : ResetAbleInstance<SymbolConnector>
{
	// ---------------  //
	// --  公有成员
	// ---------------  //
	public SymbolIOFace FirstSelectedSymbolIO
	{
		get => _first;
		set
		{
			Debug.LogError("设置第一个连接IO：" + value);
			_first = value;
			if (_first)
			{
				//Debug.Log($"正在连接 {_first?.name}");
				StartConnect();
			}
		}
	}
	public SymbolIOFace SecondSelectedSymbolIO
	{
		get => _second;
		set
		{
			Debug.LogError("设置第二个连接IO：" + value);
			if (value.IsInputFace == FirstSelectedSymbolIO.IsInputFace)
			{
				return;
			}
			else if (value != FirstSelectedSymbolIO)
			{
				_second = value;
			}
		}
	}
	public bool IsConnecting
	{
		get => FirstSelectedSymbolIO != null;
	}
	public bool CanConnect
	{
		get =>
			FirstSelectedSymbolIO != null &&
			SecondSelectedSymbolIO != null &&
			SecondSelectedSymbolIO.IsInputFace != FirstSelectedSymbolIO.IsInputFace &&
			SecondSelectedSymbolIO.IsOverlapMouse;
			
	}

	// ---------------  //
	// --  私有成员
	// ---------------  //
	private SymbolIOFace _second;
	private SymbolIOFace _first;
	private LineRenderer _connectingLine;

	// ---------------  //
	// --  公有方法
	// ---------------  //
	public void DisplayPartConnections(AbsProgrammablePartBase part)
	{
		foreach (var item in part.ModelMap.AllConnection)
		{
			item.UpdateLineRenderPosition();
		}
	}


	// ---------------  //
	// --  私有方法
	// ---------------  //
	private void StartConnect()
	{
		if (_connectingLine == null)
		{
			_connectingLine = ControllerModeling.Instance.CreateConnectionLineRender();
		}
		_connectingLine.gameObject.SetActive(true);
		MouseListener.Instance.OnMouseDown.AddListener(OnDrawLine);
		MouseListener.Instance.OnMouseUp.AddListener(OnMouseUp);
	}
	private void FinishConnect()
	{
		// 连接结束时，是否从接口位置抬起
		if (CanConnect)
		{
			SymbolIOFace inputFace = FirstSelectedSymbolIO.IsInputFace ? FirstSelectedSymbolIO : SecondSelectedSymbolIO;
			SymbolIOFace outputFace = !FirstSelectedSymbolIO.IsInputFace ? FirstSelectedSymbolIO : SecondSelectedSymbolIO;
			ControllerModeling.Instance.CreateNewConnection(outputFace.MySymbol.MyNode, outputFace.IOIndex, inputFace.MySymbol.MyNode, inputFace.IOIndex);
			Debug.Log("连接成功");
		}
		ClearConnectorState();
	}
	/// <summary>
	/// 重置连接器的数据
	/// </summary>
	private void ClearConnectorState()
	{
		Debug.LogError("重置");
		_first = null;
		_second = null;
		if (_connectingLine)
		{
			GameObject.DestroyImmediate(_connectingLine.gameObject);
			_connectingLine = null;
		}
		MouseListener.Instance.OnMouseDown.RemoveListener(OnDrawLine);
		MouseListener.Instance.OnMouseUp.RemoveListener(OnMouseUp);
	}
	private void SetLinePos(Vector2 startPos, Vector2 endPos)
	{
		if (_connectingLine.gameObject.activeSelf)
		{
			_connectingLine.positionCount = 2;
			_connectingLine.SetPosition(0, startPos);
			_connectingLine.SetPosition(1, endPos);
		}
	}

	private void OnMouseUp()
	{
		//Debug.Log("抬起触摸");
		FinishConnect();
	}
	private void OnDrawLine()
	{
		if (FirstSelectedSymbolIO)
		{
			//Debug.LogError("绘制连接线");
			SetLinePos(FirstSelectedSymbolIO.transform.position, CameraActor.Instance.MouseWorldPos);
			MaterialPropertyBlock block = new MaterialPropertyBlock();
			block.SetFloat("_useErrorLine", CanConnect ? 0 : 1);
			_connectingLine.SetPropertyBlock(block);
		}
	}

}
