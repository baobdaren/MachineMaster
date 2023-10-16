using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ControllerModeling
{
    public readonly static ControllerModeling Instance = new ControllerModeling();
    private ControllerModeling() { }
    // -------------- //
    // --  公有属性
    // -------------- //
    public MainViewModeling View 
    {
        get
        {
			if(_view == null)
			{
                _view = GameObject.FindObjectOfType<MainViewModeling>();
			}
            return _view;
        } 
    }

    public ModelModeling Model => ModelModeling.Instance;

    // -------------- //
    // --  私有属性
    // -------------- //
    private MainViewModeling _view;


    // -------------- //
    // --  私有方法
    // -------------- //
    private void DestroyAllCreatedObjects()
    {
        Debug.LogError("清空旧的建模图");
        Model.SymbolIOToConnection.Clear();
        Model.CreatedSymbolToNode.Clear();
        GameObject.DestroyImmediate(View.ModelingMapParent);
    }

    // -------------- //
    // --  共有方法
    // -------------- //
    /// <summary>
    /// 创建新零件的所有节点符号
    /// </summary>
    /// <param name="partCtrl"></param>
    public void DisplayModelMap(PlayerPartCtrl partCtrl)
    {
        // 如果和之前的零件不是同一零件，则删除旧零件内容
		if (partCtrl != Model.UsingPart)
		{
			DestroyAllCreatedObjects();
		}
		Model.UsingPart = partCtrl;
        foreach (ModelingExpressionNode nodeItem in Model.UsingPart.ModelMap.AllNodes)
		{
			if (nodeItem.NodeSymbol == null)
			{
                nodeItem.NodeSymbol = CreateNodeSymbol(nodeItem);
			}
        }
		foreach (var nodeItem in Model.UsingPart.ModelMap.AllNodes)
		{
            nodeItem.Pos = nodeItem.Pos;
            nodeItem.UpdateSymbol();
		}
        foreach (NodeInputConnection connItem in Model.UsingPart.ModelMap.AllConnection)
		{
			if (connItem.ConnectLine == null)
			{
                connItem.ConnectLine = CreateConnectionLineRender();
			}
            connItem.UpdateLineRenderPosition();
		}
        Debug.LogError($" 即将编辑新零件 { Model.UsingPart } 节点共有 { Model.UsingPart.ModelMap.AllNodes.Count} 个 连接共有 { Model.UsingPart.ModelMap.AllConnection.Count} 个");
    }

    /// <summary>
    /// 创建新的节点和符号
    /// </summary>
    /// <param name="expressionType"></param>
    public void CreateNewNodeSymbol(ExpressionID expressionType)
    {
        var node = Model.UsingPart.ModelMap.AddNode(expressionType);
        Symbol createdNodeSymbol = CreateNodeSymbol(node);
        node.NodeSymbol = createdNodeSymbol;
    }
    /// <summary>
    /// 创建已存在的节点的符号
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public Symbol CreateNodeSymbol(ModelingExpressionNode node)
    {
        Symbol symbol = GameObject.Instantiate(GameConfig.Instance.SymbolPrefab, View.ModelingMapParent.transform).GetComponent<Symbol>();
        symbol.name = string.Format($"Symbol:{symbol.GetHashCode()};Node:{node.GetHashCode()}");
        Model.CreatedSymbolToNode.Add(symbol, node);
        symbol.InitInterfaces();
        symbol.Title = node.Expression.FuncDetail;
        return symbol;
    }

    public void CreateNewConnection(ModelingExpressionNode output, int outputIndex, ModelingExpressionNode input, int inputIndex)
    {
        Model.UsingPart.ModelMap.AddConnection(output, outputIndex, input, inputIndex);
        DisplayModelMap(Model.UsingPart);
    }

    public void DeleteSymbol(Symbol symbol)
    {
        Model.UsingPart.ModelMap.DeleteNode(symbol.MyNode);
        Model.CreatedSymbolToNode.Remove(symbol);
        GameObject.Destroy(symbol.gameObject);
    }
 
    public void ShowDistanceSensorPreviewWin ( Symbol sensorSymbol )
    {
		if(sensorSymbol.MyExpression.SortType == ExpressionBase.SorttingType.SENSOR)
		{
            View.ShowSensorPreviewWin(sensorSymbol);
		}
    }
    /// <summary>
    /// 设置Symbol使用正在聚焦的Sensor
    /// </summary>
    public void SetSymbolSensor ( )
    {
        if(Model.CreatingSensorSymbol != null && Model.CurPreviewSensor != null)
        {
            SensorManager.Instance.RegistSymbolUsedSensor(Model.CreatingSensorSymbol,Model.CurPreviewSensor,  (Model.CreatingSensorSymbol.MyExpression as ExpressionSensorBase).UsingSensor<BaseSensor>());
            (Model.CreatingSensorSymbol.MyExpression as ExpressionSensorBase).Sensor = Model.CurPreviewSensor;
        }
    }
    public void SwitchSelectSensor ( bool next)
    {
        string findSensorType = (ModelModeling.Instance.CreatingSensorSymbol.MyExpression as ExpressionSensorBase).GetSensorType;
        // 还没预览过，或者上一个预览对象和下一个类型不同
		if(Model.CurPreviewSensor == null || Model.CurPreviewSensor.GetType().ToString()!=findSensorType)
		{
            View.ShowErrorTips(!SensorManager.Instance.GetFirstSensor(findSensorType, out Model.CurPreviewSensor));
        }
		else
		{
            SensorManager.Instance.GetNextSensors(Model.CurPreviewSensor, next, out Model.CurPreviewSensor);
		}
		if(Model.CurPreviewSensor)
		{
			UIManager.RenderTextureCamera.transform.DOMove(Vector3.back * 10 + Model.CurPreviewSensor.transform.position, 0.6f);
		}
	}

    public LineRenderer CreateConnectionLineRender(string name = "ConntectLine")
    {
        LineRenderer lineRender = new GameObject(name).AddComponent<LineRenderer>();
        lineRender.sortingLayerID = SortingLayer.NameToID("ForentEnv1");
        lineRender.receiveShadows = false;
        lineRender.startWidth = 0.05f;
        lineRender.alignment = LineAlignment.TransformZ;
        lineRender.numCornerVertices = 10;
        lineRender.numCapVertices = 2;
        lineRender.material = MaterialConfig.Instance.SymbolLine;
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetFloat("_useErrorLine", 0);
        lineRender.SetPropertyBlock(block);
        lineRender.transform.SetParent(View.ModelingMapParent.transform);
        return lineRender;
    }
}
