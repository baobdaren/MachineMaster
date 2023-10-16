using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainAccessor : AbsSectionPartAccesor

{
	// ----------------//
	// --- 序列化
	// ----------------//
	[SerializeField]

	// ----------------//
	// --- 公有成员
	// ----------------//
	public float ToothRadius
	{
		get
		{
			if (!_radius.HasValue)
			{
				_radius = _originSection.GetComponentInChildren<CircleCollider2D>(true).radius;
			}
			return _radius.Value;
		}
	}
	private float? _radius;


	// ----------------//
	// --- 私有成员
	// ----------------//
	private List<Collider2D> _collidersForGearTest;

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//
	protected override List<Collider2D> GetCollidersForConflict(PartTypes targetPartType)
	{
		if (targetPartType == PartTypes.Gear)
		{
			if (_collidersForGearTest == null)
			{
				// 铰链用于测试碰撞器冲突的刚体只包含胶囊碰撞器
				_collidersForGearTest = AllColliders.FindAll((Collider2D col) => { return col is CapsuleCollider2D; });
			}
			return _collidersForGearTest;
		}
		else
		{
			return AllColliders;
		}
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override void OnBeforeInit()
	{
		SectionList.Add(_originSection);
		while (SectionList.Count < (int)PartConfig.Instance.RailConfig.SpecLower)
		{
			SectionList.Add(GameObject.Instantiate(_originSection, _originSection.transform.parent));
		}
	}

	// ----------------//
	// --- 类型
	// ----------------//

}
