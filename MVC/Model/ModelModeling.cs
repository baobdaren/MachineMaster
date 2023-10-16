using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PartModelingNodeMap;

/// <summary>
/// 管理所有可编程零件的信号图
/// </summary>
public class ModelModeling : ResetAbleInstance<ModelModeling>
{
    // ------------- //
    // --- 私有成员
    // ------------- //
    public ModelModeling()
    {
        SymbolConnector.ResetData();
    }

    // ------------- //
    // --- 公有成员
    // ------------- //
    // 正在使用的零件
    public PlayerPartCtrl UsingPart;

    // 当前预览传感器窗口所使用后的传感器
    public BaseSensor CurPreviewSensor;

    // 正在创建的传感器输出符号，单指需要预览确定的输出符号
    public Symbol CreatingSensorSymbol;

    public Dictionary<Symbol, ModelingExpressionNode> CreatedSymbolToNode = new Dictionary<Symbol, ModelingExpressionNode>();
    public Dictionary<SymbolIOFace, NodeInputConnection> SymbolIOToConnection = new Dictionary<SymbolIOFace, NodeInputConnection>();
}
