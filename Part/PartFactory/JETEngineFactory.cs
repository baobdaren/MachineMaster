using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JETEngineFactory : AbsPlayerPartFactory
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
	protected override void OnCreatedAsEdit(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		foreach (var item in accesstor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.AppliyEditConfig(item);
		}
	}

	protected override void OnCreatedAsPhysics(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		foreach (var item in accesstor.AllRigids)
		{
			PartConfig.Instance.PartRigidConfig.ApplySimulateConfig(item);
		}
		accesstor.AllRigids[0].useAutoMass = false;
		accesstor.AllRigids[0].mass = 0.5f;
	}


	protected override void OnSetSize(PlayerPartCtrl partCtrlData, AbsBasePlayerPartAccessor accesstor)
	{
		
	}

	// ----------------//
	// --- 类型
	// ----------------//
}
