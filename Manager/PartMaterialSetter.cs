using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 零件材质设置
/// </summary>
public class PartMaterialSetter
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public static PartMaterialSetter Instance = new PartMaterialSetter();
	public BasePartConnection FocusConnection
	{
		get => _connection;
		set
		{
			//bool diff = _connection != value;
			_connection = value;
			UpdatePartMaterials();
		}
	}
	private BasePartConnection _connection;

	public bool DirtyFlag = true;

	// ----------------//
	// --- 私有成员
	// ----------------//
	public readonly Dictionary<PartMaterialTypes, Color> PartMatColor = new Dictionary<PartMaterialTypes, Color>()
	{
		[PartMaterialTypes.EditMain] = new Color(0.85f, 0.85f, 0.85f),

		[PartMaterialTypes.Connectable] = new Color(0.2f, 0.8f, 0.2f),
		[PartMaterialTypes.Unconnectable] = new Color(0.1f, 0.1f, 0.9f),
		[PartMaterialTypes.ConnectingTarget] = new Color(1, 1, 1),

		[PartMaterialTypes.Draging] = new Color(0.1f, 0.1f, 0.9f),
		[PartMaterialTypes.DragingTouched] = new Color(0.88f, 0.90f, 0.03f),
		[PartMaterialTypes.DragingOverlaped] = new Color(0.89f, 0.1f, 0.1f),
		[PartMaterialTypes.DragingOthers] = new Color(0.1f, 0.88f, 0.1f),
	};
	private const float LayerColorAlpha = 0.32f;

	private readonly Color[] LayerToBaseColor = new Color[]
	{
		new Color(0.639f, 0.850f, 0.000f, LayerColorAlpha),
		new Color(1.000f, 0.643f, 0.000f, LayerColorAlpha),
		new Color(0.090f, 0.486f, 0.690f, LayerColorAlpha)
	};
	private readonly Color LayerHighColor = new Color(0.941f, 0.988f, 1, 0.396f);


	private bool? finishedDragFlag;


	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	/// <summary>
	/// 重新设置所有零件的shader
	/// </summary>
	public void UpdatePartMaterials()
	{
		Debug.LogWarning("材质修改 当前玩家零件个数" + PlayerPartManager.Instance.AllPlayerPartCtrls.Count);
		//if (DisplayingState == DisplayingStates.Closed) return;
		//if (PartManager.Instance.IsDraging)
		//{
		//	finishedDragFlag = false;
		//}
		//if (finishedDragFlag == false && PartManager.Instance.IsDraging == false) // 上一帧正在拖拽，这一帧没有拖拽
		//{
		//	finishedDragFlag = true;
		//}
		// 遍历场景零件和玩家零件
		for (int i = 0; i < PartManager.Instance.AllPartCount; i++)
		{
			BasePartCtrl itemSetShaderPart = PartManager.Instance.GetPart(i);
			if (ModelPlayerPartEditor.Instance.IsDirty_PartColor && ModelPlayerPartEditor.Instance.CurrentConnectCursorPos.HasValue) // 正在连接
			{
				Debug.LogWarning("连接中");
				//if (itemSetShaderPart == ModelEdit.Instance.GetConnectMain)
				if (ModelPartEditor.Instance.EditingPart == itemSetShaderPart)
				{
					SetMaterial_EditMainPart(itemSetShaderPart);
					continue;
				}
				//else if (itemSetShaderPart == ModelPlayerPartEditor.Instance.WillConnectTarget)
				//{
				//	Setmaterial_Connect_TargetPart(itemSetShaderPart);
				//	continue;
				//}
				if (ModelPlayerPartEditor.Instance.ConnectablePartType.HasValue && (itemSetShaderPart is PlayerPartCtrl) && (itemSetShaderPart as PlayerPartCtrl).MyPartType != ModelPlayerPartEditor.Instance.ConnectablePartType) // 过滤是否涉及场景零件呢？
				{
					SetMaterial_Connect_UnconnectablePart(itemSetShaderPart);
				}
				else
				{
					SetMaterial_Connect_UnconnectablePart(itemSetShaderPart);
				}
			}
			else if (PartManager.Instance.IsDraging) // 正在拖拽
			{
				//Debug.LogWarning("正在拖拽");
				//if (PartSnapManager.Instance.SnapedParts != null && PartSnapManager.Instance.SnapedParts.Count > 0) // 显示轴承已连接零件
				//{
				//	if (PartSnapManager.Instance.SnapedParts.Contains(itemSetShaderPart))
				//	{
				//		SetMaterial_EditMainPart(itemSetShaderPart);
				//	}
				//	else
				//	{
				//		SetMaterial_EditOthers(itemSetShaderPart);
				//	}
				//}

				// 编辑主零件
				if (itemSetShaderPart == ModelPartEditor.Instance.EditingPart)
				{
					SetMaterial_EditMainPart(itemSetShaderPart);
				}
				// 是拖拽对象
				else if (itemSetShaderPart == PartManager.Instance.DragingPart)
				{
					SetMaterial_Drag_DragingPart(itemSetShaderPart);
				}
				// 吸附成功过到得对象
				//else if (SnapableBase.SnapedParts != null && SnapableBase.SnapedParts == itemSetShaderPart)
				//{
				//    PartColorManager.Instance.SetMaterial_Drag_Touched(itemSetShaderPart);
				//}
				else if (PartSnapManager.Instance.SnapedParts.Contains(itemSetShaderPart))
				{
					SetMaterial_Drag_Touched(itemSetShaderPart);
				}
				else
				{
					// 被覆盖了的（忽略碰撞）
					if (itemSetShaderPart.OverlapOther(PartManager.Instance.DragingPart))
					{
						SetMaterial_Drag_Overlaped(itemSetShaderPart);
					}
					else
					{
						SetMaterial_Drag_Others(itemSetShaderPart);
					}
				}
			}
			else if(FocusConnection != null) // 显示轴承已连接零件
			{
				Debug.LogWarning("显示轴承已连接零件");
				if (FocusConnection.ConnectedParts.ContainsKey(itemSetShaderPart))
				{
					SetMaterial_EditMainPart(itemSetShaderPart);
				}
				else
				{
					SetMaterial_EditOthers(itemSetShaderPart);
				}
			}
			else if (ModelPartEditor.Instance.IsEditing) // 编辑状态
			{
				Debug.LogWarning("编辑状态");
				if(itemSetShaderPart == ModelPartEditor.Instance.EditingPart)
				{
					SetMaterial_EditMainPart(itemSetShaderPart);
				}
				else
				{
					//SetMaterial_Connect_UnconnectablePart(itemSetShaderPart);
					SetMaterial_EditOthers(itemSetShaderPart/*, itemSetShaderPart == ModelEdit.Instance.EditingPlayerPartCtrl*/);
				}
			}
			else // 正常纹理状态
			{
				Debug.LogWarning("正常纹理状态");
				SetMaterial_AsNormal(itemSetShaderPart);
			}
		}
	}



	/// <summary>
	/// 正在编辑的主零件
	/// </summary>
	/// <param name="partShaderCtrl"></param>
	private void SetMaterial_EditMainPart(BasePartCtrl partShaderCtrl)
	{
		partShaderCtrl.SetOutLineColor(PartMatColor[PartMaterialTypes.EditMain]);
		partShaderCtrl.SetTexActive(true);
		partShaderCtrl.SetPureColor(/*highLit ? LayerHighColor : */LayerToBaseColor[partShaderCtrl.Layer]);
	}

	private void SetMaterial_EditOthers(BasePartCtrl partShaderCtrl)
	{
		partShaderCtrl.SetTexActive(true);
		partShaderCtrl.SetPureColor(/*highLit ? LayerHighColor : */LayerToBaseColor[partShaderCtrl.Layer]);
		partShaderCtrl.SetOutLineActive(false);
	}

	private void SetMaterial_Layer_Clear(ISettableShader partShaderCtrl)
	{
		partShaderCtrl.SetPureColor(Color.clear);
	}

	/// <summary>
	/// 拖拽目标
	/// </summary>
	/// <param name="partSetShader"></param>
	private void SetMaterial_Drag_DragedTargetPart(ISettableShader partSetShader)
	{
		partSetShader.SetOutLineColor(PartMatColor[PartMaterialTypes.EditMain]);
		//partSetShader.SetTexActive(false);
	}
	private void SetMaterial_Drag_DragingPart(ISettableShader partSetShader)
	{
		partSetShader.SetOutLineColor(PartMatColor[PartMaterialTypes.Draging]);
		//partSetShader.SetTexActive(false);
	}
	private void SetMaterial_Drag_Touched(ISettableShader partSetShader)
	{
		partSetShader.SetOutLineColor(PartMatColor[PartMaterialTypes.DragingTouched]);
		//partSetShader.SetTexActive(false);
	}
	private void SetMaterial_Drag_Others(ISettableShader partSetShader)
	{
		partSetShader.SetOutLineColor(PartMatColor[PartMaterialTypes.DragingOthers]);
		//partSetShader.SetTexActive(activeTexture);
		partSetShader.SetOutLineActive(false);
	}
	private void SetMaterial_Drag_Overlaped(ISettableShader partSetShader)
	{
		partSetShader.SetOutLineColor(PartMatColor[PartMaterialTypes.DragingOverlaped]);
		//partSetShader.SetTexActive(true);
	}

	/// <summary>
	/// 连接主对象
	/// </summary>
	/// <param name="partSetShader"></param>
	private void SetMaterial_Connect_MainPart(ISettableShader partSetShader)
	{ 
		partSetShader.SetOutLineColor(PartMatColor[PartMaterialTypes.EditMain]);
		partSetShader.SetTexActive(false);
	}
	private void Setmaterial_Connect_TargetPart(ISettableShader partShaderCtrl)
	{
		partShaderCtrl.SetOutLineColor(PartMatColor[PartMaterialTypes.Connectable]);
		partShaderCtrl.SetTexActive(false);
	}
	private void SetMaterial_Connect_ConnectingPart(ISettableShader partShaderCtrl)
	{
		partShaderCtrl.SetOutLineColor(PartMatColor[PartMaterialTypes.ConnectingTarget]);
		partShaderCtrl.SetTexActive(true);
	}
	private void SetMaterial_Connect_UnconnectablePart(ISettableShader partShaderCtrl)
	{
		partShaderCtrl.SetOutLineColor(PartMatColor[PartMaterialTypes.Unconnectable]);
		partShaderCtrl.SetTexActive(false);
	}

	public void SetMaterial_ErrorTex(ISettableShader partShaderCtrl, bool active)
	{ 
		partShaderCtrl.SetTexActive(true);
		partShaderCtrl.SetErrorTex(active);
	}

	private void SetMaterial_AsNormal(ISettableShader partShaderCtrl)
	{
		partShaderCtrl.SetOutLineActive(false);
		partShaderCtrl.SetTexActive(true);
		partShaderCtrl.SetPureColor(Color.clear);
	}


	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
	public enum PartMaterialTypes
	{
		EditMain, 
		Connectable, 
		Unconnectable, 
		ConnectingTarget,

		Draging,
		DragingTouched,
		DragingOverlaped,
		DragingOthers,
	}
}
