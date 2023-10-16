using System.Collections.Generic;
using UnityEngine;

public class ModelPlayerPartEditor : ResetAbleInstance<ModelPlayerPartEditor>
{

	// --------------- //
	// -- 私有属性
	// --------------- //
	private PlayerPartCtrl _editingPartCtrl;
	private IConnectableCtrl willConnectTarget;
	private PartTypes? connectablePartType;

	// --------------- //
	// -- 公有属性
	// --------------- //
	public PlayerPartCtrl EditingPartCtrl
	{
		set
		{
			_editingPartCtrl = value;
			IsDirty_PartColor = true;

			PartMaterialSetter.Instance.UpdatePartMaterials();
		}
	}

	public bool IsEditingSteel = false;
	public bool IsConnecting = false;

	public bool IsEditing
	{
		get{ return _editingPartCtrl != null;}
	}
	//public bool IsEditingPlayerPart 
	//{
	//	get 
	//	{ return _editingPartCtrl is PlayerPartCtrl; }
	//}
	public PlayerPartCtrl EditingPlayerPartCtrl
	{
		get
		{return _editingPartCtrl as PlayerPartCtrl;}
	}

	//public bool IsEditingScenePart
	//{
	//	get { return _editingPartCtrl is ScenePartCtrl; }
	//}
	//public ScenePartCtrl EditingScenePartCtrl
	//{
	//	get { return _editingPartCtrl as ScenePartCtrl; }
	//}

	/// <summary>
	/// 再固定连接时，光标位置处可以被焊接的目标
	/// </summary>
	//public IConnectableCtrl WillConnectTarget
	//{
	//	private set
	//	{
	//		if (willConnectTarget != value)
	//		{
	//			IsDirty_PartColor = true;
	//			willConnectTarget = value;
	//		}

	//		PartMaterialSetter.Instance.UpdatePartMaterials();
	//	}
	//	get { return willConnectTarget; }
	//}

	public PartTypes? ConnectablePartType
	{
		get { return connectablePartType; }
		set
		{
			connectablePartType = value;
			IsDirty_PartColor = true;
		}
	}

	/// <summary>
	/// 开始连接时，当前编辑的主零件
	/// </summary>
	public IConnectableCtrl GetConnectMain => EditingPlayerPartCtrl;
	public Vector2? CurrentConnectCursorPos { get; private set; } = null;

	public bool IsDirty_PartColor
	{
		set
		{
			__partColorIsDirty = value;
			if (__partColorIsDirty) { PartMaterialSetter.Instance.UpdatePartMaterials(); }
		}
		get
		{
			return __partColorIsDirty;
		}
	}
	private bool __partColorIsDirty;

	/// <summary>
	/// 轴承显示更新标志
	/// </summary>
	public bool DirtyFlag_BearingColor
	{
		set
		{
			__bearingColorIsDirty = value;
		}
		get
		{
			return __bearingColorIsDirty;
		}
	}
	private bool __bearingColorIsDirty;

	/// <summary>
	/// 是否正在查找轴承
	/// 用于轴承图标更新
	/// </summary>
	public bool IsFindingBearing = false;


	public bool IsCursorOverlapedMainNow
	{
		get
		{
			if (CurrentConnectCursorPos.HasValue == false) return false;
			return GetConnectMain.OverlapPoint(CurrentConnectCursorPos.Value);
		}
	}
	//public bool IsCursorCanFixedConecctNow => IsCursorOverlapedMainNow && WillConnectTarget != null;
	// --------------- //
	// --- 共有方法
	// --------------- //
	public void SetConnectData(Vector2? anchorPos, IConnectableCtrl connectTarget = null)
	{
		CurrentConnectCursorPos = anchorPos;
		//WillConnectTarget = connectTarget;
		DirtyFlag_BearingColor = true;
	}

	public void ResetConnectState()
	{
		SetConnectData(null, null);
	}

	// --------------- //
	// -- 公共方法
	// --------------- //
	public void ClearDirty()
	{
		Debug.Log("Clear Dirty");
		IsDirty_PartColor = false;
	}

	// --------------- //
	// -- 类型
	// --------------- //
}
