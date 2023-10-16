using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 提供零件的控制数据访问和修改，修改数据自动刷新编辑状态的零件
/// 数据修改必须全部经过该类，该类则回使用工厂以最新的数据来修改编辑状态的零件
/// </summary>
public class PlayerPartCtrl : BasePartCtrl
{
	// ----------------//
	// --- 公有成员
	// ----------------//
	public PlayerPartCtrlData GetCoreDataCopy { get => new PlayerPartCtrlData(PartCtrlData as PlayerPartCtrlData); }

	public int SectionsCount { get => PartCtrlData.SectionDataList.Count; }

	public override PartTypes MyPartType  => PartCtrlData.MyPartType; 
	public PartModelingNodeMap ModelMap
	{
		get
		{
			return ModelMapManager.Instance.CreateMap(this);
		}
	}

	//public int ColorHue
	//{
	//	get => CoreData.ColorOffset;
	//	set
	//	{
	//		CoreData.ColorOffset = value;
	//		PartSuperFactory.ModifyEditPart(this);
	//	}
	//}

	public bool IsProgrammablePart
	{
		get => MyPartType == PartTypes.Engine || MyPartType == PartTypes.JETEngine || MyPartType == PartTypes.Presser;
	}
	public bool IsSectionPart
	{
		get => MyPartType == PartTypes.Rail || MyPartType == PartTypes.Rope;
	}

	public AbsBasePlayerPartAccessor MyEditPartAccesstor;
	public PlayerPartBase MyPhysicsPart;
	public AbsBasePlayerPartAccessor MyPhysicsPartAccesstor
	{
		get
		{
			if (_physicsAccesstor == null && MyPhysicsPart != null)
			{
				_physicsAccesstor = MyPhysicsPart.GetComponentInChildren<AbsBasePlayerPartAccessor>();
				//_physicsAccesstor.MyPartCtrl = this;
				_physicsAccesstor.gameObject.layer = GameLayerManager.GetPartGameObjectLayer(Layer);
			}
			return _physicsAccesstor;
		}
	}


	public override List<Collider2D> GetEditConnectableColliders => MyEditPartAccesstor.AllConectableColliders;
	public override List<Collider2D> GetPhysicsConnectableColliders => MyPhysicsPartAccesstor.AllConectableColliders;
	public override List<Collider2D> GetIgnorableColliders => MyPhysicsPartAccesstor.AllColliders;

	protected override List<Renderer> EditRenders => MyEditPartAccesstor.AllRenders;
	protected override List<Renderer> PhysicsRenders => MyPhysicsPartAccesstor.AllRenders;

	public override bool IsPlayerPart => true;

	public override IEnumerable<Collider2D> GetPhysicsColliders => MyPhysicsPartAccesstor.AllColliders;
	public override SnapableBase PartDragCmpnt => MyEditPartAccesstor.PartDragCmpnt;
	// ----------------//
	// --- 私有成员
	// ----------------//
	private AbsBasePlayerPartAccessor _physicsAccesstor;
	private PlayerPartCtrlData _coreData { get; set; }
	protected override PartCtrlCoreData PartCtrlData => _coreData;

	public override MouseDoubleClick PartDoubleClickCmpnt => MyEditPartAccesstor.DoubleClickCmpnt;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	public PlayerPartCtrl(PartTypes t)
	{
		_coreData = new PlayerPartCtrlData(t);
	}
	/// <summary>
	/// 加载存档时需要直接从数据创建
	/// </summary>
	/// <param name="partCtrlData"></param>
	public PlayerPartCtrl(PlayerPartCtrlData partCtrlData)
	{
		_coreData = partCtrlData;
	}

	/// <summary>
	/// 主动更新数据
	/// 多节组合的零件（绳子和铰链），其每一节都需要保存位置
	/// 更新主体位置和旋转以及层级
	/// </summary>
	public void UpdateCoreDataFromAccessor()
	{
		if (IsSectionPart)
		{
			List<GameObject> sections = (MyEditPartAccesstor as AbsSectionPartAccesor).SectionList;
			PartCtrlData.SectionDataList = new List<(Vector3, Quaternion)>(sections.Count);
			for (int i = 0; i < sections.Count; i++)
			{
				PartCtrlData.SectionDataList.Add((sections[i].transform.position, sections[i].transform.rotation));
			}
		}
		//MainPosition = MyEditPartAccesstor.transform.position;
		//MainRotation = MyEditPartAccesstor.transform.rotation;
		//LayerIndex = MyEditPartAccesstor.gameObject.layer;
	}

	public void ApplyCoreDataToAccessor()
	{
		if (IsSectionPart)
		{
			List<GameObject> sections = (MyEditPartAccesstor as AbsSectionPartAccesor).SectionList;
			for (int i = 0; i < sections.Count; i++)
			{
				sections[i].transform.SetPositionAndRotation(PartCtrlData.SectionDataList[i].Item1, PartCtrlData.SectionDataList[i].Item2);
			}
		}
		MyEditPartAccesstor.transform.SetPositionAndRotation(Position, Rotation);
		//MyEditPartAccesstor.gameObject.layer = GetLayer;
		foreach (var item in MyEditPartAccesstor.GetComponentsInChildren<Transform>(true))
		{
			item.gameObject.layer = Layer;
		}
	}

	public override IEnumerable<Collider2D> GetColliders_CollisionTest(PartTypes partType)
	{
		return MyEditPartAccesstor.GetCollidersForConflictTest(partType);
	}

	public void AddSection(Vector3 pos, Quaternion rot)
	{
		PartCtrlData.SectionDataList.Add((pos, rot));
	}

	public void RemoveSection(int index) 
	{
		PartCtrlData.SectionDataList.RemoveAt(index);
	}

	public void SetSection(int index, Vector3 pos, Quaternion rot)
	{
		Debug.Assert(index < PartCtrlData.SectionDataList.Count);
		PartCtrlData.SectionDataList[index] = (pos, rot);
	}

	public (Vector3, Quaternion) GetSection(int index)
	{
		return PartCtrlData.SectionDataList[index];
	}
	public void DeltePartAccessorObject()
	{
		if(MyEditPartAccesstor != null) GameObject.DestroyImmediate(MyEditPartAccesstor.gameObject);
		if(MyPhysicsPartAccesstor!= null) GameObject.DestroyImmediate(MyPhysicsPartAccesstor.gameObject);
	}

	protected override void OnSettedCtrlData()
	{
		PlayerPartSuperFactory.ModifyPlayerEditPart(this);
	}

	// ----------------//
	// --- 私有方法
	// ----------------//

	// ----------------//
	// --- 类型
	// ----------------//
}
