using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static MaterialConfig;
using static PartConnectionManager;

public class ControllerPlayerPartEditor
{
	public static ControllerPlayerPartEditor Instance { get; private set; } = new ControllerPlayerPartEditor();


	// ----------------- //
	// --  公有成员
	// ----------------- //
	[SerializeField] public class NonArgAction : UnityEvent { }
	public UnityEvent<PlayerPartCtrl> OnStartEditPlayerPart = new UnityEvent<PlayerPartCtrl>();
	public NonArgAction OnFinishEditPlayerPart = new NonArgAction();

	public bool IsCreating => Model.EditingPlayerPartCtrl != null;

	// ----------------- //
	// --  私有成员
	// ----------------- //
	private ModelPlayerPartEditor Model { get => ModelPlayerPartEditor.Instance; }
	private ControllerPlayerPartEditor()
	{
		ConnectCursor.Instance.OnMoveStart.AddListener(EnterConnectingState);
		//ConnectCursor.Instance.OnMoving.AddListener(UpdateConnectingState);
		ConnectCursor.Instance.OnMoveEnd.AddListener(OnFinishConnect);

	}

	//-------------------//
	// --- 私有方法
	//-------------------//
	/// <summary>
	/// 进入链接状态，链接光标开始工作时回调
	/// </summary>
	private void EnterConnectingState()
	{
		//Model.IsConnecting = true;
		//Model.OverlapedMain = false;
	}

	/// <summary>
	/// 结束链接状态，链接光标结束工作时回调
	/// </summary>
	private void OnFinishConnect()
	{
		Model.ResetConnectState();
		return;
		//if (state == ConnectCursor.ConnectCursorStates.Bearing||
		//	state == ConnectCursor.ConnectCursorStates.Fixed)
		//{
		//	Model.ResetConnectState();
		//}
		//LayerWorker.Instance.Wrok(false);
	}


	/// <summary>
	/// 创建界面完成
	/// </summary>
	private void EditFinish_Succeed()
	{
		if (Model.EditingPlayerPartCtrl != null)
		{
			if ((Model.EditingPlayerPartCtrl as PlayerPartCtrl).IsSectionPart)
			{
				(Model.EditingPlayerPartCtrl as PlayerPartCtrl).UpdateCoreDataFromAccessor();
			}

			//PartMaterialSetter.Instance.SetMaterial_AsNormal(Model.EditingPlayerPartCtrl);
			OnFinishEditPlayerPart?.Invoke();
			Model.EditingPartCtrl = null;
		}
		Model.ResetConnectState();
		PartMaterialSetter.Instance.UpdatePartMaterials();
	}

	/// <summary>
	/// 创建界面删除
	/// </summary>
	private void EditFinish_Delete()
	{
		PlayerPartCtrl deletePart = Model.EditingPlayerPartCtrl;
		PartSnapManager.Instance.UnRegistSnapableTarget(deletePart.MyEditPartAccesstor.PartDragCmpnt);
		PartConnectionManager.Instance.ClearPartConnection(deletePart);
		PlayerPartManager.Instance.DeletePart(deletePart);
		Model.EditingPartCtrl = null;
		OnFinishEditPlayerPart?.Invoke();
		Model.ResetConnectState();
	}

	//-------------------//
	// --- 公有方法
	//-------------------//
	/// <summary>
	/// 设置这个PART为编辑对象
	/// </summary>
	/// <param name="part"></param>
	public void SetEditPart(PlayerPartCtrl part)
	{
		Model.EditingPartCtrl = part;
		Model.DirtyFlag_BearingColor = true;
		Model.IsFindingBearing = false;
		Model.DirtyFlag_BearingColor = true;
		Model.IsConnecting = false;
		Model.IsEditingSteel = Model.EditingPlayerPartCtrl.MyPartType == PartTypes.Steel;
		if (part.IsPlayerPart == false) { Debug.Assert(false); }
		part.MyEditPartAccesstor.gameObject.SetActive(true);
		// 处理 连接图标显示
		OnStartEditPlayerPart?.Invoke(Model.EditingPlayerPartCtrl);
	}


	/////// <summary>
	/////// 设置铰接主题的数据
	/////// </summary>
	/////// <param name="connectPos"></param>
	//public void SetPhysicsPartsDisplay(bool active)
	//{
	//	ObjParentsManager.Instance.ParentOfEditParts.SetActive(active);
	//}

	///// <summary>
	///// 设置铰接目标数据
	///// </summary>
	///// <param name="targetCtrlData"></param>
	///// <param name="anchorPosition"></param>
	//public void SetConnectTarget( IConnectableCtrl targetCtrlData)
	//{
	//	if (targetCtrlData != Model.WillConnectTarget)
	//	{
	//		Debug.Log("更换铰接对象目标为" + targetCtrlData);
	//		Model.WillConnectTarget = targetCtrlData;
	//	}
	//	else
	//	{
	//		Debug.Log("设置铰接对象目标为" + targetCtrlData);
	//	}
	//}

	///// <summary>
	///// 重置铰接目标数据
	///// </summary>
	//public void ResetTargetConnectionState()
	//{
	//	Model.WillConnectTarget = null;
	//}

	/// <summary>
	/// 清除铰接目标和铰接坐标的数据
	/// </summary>
	//public void ResetConnectData()
	//{
	//	Debug.Log("清空连接数据");
	//	Model.WillConnectTarget = null;
	//	Model.AnchorPos = null;
	//	Model.OverlapedMain = null;
	//}

	/// <summary>
	/// 重置铰接主体数据
	/// </summary>
	public void DeleteMasterConnection()
	{
		PartConnectionManager.Instance.ClearPartConnection(Model.EditingPlayerPartCtrl as PlayerPartCtrl);
		//Model.MainPartData.part.ClearConnects();
	}

	public void EditFinish(bool succeed)
	{
		if (succeed)
		{
			EditFinish_Succeed();
		}
		else
		{
			EditFinish_Delete();
		}
	}

	//public void SetMasterPartHueOffset(int offset)
	//{
	//	Debug.LogError("设置颜色 - " + offset);
	//	if (Model.EditingPlayerPartCtrl != null && Model.EditingPlayerPartCtrl is PlayerPartCtrl)
	//	{
	//		(Model.EditingPlayerPartCtrl as PlayerPartCtrl).ColorHue = offset;
	//	}
	//}

}
