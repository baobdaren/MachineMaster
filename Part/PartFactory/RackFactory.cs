using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RackFactory : AbsPlayerPartFactory
{
	// ----------------//
	// --- 序列化
	// ----------------//

	// ----------------//
	// --- 公有成员
	// ----------------//

	// ----------------//
	// --- 私有成员
	// ----------------//

	// ----------------//
	// --- Unity消息
	// ----------------//

	// ----------------//
	// --- 公有方法
	// ----------------//

	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override void OnSetSize(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		RackAccessor rackAccessor = accesstor as RackAccessor;
		int sectionAmount = (int)partCtrlData.Size;
		float toothEadius = rackAccessor.Sections[0].GetComponentInChildren<CircleCollider2D>().radius;
		for (int i = 0; i < rackAccessor.Sections.Length; i++)
		{
			rackAccessor.Sections[i].gameObject.SetActive(i < sectionAmount);
			if (i < sectionAmount)
			{
				rackAccessor.Sections[i].transform.SetPositionAndRotation(partCtrlData.Position + Vector2.right * toothEadius, partCtrlData.Rotation);
			}
		}
	}

	protected override void OnCreatedAsEdit(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		foreach (var item in accesstor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.AppliyEditConfig(item);
		}
		accesstor.PartDragCmpnt.EnableSnapBound = false;
	}

	protected override void OnCreatedAsPhysics(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		foreach (var item in accesstor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.ApplySimulateConfig(item);
		}
		accesstor.PartDragCmpnt.EnableSnapBound = false;
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
