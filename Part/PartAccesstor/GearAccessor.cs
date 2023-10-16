using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearAccessor : AbsBasePlayerPartAccessor
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
#if UNITY_EDITOR
	protected void Awake()
	{
		
	}
#endif
	// ----------------//
	// --- 公有方法
	// ----------------//
	public void SetGearDisplay(int gearSizeIndex)
	{ 
	}

	// ----------------//
	// --- 私有方法
	// ----------------//
	protected override List<Collider2D> GetCollidersForConflict(PartTypes targetPartType)
	{
		
	}

	protected override void OnBeforeInit()
	{
		
	}

	// ----------------//
	// --- 类型
	// ----------------//

}
