using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ScenePartCtrl : BasePartCtrl
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//
	public Hash128 GetID { get => _scenePart.GetPartHashID; }
	public ScenePartCtrlData GetDataClone { get => new ScenePartCtrlData(_coreData); }
	/// <summary>
	/// 暂时设定场景零件没有编辑状态和物理状态区分
	/// </summary>

	public int PartLayer => _scenePart.PartLayer;

	public bool EnableDrag => _scenePart.EnableDrag;
	public override List<Collider2D> GetEditConnectableColliders => _scenePart.ConnectableColliders;
	public override List<Collider2D> GetPhysicsConnectableColliders => _scenePart.ConnectableColliders;
	public override List<Collider2D> GetIgnorableColliders => _physicsPart.AllColliders;
	protected override List<Renderer> EditRenders => _scenePart.AllRenders;
	protected override List<Renderer> PhysicsRenders => _scenePart.AllRenders;

	public override PartTypes MyPartType => _scenePart.IsGear ? PartTypes.Gear : PartTypes.Custom;

	public override bool IsPlayerPart => false;

	public override IEnumerable<Collider2D> GetPhysicsColliders => _physicsPart.AllColliders;

	public override SnapableBase PartDragCmpnt => _scenePart.DragCmpnt;

	//public override int Layer { get => _scenePart._partLayer; set => _scenePart._partLayer = value; }
	//public override Vector2 Position 
	//{ 
	//	get => _scenePart.transform.position;
	//	set => _scenePart.transform.position = value;
	//}


	// ----------------//
	// --- 私有成员
	// ----------------//
	private ScenePart _scenePart;
	private ScenePart _physicsPart => _scenePart;
	private ScenePartCtrlData _coreData { get; set; }
	protected override PartCtrlCoreData PartCtrlData => _coreData;

	public override MouseDoubleClick PartDoubleClickCmpnt => _scenePart.DoubleClickCmpnt;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public ScenePartCtrl(ScenePart scenePart, ScenePartCtrlData initData = null)
	{ 
		_scenePart = scenePart;
		if (initData != null)
		{
			_coreData = initData;
		}	
		else
		{
			_coreData = _scenePart.GetInitCoreData;
		}
	}

	///// <summary>
	///// 加载存档时需要直接从数据创建
	///// </summary>
	///// <param name="partCtrlData"></param>
	//public ScenePartCtrl(ScenePartCtrlData partCtrlData)
	//{
	//	CoreData = partCtrlData;
	//}

	public override IEnumerable<Collider2D> GetColliders_CollisionTest(PartTypes partType)
	{
		if (partType == PartTypes.Gear && _scenePart.IsGear)
		{
			return new List<Collider2D>() { _scenePart.GearMiddleCircle };
		}
		else
		{
			return _scenePart.AllColliders;
		}
	}

	protected override void OnSettedCtrlData()
	{
		_scenePart._partLayer = PartCtrlData.Layer;  //???
		if(_scenePart.EnableDrag)
		{
			_scenePart.transform.position = PartCtrlData.Position;
		}
		//_scenePart.AllRenders
		//throw new System.NotImplementedException();
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
